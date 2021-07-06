package com.example.demo;

import com.soapsample.xmlns.schemas.employmentdetailsresponse.*;
import com.sun.org.apache.xerces.internal.jaxp.datatype.XMLGregorianCalendarImpl;
import org.springframework.stereotype.Component;

import javax.xml.datatype.DatatypeConfigurationException;
import javax.xml.datatype.DatatypeFactory;
import javax.xml.datatype.XMLGregorianCalendar;
import java.util.*;

@Component
public class EmployeeEndpointRepository {

    private List<EmploymentDetailsResponseType> EmploymentDetails = new ArrayList<EmploymentDetailsResponseType>();

    public EmployeeEndpointRepository() throws DatatypeConfigurationException {
        String[] firstNames = new String[]{"Fred", "Graeme", "Jeff", "Sarah", "Mary", "Samantha", "Charles"};
        String[] surNames = new String[]{"Fibnar", "Smith", "Jones", "Jefferson", "Murphy"};
        String[] cities = new String[]{"Perth", "Darwin", "Sydney", "Adelaide", "Melbourne", "Canberra"};
        String[] streets = new String[]{"Acacia", "Swansea", "Manchester", "St Georges", "Hubert", "Balmoral", "Cardiff"};
        String[] states = new String[]{"WA", "NSW", "ACT", "VIC", "SA", "NT"};
        String[] streetTypes = new String[]{"Avenue", "Street", "Terrace", "Road"};
        String[] emails = new String[]{"gmail.com", "hotmail.com", "fibnar.com", "iifoo.com.au"};
        String[] awards = new String[]{"Best", "Rookie", "Excellence", "Sharing"};
        String[] Roles = new String[]{"Manager", "Leader", "Engineer", "Assistant"};
        String[] Positions = new String[]{"Developer", "Architect", "Intern"};
        String[] postcodes = new String[]{"1000","2000","3000","4000","6000"};

        int[] awardCount = new int[]{1, 3, 5, 7};
        int[] EmploymentDetailsCount = new int[]{1, 2, 4};

        //create 1000 employees with predictable data
        int startEmploymentNumber = 12345678;

        GregorianCalendar startHireDate = XMLGregorianCalendarImpl.createDate(2000, 6, 2, 8).toGregorianCalendar();
        GregorianCalendar startApprovalDate = XMLGregorianCalendarImpl.createDate(2000, 6, 2, 8).toGregorianCalendar();

        GregorianCalendar startEmploymentDetailsDate = XMLGregorianCalendarImpl.createDate(2000, 6, 2, 8).toGregorianCalendar();
        int[] EmploymentDetailsLength = new int [] {14, 30, 350};

        for (int i = 0; i < 1000; i++) {
            int finalPmKeys = startEmploymentNumber;
            int finalCurrent = i;
            EmploymentDetailsResponseType newEmploymentDetails = new EmploymentDetailsResponseType() {{
                setPersonalSummaryData(new PersonalSummaryDataType() {{
                    setEmailAddress(String.format("%s.%s@%s", firstNames[finalCurrent % firstNames.length], surNames[finalCurrent % surNames.length], emails[finalCurrent % emails.length]));
                    getPhone().add(new PhoneType() {{
                        setType("HOME");
                        setNumber("0476137361");
                    }});
                    setEmploymentNumber(String.format("%d", finalPmKeys));
                    setResidentialAddress(new AddressType() {{
                        setCountry("Australia");
                        setCity(cities[finalCurrent % cities.length]);
                        setPostCode(postcodes[finalCurrent % postcodes.length]);
                        setState(states[finalCurrent % states.length]);
                        getAddressLine().add(String.format("%d %s %s", finalCurrent + 1, streets[finalCurrent % streets.length], streetTypes[finalCurrent % streetTypes.length]));
                    }});
                    setPostalAddress(new AddressType() {{
                        setCountry("Australia");
                        setCity(cities[finalCurrent % cities.length]);
                        setPostCode(postcodes[finalCurrent % postcodes.length]);
                        setState(states[finalCurrent % states.length]);
                        getAddressLine().add(String.format("%d %s %s", finalCurrent + 1, streets[finalCurrent % streets.length], streetTypes[finalCurrent % streetTypes.length]));
                    }});
                }});
                setEmploymentSummaryInformation(new EmploymentSummaryInformationType() {{
                    setOriginalHireDate(AddDaysTo(startHireDate, finalCurrent));
                }});

                for (int a = 0; a < awardCount[finalCurrent % awardCount.length]; a++) {
                    int finalA = a;
                    getEmploymentAwards().add(new EmploymentAwardsType() {{
                        setApprovalDate(AddDaysTo(startApprovalDate, finalA));
                        setAwardTitle(awards[finalA % awards.length]);
                        setCode(awards[finalA % awards.length].substring(0, 1));
                    }});
                }

                for (int b = 0; b < EmploymentDetailsCount[finalCurrent % EmploymentDetailsCount.length]; b++) {
                    int finalB = b;
                    getEmploymentDetails().add(new EmploymentDetailsType() {{
                        setPosition(Positions[finalB % Positions.length]);
                        setRole(Roles[finalB % Roles.length]);
                        setStartDate(AddDaysTo(startEmploymentDetailsDate, finalB));
                        setEndDate(AddDaysTo(startEmploymentDetailsDate, finalB + EmploymentDetailsLength[finalB % EmploymentDetailsLength.length] ));
                    }});
                }
            }};
            this.EmploymentDetails.add(newEmploymentDetails);
            startEmploymentNumber = startEmploymentNumber + 1;
        }
    }

    private XMLGregorianCalendar AddDaysTo(GregorianCalendar startDate, int days) throws DatatypeConfigurationException {
        startDate.add(Calendar.DAY_OF_YEAR, days);
        return DatatypeFactory
                .newInstance().newXMLGregorianCalendar(startDate);
    }

    public Optional<EmploymentDetailsResponseType> find(String employmentNumber) {
        return this.EmploymentDetails.stream().filter(EmploymentDetailsResponseType -> EmploymentDetailsResponseType.getPersonalSummaryData().getEmploymentNumber().equals(employmentNumber)).findFirst();
    }
}
