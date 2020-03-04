
## Local Development Procedure
1. Make sure that Visual Studio solution is building and is running. 
2. Ensure that it is running the docker-compose version so that the identity server docker and the dynamo db docker run. 
3. Manually run react usig `npm run`

## Developer Depenencies
* [Docker Desktop 2.1.0.5 (40693)](https://docs.docker.com/docker-for-windows/install/)
* [Dotnet Core 3.1.101](https://dotnet.microsoft.com/download/dotnet-core/3.1)
__NOTE: You must be using Visual Studio 2019 16.4.3 or above to work with Dotnet Core 3.1.__
* [AWS CLI v1](https://docs.aws.amazon.com/cli/latest/userguide/install-windows.html#install-msi-on-windows)
* [AWS SAM CLI](https://docs.aws.amazon.com/serverless-application-model/latest/developerguide/serverless-sam-cli-install-windows.html)
* An Administrator Local Account ( Docker needs this to mount drives. )
* If you are using Visual Studio - it must be v2019 16.4.3 or above to work with the Dotnet Core 3.1 bits. 
* Node version 12.14.1 or later.

## Before you run the code for the first time!!
You must create a few docker images.  The primary reason for this was that the images change rarely, and the time to build was getting obnoxious.  
1. Run from Git Bash: `GTDToDo/Externals/MakeDnaymoDbDockerImage/BuildCustomDyanmoDbDocker.sh`

### Dynamo DB
If it is the intention of the developer to alter the Dynamo DB schema - that should require a special check out and an iterating of the docker container version so that all developers are aware of the changes.  The new code as well as the schema changes should be checked in as a single unit.  

## Logging In
>**NOTE: When prompted to “seed” the user database, choose “Y” for “yes”. This populates the user database with our “alice” and “bob” users. Their passwords are “Pass123$”.**

