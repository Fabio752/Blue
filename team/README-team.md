# Blue Documentation

**Blue is a typed functional language**

This documentation will contain:
- how to run the code
  - run expecto tests
  - launch GUI editor
- features of the Blue language with some code examples
  - language constructs
  - builtin functions supported
  - importing external libraries (e.g. String and List libraries)
- testing methodology
- demo code
  - List and String libraries, written in Blue
  - Lexer for Blue, written in Blue
- implementative details of our system
  - preprocessor
  - lexer
  - parser
  - type checker
  - SKI reduction engine
  - Beta reduction engine

## Run the code

### Expecto tests

Enter the `team/editor/src/Emulator/Tests` directory and run with `dotnet run`.

If you are using windows, you can also run the tests with `run-expecto-tests.bat`.

### GUI

The editor allows to run the code and see:

- its output (from print statemets)
- its reduced value
- its type

You can also easily turn on and off type checking, and choosing the reduction
engine you prefer (SKI or Beta).

After clicking the run button, you can hover the let expressions to see
a tooltip with their types!

**Windows instructions:**  
Unzip `team\editor\dist\Blue-win32-x64.zip` to a directory of your choice and run `Blue.exe`.  
Alternatively, execute `setup.bat`. It will create a new directory `Blue` and launch the Blue GUI.  
The GUI can then be started anytime by running `Blue\Blue.exe`.  


**Alternative method:**  
Enter the `team/editor` directory and follow the instructions in the README 
(like Visual2). The app can be compiled and run live or packed to then be used on Windows/Linux/macOS.

## Features

Blue is a typed functional language with two runtimes (beta reduction and combinators reduction). It also has comments, builtin types and builtin operators/functions.

### Comments:

- inline: # --> \n
- multiline: (* --> *)
  
### Builtin types

The language supports all main built-in types, as listed below:
- int
- bool
- string
- untyped sequence (seq)
- NULL ( sequence terminator )

### Builtin operators/functions
As for the builtin functions, the language is able to perform the following:
| function | symbol | Blue type |
| --- | --- | --- |
| plus | + | int -> int -> int |
| minus | - | int -> int -> int |
| multiply | * | int -> int -> int |
| divide | / | int -> int -> int |
| greater | > | int -> int -> bool |
| greater or equal to | >= | int -> int -> bool |
| less than | < | int -> int -> bool |
| less than or equal to |<= | int -> int -> bool |
| equal to | == | int -> int -> bool |
| logical and | && | bool -> bool -> bool |
| logical or | \|\| | bool -> bool -> bool |
| logical not | ! | bool -> bool |
| head | | seq -> 'a
| tail | | seq -> seq
| size | | seq -> int
| append | | 'a -> seq -> seq
| strEq | | string -> string -> bool
| ifThenElse | | bool -> 'a -> 'a -> 'a
| implode | | seq -> string
| explode | | string -> seq
| test | | 'a -> bool
| print| | 'a -> 'a

Note: you can easily test the type of each builtin by typing it in 
the Blue editor, running the code and looking at the "Type" tab.

In addition to these builtins, we created a List library with the 
most common List library, which you can import and use out of the 
box (see below).

### Libraries

Our implementation supports the inclusion of external libraries.

We implemented a couple of libraries, as a proof of concept.

#### List library
Contains the most common list libraries implemented in Blue. These are:

- listMap
- listReduce
- listFold
- listFindInt
- listFind
- listConcat
- listReverse
- listSplitAt
- listSort (with arbitrary types and comparator)
- listItem (get the item at the index provided)
- listContains

You can inspect the code in `demo-code/ListLib.s`.

#### String library

Contains:

- stringLength
- stringItem (get char at the index provided)

You can inspect the code in `demo-code/StringLib.s`.

### Functions
The language supports recursion and both anonymous and named functions, here are some practicle examples for both classes.
- Anonymus functions:
```
\x.x  # f# equivalent of `fun x -> x`
\x y.x y  # f# equivalent of `fun x y -> x y` 
```
- Named functions:
```
let name = \x. x in   # Assign a name to the lambda function.
let name x = x in     # Syntactic sugar for the named expression above.
let name x y = x y in # Gets transformed into a series of curried lambda, by the parser. Equivalent to:
let name = \x. \y. x y in 
```

## Testing

The code have been tested with around 400 tests, including:
  - Unit tests
  - End To End tests

This process has been made particularly frictionless by developing a Test library, that massively eased the job of adding new tests and allowed us to do Regression Testing.

Moreover, having a working ide to write and run Blue code in an easy and intuitive 
way allowed us to write large programs, like a lexer and List and String libraries.

The lexer (see `demo-code/lexngram.s`), correctly lexes strings, bools, ints, functions, lists as well as all the builtin functions we included in Blue.

## Demo code

Our demo codes have been mentioned a few times already:

