/// integrate emulator code with renderer
module Integration

open Tabs
//open Views
open Refs
open Editors
open Fable.Core.JsInterop
open Fable.Import

open EmulatorTop
open TypeChecker
open SharedTypes

let resetEmulator() =
    showVexAlert "Resetting..."
    (getHtml "out-text").innerHTML <- ""
    Editors.removeEditorDecorations currentFileTabId
    Editors.enableEditors()

//let enableTypeCheck() =
//    showVexAlert "enable type check"

//let disableTypeCheck() =
//    showVexAlert "disable type check"

//let enableSki() =
//    showVexAlert "enable SKI"

//let enableBeta() =
//    showVexAlert "enable BETA"

let getProgram () =
    textOfTId currentFileTabId
    |> List.fold (fun r s -> r + s + "\n") ""

/// Top-level simulation execute
/// If current tab is TB run TB if this is possible
let runCode () =
    let program = getProgram ()
    try
        let res = end2end currentTypeCheck currentRuntime program
        (getHtml "out-text").innerHTML <-  sprintf "%A" res
        // TODO (oliver): put this in a window like output
        showVexAlert <| getType program
    with
        // Some of the impossible cases has been triggered, or there was a stack
        // overflow.
        Failure msg -> showVexAlert <| sprintf "EXCEPTION:\n%A" msg

