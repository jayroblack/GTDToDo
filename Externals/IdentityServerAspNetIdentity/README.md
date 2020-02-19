# [Identity Server 4](http://docs.identityserver.io/en/latest/quickstarts/6_aspnet_identity.html) & [ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-3.1&tabs=visual-studio)
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

## TODO
1. Come back and add the capability to add new users on the fly with a registration process.
2. Add UI to Manage APIs and Clients / Apps.  
3. Add UI to better manage and Revoke Grants. 
4. Add UI to manage scopes.  
5. Verify Email -> Auto Verify in some cases.  ( For Testing )
    * Mail Trap

