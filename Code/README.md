
## Developer Depenencies
* [Docker Desktop 2.1.0.5 (40693)](https://docs.docker.com/docker-for-windows/install/)
* [Dotnet Core 3.1.101](https://dotnet.microsoft.com/download/dotnet-core/3.1)
__NOTE: You must be using Visual Studio 2019 16.4.3 or above to work with Dotnet Core 3.1.__
* [AWS CLI v1](https://docs.aws.amazon.com/cli/latest/userguide/install-windows.html#install-msi-on-windows)
* [AWS SAM CLI](https://docs.aws.amazon.com/serverless-application-model/latest/developerguide/serverless-sam-cli-install-windows.html)
* An Administrator Local Account ( Docker needs this to mount drives. )
* If you are using Visual Studio - it must be v2019 16.4.3 or above to work with the Dotnet Core 3.1 bits. 
* Node version 12.14.1 or later.

# Setup Docker Image
In order to run the tests you must build your docker image that has your base schema installed.  If you do not you will see something like this when running Integration Tests. 

![Error Running Tests](../Docs/FirstTimeRunningTests.PNG "Error")

To Fix, open up Gitbash here:  `GTDToDo\MakeDnaymoDbDockerImage`
> NOTE:  This step is going to require that you have a local admin account.  Docker does not like certain types of accounts when mounting volumes, for that reason we suggest just using a local admin account.  Open up Docker for Windows, Settings, and then unshare and reset credentials, and then share C drive only this time use your local admin account.  

> NOTE:  Be sure you have already run aws configure.  You should be able to list tables by running this command: `aws dynamodb list-tables`

Now run the command to bulid your docker image:  
```
./BuildCustomDyanmoDbDocker.sh
```