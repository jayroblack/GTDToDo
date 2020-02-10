
## Developer Depenencies
* [Docker Desktop 2.1.0.5 (40693)](https://docs.docker.com/docker-for-windows/install/)
* [Dotnet Core 3.1.101](https://dotnet.microsoft.com/download/dotnet-core/3.1)
__NOTE: You must be using Visual Studio 2019 16.4.3 or above to work with Dotnet Core 3.1.__
* [AWS CLI v1](https://docs.aws.amazon.com/cli/latest/userguide/install-windows.html#install-msi-on-windows)
* [AWS SAM CLI](https://docs.aws.amazon.com/serverless-application-model/latest/developerguide/serverless-sam-cli-install-windows.html)
* An Administrator Local Account ( Docker needs this to mount drives. )
* If you are using Visual Studio - it must be v2019 16.4.3 or above to work with the Dotnet Core 3.1 bits. 
* Node version 12.14.1 or later.



## [Identity Server 4](http://docs.identityserver.io/en/latest/quickstarts/6_aspnet_identity.html) & [ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-3.1&tabs=visual-studio)
* [SqlLite Browser](https://sqlitebrowser.org/) - needed for inspecting the identity server Sql Lite Database Files. 
* Install the templates by running: 
```
dotnet new -i IdentityServer4.Templates
```
* Create an Application by:
```
dotnet new is4aspid -n IdentityServerAspNetIdentity
```
>**NOTE: When prompted to “seed” the user database, choose “Y” for “yes”. This populates the user database with our “alice” and “bob” users. Their passwords are “Pass123$”.**

