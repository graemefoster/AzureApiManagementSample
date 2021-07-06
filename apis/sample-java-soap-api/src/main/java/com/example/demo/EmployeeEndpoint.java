package com.example.demo;

import com.soapsample.xmlns.schemas.employmentdetailsresponse.EmploymentDetailsResponseType;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.ws.server.endpoint.annotation.Endpoint;
import org.springframework.ws.server.endpoint.annotation.PayloadRoot;
import org.springframework.ws.server.endpoint.annotation.RequestPayload;
import org.springframework.ws.server.endpoint.annotation.ResponsePayload;
import samplesoap.xml.schema.humanresources.employee.v1_0.request.EmploymentDetailsRequestMessage;
import samplesoap.xml.schema.humanresources.employee.v1_0.response.EmploymentDetailsResponseMessage;
import samplesoap.xml.schema.humanresources.employee.v1_0.results.ProviderSystemErrorType;
import samplesoap.xml.schema.humanresources.employee.v1_0.results.ResultsType;

import java.util.Optional;

@Endpoint
public class EmployeeEndpoint {

    private static final String NAMESPACE_URI = "http://xml.samplesoap/Schema/HumanResources/Employee/V1.0/Request";
    private final EmployeeEndpointRepository repository;

    @Autowired
    public EmployeeEndpoint(EmployeeEndpointRepository repository) {
        this.repository = repository;
    }

    @PayloadRoot(namespace = NAMESPACE_URI, localPart = "EmploymentDetailsRequestMessage")
    public @ResponsePayload
    EmploymentDetailsResponseMessage handleEmploymentDetailsRequest(@RequestPayload EmploymentDetailsRequestMessage request) {

        if (request.getEmploymentDetailsRequest().getSearchKey1().getEmploymentNumber().length() != 8) {
            return new EmploymentDetailsResponseMessage() {{
                setEmploymentDetailsResponseResults(new ResultsType() {{
                    setReturnCode("ERROR");
                    setReturnType("BUSINESS_ERROR");
                    setReturnText("No employee record found");
                    setProviderSystemError(new ProviderSystemErrorType() {{
                        setReturnType("Validation");
                        setReturnCode("EmploymentNumber");
                    }});
                }});
            }};
        };

        Optional<EmploymentDetailsResponseType> result = repository.find(request.getEmploymentDetailsRequest().getSearchKey1().getEmploymentNumber());
        if (result.isPresent()) {
            return new EmploymentDetailsResponseMessage() {{
                setEmploymentDetailsResponse(result.get());
            }};
        }

        return new EmploymentDetailsResponseMessage() {{
            setEmploymentDetailsResponseResults(new ResultsType() {{
                setReturnCode("ERROR");
                setReturnType("BUSINESS_ERROR");
                setReturnText("No employee record found");
                setProviderSystemError(new ProviderSystemErrorType() {{
                    setReturnType("Business");
                    setReturnCode("1");
                }});
            }});
        }};
    }
}