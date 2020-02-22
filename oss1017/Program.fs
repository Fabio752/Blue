﻿open System
open Expecto

open ski_combinator
open Parser
open TokeniserStub



// TESTBENCH

let listTests = [

    "ListSize", 
    FuncApp( BuiltInFunc Size, SeqExp ( Literal (IntLit 1), SeqExp ( Literal (IntLit 2),  SeqExp ( Literal (IntLit 3), Null )))), 
    Literal (IntLit 3);

    "List Size 2",
    FuncApp( BuiltInFunc Size, SeqExp ( Literal (IntLit 1), Null)),
    Literal (IntLit 1);

    "List Head",
    FuncApp( BuiltInFunc Head, SeqExp ( Literal (IntLit 1), Null)),
    Literal (IntLit 1);

    "List tail",
    FuncApp( BuiltInFunc Tail, SeqExp ( Literal (IntLit 1), SeqExp ( Literal (IntLit 2),  SeqExp ( Literal (IntLit 3), Null )))),
    SeqExp ( Literal (IntLit 2),  SeqExp ( Literal (IntLit 3), Null ));

]

let arithmeticTests = [

    "Add",
    FuncApp( FuncApp( BuiltInFunc Plus, Literal (IntLit 4)), Literal (IntLit 2)),
    Literal (IntLit 6);
    
    "Sub",
    FuncApp( FuncApp( BuiltInFunc Minus, Literal (IntLit 4)), Literal (IntLit 2)),
    Literal (IntLit 2);

    "Mult",
    FuncApp( FuncApp( BuiltInFunc Mult, Literal (IntLit 4)), Literal (IntLit 2)),
    Literal (IntLit 8);

    "Div",
    FuncApp( FuncApp( BuiltInFunc Div, Literal (IntLit 4)), Literal (IntLit 2)),
    Literal (IntLit 2);

]

let booleanTests = [
    
    "greater (true)",
    FuncApp( FuncApp( BuiltInFunc Greater, Literal (IntLit 4)), Literal (IntLit 2)),
    Literal (BoolLit true);

    "greater (false)",
    FuncApp( FuncApp( BuiltInFunc Greater, Literal (IntLit 1)), Literal (IntLit 2)),
    Literal (BoolLit false);

    "greater equal (true)",
    FuncApp( FuncApp( BuiltInFunc GreaterEq, Literal (IntLit 4)), Literal (IntLit 4)),
    Literal (BoolLit true);

    "greater equal (false)",
    FuncApp( FuncApp( BuiltInFunc GreaterEq, Literal (IntLit 4)), Literal (IntLit 5)),
    Literal (BoolLit false);

    "less (false)",
    FuncApp( FuncApp( BuiltInFunc Less, Literal (IntLit 4)), Literal (IntLit 2)),
    Literal (BoolLit false);

    "less (true)",
    FuncApp( FuncApp( BuiltInFunc Less, Literal (IntLit 1)), Literal (IntLit 2)),
    Literal (BoolLit true);

    "less equal (flase)",
    FuncApp( FuncApp( BuiltInFunc LessEq, Literal (IntLit 4)), Literal (IntLit 2)),
    Literal (BoolLit false);

    "less equal (true)",
    FuncApp( FuncApp( BuiltInFunc LessEq, Literal (IntLit 4)), Literal (IntLit 4)),
    Literal (BoolLit true);

    "equal (false)",
    FuncApp( FuncApp( BuiltInFunc Equal, Literal (IntLit 4)), Literal (IntLit 2)),
    Literal (BoolLit false);

    "equal (true)",
    FuncApp( FuncApp( BuiltInFunc Equal, Literal (IntLit 2)), Literal (IntLit 2)),
    Literal (BoolLit true);

]

