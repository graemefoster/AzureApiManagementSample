﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServiceReference
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://xml.samplesoap/Schema/HumanResources/Employee/V1.0/WSDL", ConfigurationName="ServiceReference.SV_EMPLOYEES_PortType")]
    public interface SV_EMPLOYEES_PortType
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="REQUESTEMPLOYMENTDETAILS.V1", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        System.Threading.Tasks.Task<ServiceReference.EMPLOYMENTDETAILSRESPONSEV1> REQUESTEMPLOYMENTDETAILSAsync(ServiceReference.EMPLOYMENTDETAILSREQUESTV1 request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://xml.samplesoap/soa/assertions")]
    public partial class HandlingAssertionType
    {
        
        private HandlingStatementType handlingStatementField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public HandlingStatementType HandlingStatement
        {
            get
            {
                return this.handlingStatementField;
            }
            set
            {
                this.handlingStatementField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://xml.samplesoap/soa/assertions")]
    public partial class HandlingStatementType
    {
        
        private DataClassificationType dataClassificationField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public DataClassificationType DataClassification
        {
            get
            {
                return this.dataClassificationField;
            }
            set
            {
                this.dataClassificationField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://xml.samplesoap/soa/assertions")]
    public partial class DataClassificationType
    {
        
        private DataClassificationTypeClassificationLabel classificationLabelField;
        
        private bool classificationLabelFieldSpecified;
        
        private System.DateTime timeStampField;
        
        private string producerField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public DataClassificationTypeClassificationLabel ClassificationLabel
        {
            get
            {
                return this.classificationLabelField;
            }
            set
            {
                this.classificationLabelField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ClassificationLabelSpecified
        {
            get
            {
                return this.classificationLabelFieldSpecified;
            }
            set
            {
                this.classificationLabelFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime TimeStamp
        {
            get
            {
                return this.timeStampField;
            }
            set
            {
                this.timeStampField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string producer
        {
            get
            {
                return this.producerField;
            }
            set
            {
                this.producerField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://xml.samplesoap/soa/assertions")]
    public enum DataClassificationTypeClassificationLabel
    {
        
        /// <remarks/>
        PROTECTED,
        
        /// <remarks/>
        PUBLIC,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://xml.samplesoap/Schema/HumanResources/Employee/V1.0/Results")]
    public partial class ProviderSystemErrorType
    {
        
        private string returnTypeField;
        
        private string returnCodeField;
        
        private string returnTextField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string ReturnType
        {
            get
            {
                return this.returnTypeField;
            }
            set
            {
                this.returnTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string ReturnCode
        {
            get
            {
                return this.returnCodeField;
            }
            set
            {
                this.returnCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string ReturnText
        {
            get
            {
                return this.returnTextField;
            }
            set
            {
                this.returnTextField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://xml.samplesoap/Schema/HumanResources/Employee/V1.0/Results")]
    public partial class ResultsType
    {
        
        private ResultsTypeReturnType returnTypeField;
        
        private string returnCodeField;
        
        private string returnTextField;
        
        private ProviderSystemErrorType providerSystemErrorField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public ResultsTypeReturnType ReturnType
        {
            get
            {
                return this.returnTypeField;
            }
            set
            {
                this.returnTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string ReturnCode
        {
            get
            {
                return this.returnCodeField;
            }
            set
            {
                this.returnCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string ReturnText
        {
            get
            {
                return this.returnTextField;
            }
            set
            {
                this.returnTextField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public ProviderSystemErrorType ProviderSystemError
        {
            get
            {
                return this.providerSystemErrorField;
            }
            set
            {
                this.providerSystemErrorField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://xml.samplesoap/Schema/HumanResources/Employee/V1.0/Results")]
    public enum ResultsTypeReturnType
    {
        
        /// <remarks/>
        SUCCESS,
        
        /// <remarks/>
        BUSINESS_ERROR,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://xmlns.soapsample.com/schemas/EMPLOYMENTDETAILSRESPONSE.V1")]
    public partial class employmentAwardsType
    {
        
        private System.DateTime approvalDateField;
        
        private bool approvalDateFieldSpecified;
        
        private string codeField;
        
        private string awardTitleField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date", Order=0)]
        public System.DateTime approvalDate
        {
            get
            {
                return this.approvalDateField;
            }
            set
            {
                this.approvalDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool approvalDateSpecified
        {
            get
            {
                return this.approvalDateFieldSpecified;
            }
            set
            {
                this.approvalDateFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string awardTitle
        {
            get
            {
                return this.awardTitleField;
            }
            set
            {
                this.awardTitleField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://xmlns.soapsample.com/schemas/EMPLOYMENTDETAILSRESPONSE.V1")]
    public partial class employmentDetailsType
    {
        
        private string positionField;
        
        private string roleField;
        
        private System.DateTime startDateField;
        
        private System.DateTime endDateField;
        
        private bool endDateFieldSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string position
        {
            get
            {
                return this.positionField;
            }
            set
            {
                this.positionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string role
        {
            get
            {
                return this.roleField;
            }
            set
            {
                this.roleField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date", Order=2)]
        public System.DateTime startDate
        {
            get
            {
                return this.startDateField;
            }
            set
            {
                this.startDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date", Order=3)]
        public System.DateTime endDate
        {
            get
            {
                return this.endDateField;
            }
            set
            {
                this.endDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool endDateSpecified
        {
            get
            {
                return this.endDateFieldSpecified;
            }
            set
            {
                this.endDateFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://xmlns.soapsample.com/schemas/EMPLOYMENTDETAILSRESPONSE.V1")]
    public partial class employmentSummaryInformationType
    {
        
        private System.DateTime originalHireDateField;
        
        private bool originalHireDateFieldSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date", Order=0)]
        public System.DateTime originalHireDate
        {
            get
            {
                return this.originalHireDateField;
            }
            set
            {
                this.originalHireDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool originalHireDateSpecified
        {
            get
            {
                return this.originalHireDateFieldSpecified;
            }
            set
            {
                this.originalHireDateFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://xmlns.soapsample.com/schemas/EMPLOYMENTDETAILSRESPONSE.V1")]
    public partial class phoneType
    {
        
        private phoneTypeType typeField;
        
        private string numberField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public phoneTypeType type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://xmlns.soapsample.com/schemas/EMPLOYMENTDETAILSRESPONSE.V1")]
    public enum phoneTypeType
    {
        
        /// <remarks/>
        HOME,
        
        /// <remarks/>
        MOBILE,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://xmlns.soapsample.com/schemas/EMPLOYMENTDETAILSRESPONSE.V1")]
    public partial class addressType
    {
        
        private string[] addressLineField;
        
        private string cityField;
        
        private string stateField;
        
        private string postCodeField;
        
        private string countryField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("addressLine", Order=0)]
        public string[] addressLine
        {
            get
            {
                return this.addressLineField;
            }
            set
            {
                this.addressLineField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string city
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string state
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string postCode
        {
            get
            {
                return this.postCodeField;
            }
            set
            {
                this.postCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://xmlns.soapsample.com/schemas/EMPLOYMENTDETAILSRESPONSE.V1")]
    public partial class personalSummaryDataType
    {
        
        private string employmentNumberField;
        
        private addressType residentialAddressField;
        
        private addressType postalAddressField;
        
        private phoneType[] phoneField;
        
        private string emailAddressField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string EmploymentNumber
        {
            get
            {
                return this.employmentNumberField;
            }
            set
            {
                this.employmentNumberField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public addressType residentialAddress
        {
            get
            {
                return this.residentialAddressField;
            }
            set
            {
                this.residentialAddressField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public addressType postalAddress
        {
            get
            {
                return this.postalAddressField;
            }
            set
            {
                this.postalAddressField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("phone", Order=3)]
        public phoneType[] phone
        {
            get
            {
                return this.phoneField;
            }
            set
            {
                this.phoneField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string emailAddress
        {
            get
            {
                return this.emailAddressField;
            }
            set
            {
                this.emailAddressField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://xmlns.soapsample.com/schemas/EMPLOYMENTDETAILSRESPONSE.V1")]
    public partial class EmploymentDetailsResponseType
    {
        
        private personalSummaryDataType personalSummaryDataField;
        
        private employmentSummaryInformationType employmentSummaryInformationField;
        
        private employmentDetailsType[] employmentDetailsField;
        
        private employmentAwardsType[] employmentAwardsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public personalSummaryDataType personalSummaryData
        {
            get
            {
                return this.personalSummaryDataField;
            }
            set
            {
                this.personalSummaryDataField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public employmentSummaryInformationType employmentSummaryInformation
        {
            get
            {
                return this.employmentSummaryInformationField;
            }
            set
            {
                this.employmentSummaryInformationField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("employmentDetails", Order=2)]
        public employmentDetailsType[] employmentDetails
        {
            get
            {
                return this.employmentDetailsField;
            }
            set
            {
                this.employmentDetailsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("employmentAwards", Order=3)]
        public employmentAwardsType[] employmentAwards
        {
            get
            {
                return this.employmentAwardsField;
            }
            set
            {
                this.employmentAwardsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://xmlns.soapsample.com/schemas/EMPLOYMENTDETAILSREQUEST.V1")]
    public partial class searchKey2Type
    {
        
        private System.DateTime dateOfBirthField;
        
        private searchKey2TypeGender genderField;
        
        private string surnameField;
        
        private string givenNamesField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date", Order=0)]
        public System.DateTime dateOfBirth
        {
            get
            {
                return this.dateOfBirthField;
            }
            set
            {
                this.dateOfBirthField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public searchKey2TypeGender gender
        {
            get
            {
                return this.genderField;
            }
            set
            {
                this.genderField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string surname
        {
            get
            {
                return this.surnameField;
            }
            set
            {
                this.surnameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string givenNames
        {
            get
            {
                return this.givenNamesField;
            }
            set
            {
                this.givenNamesField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://xmlns.soapsample.com/schemas/EMPLOYMENTDETAILSREQUEST.V1")]
    public enum searchKey2TypeGender
    {
        
        /// <remarks/>
        F,
        
        /// <remarks/>
        M,
        
        /// <remarks/>
        U,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://xmlns.soapsample.com/schemas/EMPLOYMENTDETAILSREQUEST.V1")]
    public partial class searchKey1Type
    {
        
        private string employmentNumberField;
        
        private System.DateTime dateOfBirthField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string EmploymentNumber
        {
            get
            {
                return this.employmentNumberField;
            }
            set
            {
                this.employmentNumberField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date", Order=1)]
        public System.DateTime dateOfBirth
        {
            get
            {
                return this.dateOfBirthField;
            }
            set
            {
                this.dateOfBirthField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://xmlns.soapsample.com/schemas/EMPLOYMENTDETAILSREQUEST.V1")]
    public partial class EmploymentDetailsRequestType
    {
        
        private object itemField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("searchKey1", typeof(searchKey1Type), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("searchKey2", typeof(searchKey2Type), Order=0)]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://xml.samplesoap/Schema/HumanResources/Employee/V1.0/Request")]
    public partial class EmploymentDetailsRequestMessage
    {
        
        private EmploymentDetailsRequestType employmentDetailsRequestField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public EmploymentDetailsRequestType EmploymentDetailsRequest
        {
            get
            {
                return this.employmentDetailsRequestField;
            }
            set
            {
                this.employmentDetailsRequestField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://xml.samplesoap/Schema/HumanResources/Employee/V1.0/Response")]
    public partial class EmploymentDetailsResponseMessage
    {
        
        private EmploymentDetailsResponseType employmentDetailsResponseField;
        
        private ResultsType employmentDetailsResponseResultsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public EmploymentDetailsResponseType EmploymentDetailsResponse
        {
            get
            {
                return this.employmentDetailsResponseField;
            }
            set
            {
                this.employmentDetailsResponseField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public ResultsType EmploymentDetailsResponseResults
        {
            get
            {
                return this.employmentDetailsResponseResultsField;
            }
            set
            {
                this.employmentDetailsResponseResultsField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class EMPLOYMENTDETAILSREQUESTV1
    {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://xml.samplesoap/soa/assertions")]
        public ServiceReference.HandlingAssertionType HandlingAssertion;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://xml.samplesoap/Schema/HumanResources/Employee/V1.0/Request", Order=0)]
        public ServiceReference.EmploymentDetailsRequestMessage EmploymentDetailsRequestMessage;
        
        public EMPLOYMENTDETAILSREQUESTV1()
        {
        }
        
        public EMPLOYMENTDETAILSREQUESTV1(ServiceReference.HandlingAssertionType HandlingAssertion, ServiceReference.EmploymentDetailsRequestMessage EmploymentDetailsRequestMessage)
        {
            this.HandlingAssertion = HandlingAssertion;
            this.EmploymentDetailsRequestMessage = EmploymentDetailsRequestMessage;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class EMPLOYMENTDETAILSRESPONSEV1
    {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://xml.samplesoap/soa/assertions")]
        public ServiceReference.HandlingAssertionType HandlingAssertion;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://xml.samplesoap/Schema/HumanResources/Employee/V1.0/Response", Order=0)]
        public ServiceReference.EmploymentDetailsResponseMessage EmploymentDetailsResponseMessage;
        
        public EMPLOYMENTDETAILSRESPONSEV1()
        {
        }
        
        public EMPLOYMENTDETAILSRESPONSEV1(ServiceReference.HandlingAssertionType HandlingAssertion, ServiceReference.EmploymentDetailsResponseMessage EmploymentDetailsResponseMessage)
        {
            this.HandlingAssertion = HandlingAssertion;
            this.EmploymentDetailsResponseMessage = EmploymentDetailsResponseMessage;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public interface SV_EMPLOYEES_PortTypeChannel : ServiceReference.SV_EMPLOYEES_PortType, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public partial class SV_EMPLOYEES_PortTypeClient : System.ServiceModel.ClientBase<ServiceReference.SV_EMPLOYEES_PortType>, ServiceReference.SV_EMPLOYEES_PortType
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public SV_EMPLOYEES_PortTypeClient() : 
                base(SV_EMPLOYEES_PortTypeClient.GetDefaultBinding(), SV_EMPLOYEES_PortTypeClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.SV_EMPLOYEES_Binding.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public SV_EMPLOYEES_PortTypeClient(EndpointConfiguration endpointConfiguration) : 
                base(SV_EMPLOYEES_PortTypeClient.GetBindingForEndpoint(endpointConfiguration), SV_EMPLOYEES_PortTypeClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public SV_EMPLOYEES_PortTypeClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(SV_EMPLOYEES_PortTypeClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public SV_EMPLOYEES_PortTypeClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(SV_EMPLOYEES_PortTypeClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public SV_EMPLOYEES_PortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.EMPLOYMENTDETAILSRESPONSEV1> ServiceReference.SV_EMPLOYEES_PortType.REQUESTEMPLOYMENTDETAILSAsync(ServiceReference.EMPLOYMENTDETAILSREQUESTV1 request)
        {
            return base.Channel.REQUESTEMPLOYMENTDETAILSAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.EMPLOYMENTDETAILSRESPONSEV1> REQUESTEMPLOYMENTDETAILSAsync(ServiceReference.HandlingAssertionType HandlingAssertion, ServiceReference.EmploymentDetailsRequestMessage EmploymentDetailsRequestMessage)
        {
            ServiceReference.EMPLOYMENTDETAILSREQUESTV1 inValue = new ServiceReference.EMPLOYMENTDETAILSREQUESTV1();
            inValue.HandlingAssertion = HandlingAssertion;
            inValue.EmploymentDetailsRequestMessage = EmploymentDetailsRequestMessage;
            return ((ServiceReference.SV_EMPLOYEES_PortType)(this)).REQUESTEMPLOYMENTDETAILSAsync(inValue);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.SV_EMPLOYEES_Binding))
            {
                System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
                System.ServiceModel.Channels.TextMessageEncodingBindingElement textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
                textBindingElement.MessageVersion = System.ServiceModel.Channels.MessageVersion.CreateVersion(System.ServiceModel.EnvelopeVersion.Soap12, System.ServiceModel.Channels.AddressingVersion.None);
                result.Elements.Add(textBindingElement);
                System.ServiceModel.Channels.HttpsTransportBindingElement httpsBindingElement = new System.ServiceModel.Channels.HttpsTransportBindingElement();
                httpsBindingElement.AllowCookies = true;
                httpsBindingElement.MaxBufferSize = int.MaxValue;
                httpsBindingElement.MaxReceivedMessageSize = int.MaxValue;
                result.Elements.Add(httpsBindingElement);
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.SV_EMPLOYEES_Binding))
            {
                return new System.ServiceModel.EndpointAddress("https://localhost:8080/HumanResources/Employee/V1.0");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return SV_EMPLOYEES_PortTypeClient.GetBindingForEndpoint(EndpointConfiguration.SV_EMPLOYEES_Binding);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return SV_EMPLOYEES_PortTypeClient.GetEndpointAddress(EndpointConfiguration.SV_EMPLOYEES_Binding);
        }
        
        public enum EndpointConfiguration
        {
            
            SV_EMPLOYEES_Binding,
        }
    }
}
