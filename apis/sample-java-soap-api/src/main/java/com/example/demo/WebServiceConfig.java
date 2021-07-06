package com.example.demo;

import org.springframework.boot.web.servlet.ServletRegistrationBean;
import org.springframework.context.ApplicationContext;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.core.io.ClassPathResource;
import org.springframework.ws.config.annotation.EnableWs;
import org.springframework.ws.config.annotation.WsConfigurerAdapter;
import org.springframework.ws.server.EndpointInterceptor;
import org.springframework.ws.soap.SoapVersion;
import org.springframework.ws.soap.saaj.SaajSoapMessageFactory;
import org.springframework.ws.soap.security.wss4j2.Wss4jSecurityInterceptor;
import org.springframework.ws.soap.security.wss4j2.callback.SimplePasswordValidationCallbackHandler;
import org.springframework.ws.transport.http.MessageDispatcherServlet;
import org.springframework.ws.wsdl.wsdl11.SimpleWsdl11Definition;
import org.springframework.xml.xsd.SimpleXsdSchema;

import java.util.List;
import java.util.Properties;

@EnableWs
@Configuration
public class WebServiceConfig extends WsConfigurerAdapter {
    @Bean
    public ServletRegistrationBean<MessageDispatcherServlet> messageDispatcherServlet(ApplicationContext applicationContext) {
        MessageDispatcherServlet servlet = new MessageDispatcherServlet();
        servlet.setApplicationContext(applicationContext);
        servlet.setTransformWsdlLocations(true);
        return new ServletRegistrationBean<>(servlet, "/ws/*");
    }

    @Bean(name = "employees")
    public SimpleWsdl11Definition defaultWsdl11Definition() {
        SimpleWsdl11Definition wsdl = new SimpleWsdl11Definition(new ClassPathResource("WSDL/SV-Employees-V1.wsdl"));
        return wsdl;
    }

    @Override
    public void addInterceptors(List<EndpointInterceptor> interceptors) {
        interceptors.add(securityInterceptor());
    }

    public Wss4jSecurityInterceptor securityInterceptor() {
        Wss4jSecurityInterceptor security = new Wss4jSecurityInterceptor();
        security.setValidationActions("UsernameToken");
        security.setValidationCallbackHandler(securityCallbackHandler());
        return security;
    }

    public SimplePasswordValidationCallbackHandler securityCallbackHandler(){
        SimplePasswordValidationCallbackHandler callbackHandler = new SimplePasswordValidationCallbackHandler();
        Properties users = new Properties();
        users.setProperty("sysuser012345", "this-is-a-sample-password");
        callbackHandler.setUsers(users);
        return callbackHandler;
    }

    @Bean
    public SaajSoapMessageFactory messageFactory() {
        SaajSoapMessageFactory messageFactory = new SaajSoapMessageFactory();
        messageFactory.setSoapVersion(SoapVersion.SOAP_12);
        return messageFactory;
    }

    @Bean("HandlingAssertion")
    public SimpleXsdSchema schema3() {
        SimpleXsdSchema schema = new SimpleXsdSchema(
                new ClassPathResource("XSD/HandlingAssertion.xsd"));
        return schema;
    }

    @Bean("EmploymentDetailsResponseFault.V1")
    public SimpleXsdSchema schema4() {
        SimpleXsdSchema schema = new SimpleXsdSchema(
                new ClassPathResource("XSD/EmploymentDetailsFault.V1.xsd"));
        return schema;
    }

    @Bean("EmploymentDetailsRequest.V1")
    public SimpleXsdSchema schema5() {
        SimpleXsdSchema schema = new SimpleXsdSchema(
                new ClassPathResource("XSD/EmploymentDetailsRequest.V1.xsd"));
        return schema;
    }

    @Bean("EmploymentDetailsResponse.V1")
    public SimpleXsdSchema schema6() {
        SimpleXsdSchema schema = new SimpleXsdSchema(
                new ClassPathResource("XSD/EmploymentDetailsResponse.V1.xsd"));
        return schema;
    }


    @Bean("SV-EmploymentDetailsRequest.V1")
    public SimpleXsdSchema schema1() {
        SimpleXsdSchema schema = new SimpleXsdSchema(
                new ClassPathResource("XSD/SV-EmploymentDetailsRequest.V1.xsd"));
        return schema;
    }

    @Bean("SV-EmploymentDetailsResponse.V1")
    public SimpleXsdSchema schema2() {
        SimpleXsdSchema schema = new SimpleXsdSchema(
                new ClassPathResource("XSD/SV-EmploymentDetailsResponse.V1.xsd"));
        return schema;
    }

    @Bean("SV-EmploymentDetailsResults.V1")
    public SimpleXsdSchema schema7() {
        SimpleXsdSchema schema = new SimpleXsdSchema(
                new ClassPathResource("XSD/SV-EmploymentDetailsResults.V1.xsd"));
        return schema;
    }

}