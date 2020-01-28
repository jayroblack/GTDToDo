# What is the Architecture?

Is it Hexagon?  Is it CQRS?  Is it domain driven?  Is it aspect oriented?  The answer is yes.  Let me explain.  

## Background

In my experience over the last few decades I have seen patterns and processe evolve from the Grady Booch Method / UML / Waterfall methodology to the more modern Agile aproaches of today and we have only just now begun to realize the value is some of the more modern ideas of applying Complex Systems Theory and PLE to software. 

The point is that you experiment with different patterns and practices and you stick to what works, and you attempt to innovate and modify what does not work.  

I have been havily influenced by my Heroes:  Michael Feathers, Robert C. Martin (Uncle Bob), Eric Evans, Charles Petzold, Kent Beck and Martin Fowler and Sam Evans.  Honestly too many for me to name in a single article - all of them have helped to shape my perpective regarding what makes a good architecture versus a bad architecture.  

In the end I decided that I liked Uncle Bob's definition the best:  In a Universe where change is the only constant, we can therefore judge software only on it's ability to change without the negative side effects of entropy and unecessary complecity ( I modified it a little)

## Solution / Project Structure

The Dependency Inversion principal is front and center in my design.  Logic should depend on service abstraxtions and not concrete implementation code.  This is the secret ingredient to making your code testable, resilient to change, and antifragile.  

### Core of the Hexagon, Onion, Whatever
In the center we have the Pattern Primitives.  These are common interfaces of patterns that we see creep up in SW development over and over again.


### AND THEN....
Application and it's associated external services.  In this case - I have a project for Dynamo DB - because that is my only external service, but over time this is going to change.  

NOTE:  As projects grow in size and complecity it is soetimes necessary to create a project between Application and Patterns that is named Abstractions.  We will see this happen over time with this design.  Sit tight.  The primary motiviation is to allow for standard abstractions of re occuring themes to have a home that is not in core and is not central to Application so that it can simultaneously exist in DB and in Application ( or another service )

### Last
In Hexagon we call this the Ports and Adapters Layer.  This is often one of the most difficult concepts to grasp.  Yes MVC is the entrypoint for your Application - but it is not your application.  It is just as much a service that requires abstraction as the database or email server or stream of events.  

## Tests 
So far I only have Unit Tests and Integration tests.  

## Unit Tests

Unit tests testing only a single class using mocks.  Notice I don't test every single class.  I don't test the mundane and redundant - that only slows me down when I need to make changes.  Instead I focus on logic - think of it like this - if there is a conditional logical statement like [ if, else, then, while, do, etc...] then I should write tests to ensure that it works the way I itend.  Simple delegation to other services actually introduce redundant levels of tests that are already covered in one way or another.  

## Integration Tests

In the past I have used in memory Repository objects to manage this.  But as I matured and my thinking evolved I realized the the repository pattern itself violated the SRP.  I also realized with new technology like Docker, instead of wasting precious developer time writing complex fakes - I can just use a docker image and be on my way.  

Unfortunately not all services adapt themselves to being able to be run in Docker.  But it certainly makes portability and setup of code much easier.  Time is money, and the days when we can wait 1 to 2 weeks for a developer to have his machine buiding code are long gone.  And with current technology there is really no reaso for us to cling to these old ways.  They should be purged from the earth with fire....hard.  