- List library: `demo-code/ListLib.s`
- String library: `demo-code/StringLib.s`
- Blue lexer: `demo-code/lexngram.s`
- Church Numerals, Booleans, and operands: `demo-code/church.s` 

## Implementation details
The code is divided in a series of modules.

- `TestLib`: provides a basic testing library, built on top of Expecto.
- `SharedTypes`: contains the types which are reused across different modules,
like `Token`, `Ast` and error types.
- `Preprocessor`, `Tokeniser`, `Parser`, `TypeChecker`, `SKIEngine`, `BetaEngine`: these are the
core modules of our language. Their top level functions all return a
`Result<someType, ErrorT>` where `someType` is the type returning for a correct
execution of the program.

Note: the type checker is best effort for functions `Implode` and `Explode`.
Since our lists are untyped it is impractical to check the list types.

### Preprocessor

The preprocessor takes a string as the input, potentially containing the 'import' keyword which is then substituted for the desired library. As let statements are enclosed with 'in .. ni' it also appends appropriate number of 'ni' to the end.

### Tokeniser
The tokeniser module takes as input the raw string that contains the program to be run. <br>
Strings can contain escape sequences, preceeded by the classical backwards slash char: `\`.
- Escape Sequences:
  - \a : alert
  - \b : backspace
  - \f : formfeed
  - \n : newline
  - \r : carriage return
  - \t : tab
  - \v : vertical tab
  - \\\\ : backslash
  - \\" : quotation mark
  - \\' : apostrophe

The tokeniser tries to match valid tokens as it goes through the input string. The error checking performed by the tokeniser is __exclusively__ on the validity of a single token, not on the logic sense of the expression. <br>
The Tokens that the tokeniser would match and return as valid are defined as follow, using regexes. <br>
It is also specified a (non exahustive in any way) list of cases in which the program would instead throw an error or just tokenise it differently.

| Token | Regex |
| --- | ----- |
| Bool | ['true', 'false'] |
| String| [ " ][anychar, escape sequence]*[ " ] |
| Identifier |  [ _ ]* [a - z A - Z ] [a - z A - Z _ ']* |


#### Lexing examples
```
1. let a = 2 + 3
    -> [KLet; TIdentifier "a"; KEq; TLiteral (IntLit 2); TBuiltInFunc BPlus; TLiteral (IntLit 3)]

2. let a =
   let b = \x. x * 2 in
     \c. b (b c)
   in
   a 5
   -> [KLet; TIdentifier "a"; KEq; KLet; TIdentifier "b"; KEq; KLambda;         TIdentifier "x"; KDot; TIdentifier "x"; TBuiltInFunc BMult; TLiteral     (IntLit 2); KIn; KLambda; TIdentifier "c"; KDot; TIdentifier "b";        KOpenRound; TIdentifier "b"; TIdentifier "c"; KCloseRound; KIn;          TIdentifier "a"; TLiteral (IntLit 5)]
```

### Parser

The parser implementation uses parser combinators. Just two of them are needed:
- `.+.` sequential operator (matches parse rule 1 and then parse rule 2)
- `.|.` alternative operator (matches parse rule 1 or parse rule 2)

A careful implementation of the `.|.` operator allows to report the longest
match in case of parse error.

#### BNF Grammar

The BNF grammar parsed is:

```
<identifier-list> ::= TIdentifier | TIdentfier <identifier-list>
<item-exp> ::= TLit | TIdentifier | BuiltinFunc | <round-exp> | <lambda-exp> | <defn-exp> | <if-exp> | <seq-exp>
<app-exp-list> ::= <item-exp> | <item-exp> <app-exp-list>
<round-exp> ::= "(" <exp> ")"
<lambda-exp> ::= "\" <identifier-list> "." <exp>
<defn-exp> ::= "let" <identifier-list> "=" <exp> "in" <exp> "ni"
<if-exp> ::= "if" <exp> "then" <exp> "else" <exp> "fi"
<seq-exp> ::= "[" <seq-list-exp> "]"
<seq-list-exp> ::= <exp> | <exp> "," <seq-list-exp> 
<exp> ::= <app-exp-list>
```

Parser combinators allow to map this grammar definition very closely into code.
For example, the function that parses an item expression is:
```
and pItemExp pState =
    pState
    |> (pLiteral .|. pIdentifier .|. pBuiltin .|. pRoundExp .|. pIfExp .|. pSeqExp .|. pLambdaExp .|. pFuncDefExp)
```

which is very similar to the definition of an item-exp in the grammar.

#### Error reporting

The code presents some advanced error reporting features:
- when an expression fails to parse completely, the error regarding the longest
possible match is returned.
- the error contains a trace, which helps to understand the "scope" of where the
error happened. For example, the code:
```
let x = 2 in
  let y = 3 in
    .           // gives parse error 
  ni
