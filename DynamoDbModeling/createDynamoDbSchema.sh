#!/bin/sh

value=$(<ToDo-UserProjectLabel.json)
aws dynamodb create-table --endpoint-url http://localhost:8000 --cli-input-json "$value"

value=$(<ToDo-Task.json)
aws dynamodb create-table --endpoint-url http://localhost:8000 --cli-input-json "$value"

value=$(<ToDo-Task-Old.json)
aws dynamodb create-table --endpoint-url http://localhost:8000 --cli-input-json "$value"

value=$(<ToDo-Journal.json)
aws dynamodb create-table --endpoint-url http://localhost:8000 --cli-input-json "$value"