let lambdaTests = [

    "lambda testing",
    FuncApp (Lambda { LambdaParam = "x"; LambdaBody = FuncApp ( FuncApp( BuiltInFunc Plus , Identifier "x" ), Literal (IntLit 1)) } , Literal (IntLit 5)),
    Literal (IntLit 6);

    "lambda identity",
    Lambda { LambdaParam = "x" ; LambdaBody = Identifier "x" },
    Combinator I;

    "lambda passthrough",
    FuncApp (Lambda { LambdaParam = "x"; LambdaBody = Identifier "x" } , Literal (IntLit 5)),
    Literal (IntLit 5);

    "lambda x+y",
    FuncApp (FuncApp(Lambda { LambdaParam = "x"; LambdaBody = Lambda { LambdaParam = "y"; LambdaBody = FuncApp ( FuncApp( BuiltInFunc Plus , Identifier "x" ),  Identifier "y")  }},Literal (IntLit 12)),Literal (IntLit 10)),
    Literal (IntLit 22);

]

let funcDefTests = [

    "func def w/ lambdas",
    FuncDefExp {FuncName = "x"; FuncBody = buildLambda "y" (buildLambda "z" (Literal (IntLit 2))); Rest = FuncApp( FuncApp( Identifier "x", Literal (IntLit 5)), Literal (BoolLit true)) ;},
    Literal (IntLit 2);

    "unused func def and ret ID",
    FuncDefExp {FuncName = "f"; FuncBody = FuncDefExp {FuncName = "fun"; FuncBody = Literal (IntLit 2); Rest = Identifier "p";}; Rest = FuncDefExp {FuncName = "g"; FuncBody = Literal (StringLit "aaa"); Rest = Identifier "z";};},
    Identifier "z";

    "double identity lambda and ret int",
    FuncDefExp {FuncName = "f"; FuncBody =  Lambda { LambdaParam = "x" ; LambdaBody = Identifier "x" }; Rest = FuncApp( FuncApp (Identifier "f", Identifier "f"), Literal (IntLit 5));},
    Literal (IntLit 5);

    "func def w/ lambda and builtin",
    FuncDefExp {FuncName = "f"; FuncBody =  Lambda { LambdaParam = "x" ; LambdaBody = FuncApp( FuncApp( BuiltInFunc Plus, Identifier "x"), Literal (IntLit 2)) }; Rest = FuncApp( Identifier "f", Literal (IntLit 5))},
    Literal (IntLit 7);

]

let generalTests = [

    "combination of several tests",
    FuncApp( FuncApp( BuiltInFunc Equal, FuncApp( BuiltInFunc Head, SeqExp ( Literal (IntLit 2),  SeqExp ( Literal (IntLit 3), Null )))), FuncApp( FuncApp( BuiltInFunc Mult, Literal (IntLit 1)), Literal (IntLit 2))),
    Literal (BoolLit true);

    //"func app w/ missing arg",
    //FuncDefExp {FuncName = "f"; FuncBody =  Lambda { LambdaParam = "x" ; LambdaBody = FuncApp( FuncApp( BuiltInFunc Plus, Identifier "x"), Literal (IntLit 2)) }; Rest = FuncApp( Identifier "f", Literal (IntLit 5));},
    
]


let testCases = [
    listTests;
    arithmeticTests;
    booleanTests;
    lambdaTests;
    funcDefTests;
    generalTests;
]


let testEval (testName, input, expectedOutput) =
    testCase testName <| fun () ->
        Expect.equal (input |> combinatorRuntime) expectedOutput ""

let runTestLists  lst = 
    testList "Evaluator test" <| List.map testEval (List.fold (fun x y -> x @ y) [] lst)

[<Tests>]
let tests = runTestLists testCases

[<EntryPoint>]
let main argv =
    
    runTestsInAssembly defaultConfig [||] |> ignore
    //FuncDefExp {FuncName = "f"; FuncBody =  Lambda { LambdaParam = "x" ; LambdaBody = FuncApp( FuncApp( BuiltInFunc Plus, Identifier "x"), Literal (IntLit 2)) }; Rest = FuncApp( Identifier "f", Literal (IntLit 5));}
    //FuncApp (FuncApp(Lambda { LambdaParam = "x"; LambdaBody = Lambda { LambdaParam = "y"; LambdaBody = FuncApp ( FuncApp( BuiltInFunc Plus , Identifier "x" ),  Identifier "y")  }},Literal (IntLit 12)),Literal (IntLit 10))
    //|> combinatorRuntime
    //|> print
    0