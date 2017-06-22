# DynamicInterface
A library to dynamically glue an interface to one or more classes, creating a new dynamic type.

The initial version is pretty basic, but functional. 

## How it works
Pretty simple - given an interface, the code will hook up a type to the interface by looking for 
appropriate methods, properties and events with in the interface and then trying to find a match 
in the given instances. Initial verion does this using attribute metadata attached to the class 
declarations for the instance passed. Later versions will also be more dynamic and use other
approaches (TBC.)

## Why it exists
I needed to implement an interface in code that extended a current interface, but added a new 
property that represented an API Service endpoint. I was unable to change the original 
implementation, and so I added class belpers to glue the methods on to other existing services.
However this was kind of sucky, and so I wondered if I could use something like Moq to do this 
dynamically. Well, yeah, I could - buy 99% of the time it was just going to be about glueing 
code together, not defining massively comlicated mock objects for testing. So after a little 
more research, I realised I could probably achieve the same result by adding using 
System.Reflection.Emit. With a trusty decompiler and a little bit of Google-fu, I had a 
prototype working. What you now see here is that prototype "cleand up" a tiny bit.

## Should I use this in production?
Probably. YMMV. It is going to be fully testable, and so long as your code doesn't crash when
you're testing it, it should be pretty stable. But I give you no kind of waranty. 

## Why?
It was a fun little experiment one lunch time that turned in to something kind of useful. I enjoy making
stuff like this, and I hope others might find it a little useful.

## Is this all your own code?
Sure is, though a lot was gleaned from various posts on Stack Overflow, Microsoft helpfiles and 
other forums. However, the body of the code I pulled together is pretty much more than the sum of 
what I found online.

## License
2 clause BSD, even when not stated specifically. Use and abuse, but it would be nice to be attributed or 
at least mentioned. 

