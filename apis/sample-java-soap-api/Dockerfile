FROM gradle:7.0.0-jdk8 AS build
COPY --chown=gradle:gradle . /home/gradle/src
WORKDIR /home/gradle/src
RUN gradle build --no-daemon

FROM openjdk:8-jre-slim

EXPOSE 8080

RUN mkdir /app

COPY ./applicationinsights-agent-3.0.3.jar /app/applicationinsights-agent-3.0.3.jar
COPY --from=build /home/gradle/src/build/libs/*.jar /app/spring-boot-application.jar

ENTRYPOINT ["java", "-javaagent:/app/applicationinsights-agent-3.0.3.jar", "-jar","/app/spring-boot-application.jar"]
