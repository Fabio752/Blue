import List

### ATTEMPT TO LEX A SIMPLE LANGUAGE ###
### LANGUAGE SPECIFICATION:          ###
(*
LITERALS:
    INTEGERS [0-9]+
    STRINGS: ['][^']*[']
    BOOLS: [true,false]

BUILTINS:
    +   -   /   *   !    >   >=  <    <=    ==
  Plus Min Div Mul Not  Gr  GEq  Le   LEq  EqTo

LISTS-STRINGS
- size 
- head
- tail
- append
- implode
- explode 
- strEq

IDENTIFIER: 
    any sequence of chars
*)

# Library
let stringAppend lhs rhs =
    implode (listConcat (explode lhs) (explode rhs))
in

# Helper functions
let matchNum digits input =
    if size input == 0 
    then digits
    else 
        if listFind strEq (head input) ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"]
        then matchNum (digits+1) (tail input)
        else digits 
        fi
    fi
in

let matchString chars input =
    if size input == 0 
    then chars
    else 
        if ! (strEq (head input) "'")
        then matchString (chars+1) (tail input)
        else chars
        fi
    fi
in 

let matchId chars input =
    if size input == 0 
    then chars
    else 
        if ! (listFind strEq (head input) ["+","-","*","/","=","<",">","&","|"," ",".",",","(","[",")","]","#",""])
        then matchId (chars+1) (tail input)
        else chars
        fi
    fi
in 


# LEXER
let lexer program lst = 
    let headIs lst c =
        strEq (head lst) c
    in
    let lexNgram input lst = 
        # Base case
        if size input == 0
        then lst 
        else
        # Keywords
        if headIs input "."
        then (append "KDot") (lexNgram (tail input) lst)
        else
        if headIs input ","
        then (append "KComma") (lexNgram (tail input) lst)
        else
        if headIs input "("
        then (append "KOpenRound") (lexNgram (tail input) lst)
        else
        if headIs input ")"
        then (append "KCloseRound") (lexNgram (tail input) lst)
        else
        if headIs input "["
        then (append "KOpenSquare") (lexNgram (tail input) lst)
        else
        if headIs input "]"
        then (append "KCloseSquare") (lexNgram (tail input) lst)
        else
        if headIs input "\\" # Doesnt work
        then (append "KLambda") (lexNgram (tail input) lst)
        else
        # Arithmetics BuiltIn
        if headIs input "+" 
        then (append "BPlus") (lexNgram (tail input) lst)
        else 
        if headIs input "-" 
        then (append "BMinus") (lexNgram (tail input) lst)
        else
        if headIs input "*" 
        then (append "BMult") (lexNgram (tail input) lst)
        else 
        if headIs input "/" 
        then (append "BDiv") (lexNgram (tail input) lst)
        else 
        # Logic and Comparisons
        if headIs input "!" 
        then (append "BNot") (lexNgram (tail input) lst)
        else 
        if headIs input "&" && strEq (listItem 1 input "?") "&" 
        then let input' = tail input in  (append "BAnd") (lexNgram (tail input') lst) ni
        else 
        if headIs input "|" && strEq (listItem 1 input "?") "|" 
        then let input' = tail input in  (append "BOr") (lexNgram (tail input') lst) ni
        else 
        if headIs input ">" && strEq (listItem 1 input "?") "=" 
        then let input' = tail input in  (append "BGtEq") (lexNgram (tail input') lst) ni
        else 
        if headIs input "<" && strEq (listItem 1 input "?") "=" 
        then let input' = tail input in  (append "BLEq") (lexNgram (tail input') lst) ni
        else
        if headIs input "=" && strEq (listItem 1 input "?") "=" 
        then let input' = tail input in  (append "BEqTo") (lexNgram (tail input') lst) ni
        else 
        if headIs input ">" 
        then (append "BGt") (lexNgram (tail input) lst)
        else 
        if headIs input "<" 
        then (append "BLe") (lexNgram (tail input) lst)
        else 
        if headIs input "=" 
        then (append "Eq") (lexNgram (tail input) lst)
        else
        if listFind strEq (head input) ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"]
        then let digits = matchNum 0 input in
            (append (stringAppend "IntLit " (implode (head (listSplitAt digits input))))) 
                    (lexNgram (head (tail (listSplitAt digits input))) lst)
            ni
        else 
        if headIs input "'"
        then let chars = matchString 0 (tail input) in
            (append (stringAppend (stringAppend "StringLit '" (implode (head (listSplitAt (chars) (tail input))))) "'" )) 
                    (lexNgram (head (tail (listSplitAt (chars+2) input))) lst)
            ni
        else 
        # IGNORE WHITE SPACES
        if headIs input " " 
        then lexNgram (tail input) lst
        else
            let chars = matchId 0 input in
                let token = implode (head (listSplitAt chars input)) in
                    let rest = head (tail (listSplitAt chars input)) in
                        # NAMED FUNCTION KEYWORDS
                        if strEq token "let"
                        then (append "KLet") (lexNgram rest lst) 
                        else
                        if strEq token "in"
                        then (append "KIn") (lexNgram rest lst) 
                        else     
                        if strEq token "ni"
                        then (append "KNi") (lexNgram rest lst) 
                        else
                        # IFTHENELSE KEYWORDS
                        if strEq token "if"
                        then (append "KIf") (lexNgram rest lst) 
                        else
                        if strEq token "fi"
                        then (append "KFi") (lexNgram rest lst) 
                        else
                        if strEq token "then"
                        then (append "KThen") (lexNgram rest lst) 
                        else
                        if strEq token "else"
                        then (append "KElse") (lexNgram rest lst) 
                        else 
                        # List-String Builtins
                        if listFind strEq token ["size", "append", "head", "tail", "implode", "explode", "strEq"]
                        then (append token) (lexNgram rest lst) 
                        else 
                        # Booleans 
                        if strEq token "true" || strEq token "false"
                        then append (stringAppend "BoolLit " token) (lexNgram rest lst) 
                        else
                            append (stringAppend "Id " token) (lexNgram rest lst)
                        fi fi fi fi fi fi fi fi fi 
                    ni
                ni
            ni

         fi fi fi fi fi fi fi fi fi fi fi fi fi fi fi fi fi fi fi fi fi fi fi fi 
    in
        lexNgram (explode program) lst
    ni ni
in
    # testing with listReduce declaration
    # lexer "let listReduce f lst = let reducer f acc lst = if size lst == 0 then acc else reducer f (f acc (head lst)) (tail lst) fi in if size lst == 0 then 0 else reducer f (head lst) (tail lst) fi ni in let sum a b = a + b in listReduce sum [120, 2, 3, 4] ni ni" []
    
    # stringList test
    # lexer "['a','b','c']" []

    # factorial of 5
    lexer "let fact n = if n <= 1 then 1 else fact (n-1) * n fi in fact 5 ni" []

ni

# Helper functions
ni ni ni
# Library
ni


