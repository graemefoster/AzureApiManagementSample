using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;
using System.Threading.Tasks;
using ApiPoc.SoapHelpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ServiceReference;

// ReSharper disable once CheckNamespace
namespace ApiPoc
{
    public class ApiCaller<TChannel>
    {
        private readonly ILogger<ApiCaller<TChannel>> _logger;
        private readonly ChannelFactory<TChannel> _channelFactory;

        public ApiCaller(IOptions<ApiSettings> settings, ILogger<ApiCaller<TChannel>> logger)
        {
            _logger = logger;
            _channelFactory = new ChannelFactory<TChannel>(BuildCustomBindingForSoapApi(),
                new EndpointAddress(settings.Value.Endpoint));

            _channelFactory.Credentials.UserName.UserName = settings.Value.SystemUser;
            _channelFactory.Credentials.UserName.Password = settings.Value.SystemPassword;

            //SELF SIGNED CERT FOR DEMONSTRATION PURPOSES. NEVER DO THIS IN PRODUCTION.
            _channelFactory.Credentials.ServiceCertificate.SslCertificateAuthentication =
                new X509ServiceCertificateAuthentication()
                {
                    CertificateValidationMode = X509CertificateValidationMode.None,
                    RevocationMode = X509RevocationMode.NoCheck
                };
        }

        public async Task<TResult> Call<TResult>(Func<TChannel, Task<TResult>> apiCall) where TResult: ISoapHelper
        {
            var response = await apiCall(_channelFactory.CreateChannel());
            var genericResult = response.GetResult();
            if (genericResult.ReturnType == ResultsTypeReturnType.SUCCESS)
            {
                return response;
            }

            _logger.LogWarning("Downstream returned status {Status}", ResultsTypeReturnType.BUSINESS_ERROR);
            _logger.LogWarning("Downstream returned message {ProviderError}", JsonConvert.SerializeObject(genericResult?.ProviderSystemError));
            throw new DownstreamServiceException(genericResult!);
        }

        private static Binding BuildCustomBindingForSoapApi()
        {
            var binding = new WS2007HttpBinding(SecurityMode.TransportWithMessageCredential);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;

            // create a new binding based on the existing binding
            var customTransportSecurityBinding = new CustomBinding(binding);

            // locate the TextMessageEncodingBindingElement - that's the party guilty of the inclusion of the "To"
            var ele = customTransportSecurityBinding.Elements.FirstOrDefault(
                x => x is TextMessageEncodingBindingElement);
            if (ele != null)
            {
                // and replace it with a version with no addressing
                // replace {Soap12 (http://www.w3.org/2003/05/soap-envelope) Addressing10 (http://www.w3.org/2005/08/addressing)}
                //    with {Soap12 (http://www.w3.org/2003/05/soap-envelope) AddressingNone (http://schemas.microsoft.com/ws/2005/05/addressing/none)}
                int index = customTransportSecurityBinding.Elements.IndexOf(ele);
                var textBindingElement = new TextMessageEncodingBindingElement
                {
                    MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None)
                };
                customTransportSecurityBinding.Elements[index] = textBindingElement;
            }

            if (customTransportSecurityBinding.Elements.FirstOrDefault(x => x is TransportSecurityBindingElement) is
                TransportSecurityBindingElement ele2)
            {
                int index = customTransportSecurityBinding.Elements.IndexOf(ele2);
                var transportSecurityBindingElement = new TransportSecurityBindingElement
                {
                    MessageSecurityVersion =
                        ele2.MessageSecurityVersion, // MessageSecurityVersion.WSSecurity11WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11, //.WSSecurity11WSTrust13WSSecureConversation13WSSecurityPolicy12BasicSecurityProfile10,
                    IncludeTimestamp = false,
                    EndpointSupportingTokenParameters = {Signed = {new UserNameSecurityTokenParameters()}}
                };
                customTransportSecurityBinding.Elements[index] = transportSecurityBindingElement;
            }

            return customTransportSecurityBinding;
        }
    }
}