# Dynamo DB Test Image

When software is written well, unit tests are easily run quickly and in parallel.  

It is important that integration tests be local for 5 reasons:  

1. Quick feedback for the developer. 
2. Minimum investment in infrastructure. 
3. Ensure testing is not relying on fragile preconceived test datasets and instead, eating our own dog food and using the code we write to set up fixtures. 
4. Prevent developers from stepping all over each other. 
5. Allow new features to be developed with alternative schemas while avoiding impacting other developers, and by product allowing for there to be more experimentation and innovation. 

## Obstacle

To improve start up time, avoiding recreating the database each build / run is critical.  

## Solution

Use a docker image with a mounted volume to exeute the AWS CLI on to create the Dynamo Db schema.  Once created, then create another docker image where you copy the DB file into the image.  

Whenever you need to test - just launch that image and you are starting from a clean slate.  No matter what your tests do - none of the changes are saved.

This should work once we parallize as well - simply by having a user registry that ensure that our tests use different tenants.  