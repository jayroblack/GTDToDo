# Platform Blowout

![High Level Architecture Image]( ./HighLevelArchitecture.jpg "High Level Architecture" )

+ Authentication & Authorization 
    * Production:  Auth0 or Okta
    * Local Development: Identity Server 4 - Initialized by JSON running In Memory
    * Unit Testing: Mocked
    * Integration Testing: Identity Server 4 - Initialized by JSON running In Memory
+ Dynamo DB
    * Production:  Actual Dynamo DB Service
    * Local Development: Dynamo DB Docker - Initialized with Schema / NOT PERSISTED
        1. Initialization Script Runs at Startup to populate Data for Developer convenience.  That script uses the built in API in Application Project.  
    * Unit Testing: Mocked
    * Integration Testing: Dynamo DB Docker - Initialized with Schema / NOT PERSISTED
        1. No Initialization Script.
        2. Each Category or Fixture should set up the data for each test as needed.
+ Configuration
    * Production:  Parameter Store
    * Local Development: Hard Coded Fake
    * Unit Testing: Mocked
    * Integration Testing: Hard Coded Fake (Different Version than Local)
+ Email
    * Production:  Simple Email Service
    * Local Development: Mail Trap
    * Unit Testing: Mocked
    * Integration Testing: Mail Trap
+ UI
    * Main Site - ( Not Under Developer Control except for Sign up Page)
    * Primary Application
        * Production: Cloud Front - Versioned with Cache Buster
        * Local Development: Standard F5 Run and Debug built into Visual Studio
        * Unit Testing: JUnit? - Unknown at this time.
        * Integration Testing: N/A
+ API
    * Production:  API Gateway + Deployed Lambda (Versioned and Aliased)
    * Local Development: Standard F5 Run and Debug built into Visual Studio
    * Unit Testing: XUnit
    * Integration Testing: XUnit
* Payment Provider
    * Production:  TBD
    * Local Development: Fake
    * Unit Testing: Mocked
    * Integration Testing: Fake