module E2ESKITest

open SharedTypes

let testCasesSKIE2E : (string * string * Result<Ast, SharedTypes.ErrorT>) list = [
    "Small program", "\x.\y. y x",
        Ok (FuncApp (
                FuncApp (
                    Combinator S, 
                    FuncApp ( 
                        Combinator K,
                        FuncApp (
                            Combinator S,
                            Combinator I 
                        ) 
                    ) 
                ),
                FuncApp (
                    FuncApp (
                        Combinator S,
                        FuncApp (
                            Combinator K,
                            Combinator K
                        )
                    ), 
                    Combinator I 
                ) 
        ));
    "simple addition", "let x = 10 in 
           x + x 
         ni",
    Ok (Literal (IntLit 20));
    "Curried Lambda", "\xy.42",
    Ok (FuncApp (Combinator K, Literal (IntLit 42)));
    "ListMap library function", "let listMap f lst = 
                                if size lst == 0
                                then []
                                else append (f (head lst))
                                     (listMap f (tail lst))
                                fi
                              in 
                              listMap (\x.x+1)[1, 2, 3]
                              ni",
    Ok (FuncApp (Combinator K, Literal (IntLit 42)))
]
  