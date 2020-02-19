FROM amazon/dynamodb-local
MAINTAINER jayroblack
EXPOSE 8000
ENTRYPOINT ["java"]
COPY db/ db/
USER root
RUN chown -R dynamodblocal db/
USER dynamodblocal
CMD ["-jar", "DynamoDBLocal.jar", "-sharedDb", "-dbPath", "db"]