# Identity Server 4 ( For Local Development)

## Why?
For Developement and during Integration Testing we need an environment that we completely control.  We want to avoid letting fake test accounts build up and become outdated and rarely used just taking up space.  There is a specific implementation of Identity Server 4 that uses a Json file as the initial configuration, and it also sustains an in memory instance - so we can exercise all of the APIs to create new users while avoiding any expense or clutter.  

## Implementation Strategy