ni
```
produces the error trace `in y in x`, while the code:

```
let x = 2 in
  let y = 3 in
    y
  ni
  .     // gives parse error 
ni
```
produces the error trace `in x` (since we are "outside" y).

(both examples are taken from tests in `ParserTest.fs`)

#### Efficiency

Efficiency issues may arise when we need to parse the same expression at the
beginning of two alternative branches. For example
`<app-exp-list> ::= <item-exp> | <item-exp> <app-exp-list>` may require you to
parse the first item-exp once, and then again if the match in the first branch
failed.

This pattern can produce an exponential parsing time, when the same task could
be performed in linear time. My parsing rules are designed in a way that avoids
these inefficiencies.

#### Currying

The parser automatically curries all function definitions and lambdas, to make
them ready-to-use in the reduction engines.

#### Operator precedence

When the parser encounters an applicative expression list, it makes sure that
binary operators are treated with the correct precedence.

#### Wroth noting test
If you are intersed in examining the tests in `ParserTest.fs` I suggest to pay
attention to the one called `Simple program`. It tests:
- curried functions and lambas,
- operators associativity,
- function application associativity.

The code it successfully parse is:
```
let x y = x + y in
    let z = \a b. a < b && z in
        x (z 1 2)
    ni
ni
```

### TypeChecker

Blue has a semi-complete Hindley Minler type system.

The type checker can type check all languange constructs, but cannot always
guarantee that untyped sequences contains the correct types.

For example `implode [1]` will fail at runtime because the beta reduction
expects a sequence of strings.

My solution is based on some key ideas:

- There exists a context with mappings from identifiers to their types (which
may be generic). This context may change at various stages of the inference
process.
- An inference function may return:
  - if successful, the type of the ast and the substitutions that has been
  applied in order to unify different types. For example, a substitution may be
  that the generic type `Gen 1` can specialised to `Base Int`.
  - if unsuccessful, the appropiate error.
- You can apply the above mentioned substutions to both types and context, as
needed.

With this in place, the resulting code has a nice recursive structure.

In the next sections, I will describe the two core functions of my type
inference implementation.

#### Unify

This function takes two types (possibly containing wildcards) and tries to unify
them into a single type. To do so, it is often necessary to substitute some
wildcards with more specialised types (possibly simply other wildcards).

If the unification succeeds, the list of necessary substitutions is returned,
otherwise it returns an error explaining why such process is not possible.

#### Apply

This function takes a list of substitutions, and apply them to a type. This is
a recursive process, since a type may contain substitutable wildcards.

### SKI Engine
The SKI runtime takes an input from the parser and returns either the evaluated result or
an error if the evaluation is unsuccessful.

The implementation is divided into three main steps each implemented in seperate functions:
- `bracketAbstract` : Peforms bracket abstraction on the input. Reduces the output of the parser and introduces 
S,K and I combinators into the tree 
- `combinatorReduc` : Does combinator reduction on the input 
- `evalBuiltin` : Evaluates all built-in functions  

The following features are supported by the SKI combinator runtime:
- Converting Lambdas into S,K,I combinators (in `bracketAbstract` function)
- Simplifying SKI combinator expressions (in `combinatorReduc` function)
- Function definitions (in `bracketAbstract` function)
- Function applications (in `bracketAbstract` function)
- Evaluating built-in functions (in `evalBuiltin` function): 
    - print  
    - implode
    - explode
    - head
    - append
    - tail
    - size
    - basic arithmetic and logical operators
- ifThenElse expressions, only evaluating a branch if needed
- Error reporting which indicates at which stage of the runtime an error occured
- Recursion for functions with **one** argument (covered in INDIV-oss1017)


### Beta Engine
The Beta Engine takes the Ast from parser as input and returns either the evaluated result or an error if the evaluation is unsuccessful.

The top level function `runAst` which sets up empty environment and calls the `evaluate` function which is the heart of the Beta Engine.
Given the current ast it evaluates and returns the result.
For clarity `functionApplication` was designed to be separate function dealing with that case.

As for the *reduction* part :
- `evaluate` function is responsible for logic of the if-statements and adding let-statements bindings to the environment.
- `functionApplication` is responsible for function application including lambdas, named functions and built-in functions

The Built-in functions are stored in a map `BuiltIn` making adding new builti-ins quite easy. For example to add modulo operator. Assuming it was added to language built-ins and the parser returns the keyword in ast, the changes to Beta Engine would be only one line :

```
    mapInputOutputBin (|INTLIT|_|) (|INTLIT|_|) (IntLit>>Literal)
         [ // int -> int -> int
            Plus, (+);
            Minus,(-);   
            Mult, (*); 
            Div,  (/);
            Mod,  (%); // NEW FUNCTION
         ]; 
```

The `mapInputOutputBin` works by getting "matcher and unpacker" to both inputs with "packer" for output with a list of pairs between built-ins and the f# functions.

