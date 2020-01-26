#!/bin/sh

# Build our Docker File Based on the DynamoDB base. 
docker build --no-cache -t jayroblack/dynamodb-local:1.0 -f DockerFile1 .

# Run the Docker File in the background
# EXTREMELY IMPORTANT THAT YOU HAVE SHARED DRIVE WITH DOCKER USING LOCAL ADMIN ACCOUNT
docker run -d --name jay-dyn -p 8000:8000 --mount type=bind,source="$(pwd)"/db,target=/db jayroblack/dynamodb-local:1.0

value=$(<ToDo-UserProjectLabel.json)
aws dynamodb create-table --endpoint-url http://localhost:8000 --cli-input-json "$value"

value=$(<ToDo-Task.json)
aws dynamodb create-table --endpoint-url http://localhost:8000 --cli-input-json "$value"

value=$(<ToDo-Task-Old.json)
aws dynamodb create-table --endpoint-url http://localhost:8000 --cli-input-json "$value"

value=$(<ToDo-Journal.json)
aws dynamodb create-table --endpoint-url http://localhost:8000 --cli-input-json "$value"

docker stop jay-dyn

#There Should Now Be a DynamoDB file in the db folder.
#This File is now going to be copied onto the image so that we can just start it up with our default schema and no mounts.
#docker build --no-cache -t jayroblack/dynamodb-local:1.1 -f DockerFile2 .

docker run -d --name jay-dyn1 -p 8000:8000 jayroblack/dynamodb-local:1.1
aws dynamodb list-tables --endpoint-url http://localhost:8000
docker stop jay-dyn1

# If All Went Well You Should see that all tables are there.  
# If you need to update your schema later - itaerate the version please!
# Additive stuff - just iterate minor
# Breaking Changes - iterate major.