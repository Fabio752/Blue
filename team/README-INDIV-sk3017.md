## Contributions

### **Beta Engine**

While in the individual part of the project the core of the Beta Engine was 
created, it was still not complete. During the team part I have added:

1. **memoisation**

the "let .. in" expressions are initially stored whole (without any processing) in 
the environment. 
First time they are evaluated, this value is memoised and replaced. 
Since the environment is now passed back form function calls I have took extra 
care to ensure the proper handling of closures (i.e. how to integrate returned 
environment with the previous one).

2. **error reporting**

added trace to error reporting which (additionally to current Ast) provides a 
better context for debugging. When error is encountered and returned the 
appropriate function is added to the trace stack.

3. **more built-in support and change to list**

Having added `Print` and `Test` built-ins to the language I have added their 
support to the Beta Engine. Additionally for the type Checker the definition of 
empty list needed to be changed which needed to be reflected in Beta Engine.

4. **more unit tests**

The new features (from 3.) were tested.


### **Print** 

I have implemented the print as a static class which contains a buffer. 
This was the only non-functional part I have written which was challenging since 
we have not discussed this part of F# much (other then from high-level principles 
of software engineering standpoint).

We have discussed other way of adding print which would be more functional and 
monadic. However we decided that clutter from adding the print buffer to each of 
the function definitions and calls as well as the effort to do it, were not worth 
it for the benefit they would have provided.


### **Preprocessor**

To support libraries I have created a Preprocessor which reads the program before 
tokeniser and replaces "import Lib" with the 'Lib' itself.
Originally it was meant to support all "*.s" files which works for the Ionide 
complier, but does not with Fable. 
We have decided that amount of work vs the reward would not be worth it after 
trying to understand how Visual manages to open files.

As all parts of our project it comes with some unit tests.

I have written parts of the 'List' library namely `listMap` and `listSort` 
function (implemented as merge sort algorithm). 


## **Group Dynamic and contributions**

The team had a good balancing of the self-prescribed tasks. We have communicated 
regularly  and if some changes required the work of the whole team, for example
adding print, all of us would work on their part - where changes to the Lexer,
Parser and so on were made usually by the person who wrote them.

