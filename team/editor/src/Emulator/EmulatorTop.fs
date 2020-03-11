module EmulatorTop

open System
open Expecto
open TestLib
open Preprocessor
open Lexer
open LexerTest
open Parser
open ParserTest
open TypeChecker
open TypeCheckerTest
open BetaEngine
open TestBetaEngine
open SKIRuntime
open TestSKIRuntime
open SharedTypes

let maybeTypeCheck checkTypes ast =
    let printAndReturnUnchanged t =
        printfn "\nTYPE: %A" t |> ignore
        Ok ast // Return the unchanged ast
    if not checkTypes
    then Ok ast
    else typeCheck ast |> Result.bind printAndReturnUnchanged 

let selectRuntime runtime ast =
    if runtime // true == beta, false == ski
    then runAst ast
    else combinatorRuntime ast

let end2end checkTypes runtime input =
    input
    |> preprocess 
    |> Result.bind tokeniseT3
    |> Result.bind parse
    |> Result.bind (maybeTypeCheck checkTypes)
    |> Result.bind (selectRuntime runtime)

let getAst input =
    input
    |> tokeniseT3
    |> Result.bind parse

let getType input : string =
    input
    |> tokeniseT3
    |> Result.bind parse
    |> Result.bind typeCheck
    |> function
       | Ok t -> type2String t
       | Error (TypeCheckerError e) -> sprintf "%A" e
       | Error e -> "Build failed before type checker."

let getAstType (input : Result<Ast, ErrorT>) : string =
    input
    |> Result.bind typeCheck
    |> function
       | Ok t -> type2String t
       | Error (TypeCheckerError e) -> sprintf "%A" e
       | Error e -> "Build failed before type checker."
