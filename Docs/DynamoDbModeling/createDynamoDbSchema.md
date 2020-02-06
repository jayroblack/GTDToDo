# DynamoDB is Schemaless
I kept trying to create a schema that would have all of my necessary values, and nothing worked.  Eventually I decided to just create the necessary fields in order to create the table and any indexes, and it worked.  This was extremely confusing.  

First of all - when looking at the documentation the CLI and Cloud Formation etc all said that I could only create fields that were of type [B, N, S] - Binary, Number or String.  However when I did research, there are actually a lot more data types that are totally legal for dynamo db.  Turns out those are the ony values that you are allowed to use in order to make a Partition Key (Hash) or Sort Key (Range) for a table or secondary index.  

Now this makes total sense - Dynamo Db is schemaless - it is not going to prevent me from adding or subtracting fields from items - as long as the required items are there for it to create it's primary and secondary structures.  Ugh.  Could have saved a lot of time if I already knew that.  Lesson Learned.  

On to the next thing.  