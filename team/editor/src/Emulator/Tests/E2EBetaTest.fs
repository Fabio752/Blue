module E2EBetaTest

open SharedTypes

let testCasesBetaE2E : (string * string * Result<Ast, SharedTypes.ErrorT>) list = [
    "Small program", "\x.\y. y x",
    Ok
      (LambdaExp
         { LambdaParam = "x"
           LambdaBody =
                       LambdaExp
                         { LambdaParam = "y"
                           LambdaBody = FuncApp (Identifier "y", Identifier "x") } });
    "simple addition", "let x = 10 in 
           x + x 
         ni",
    Ok (Literal (IntLit 20));
    "Curried Lambda", "\xy.42",
    Ok (LambdaExp { LambdaParam = "xy"
                    LambdaBody = Literal (IntLit 42) });
    "ListMap add ", "let listMap f lst = 
                      if size lst == 0
                      then []
                      else append (f (head lst))
                           (listMap f (tail lst))
                      fi
                    in 
                    listMap (\x.x+1)[1, 2, 3]
                    ni",
    Ok (SeqExp
            (Literal (IntLit 2),
            SeqExp (Literal (IntLit 3),SeqExp (Literal (IntLit 4),SeqExp (Null,Null)))));
    "ListMap mult ", "let listMap f lst = 
                        if size lst == 0
                        then []
                        else append (f (head lst))
                             (listMap f (tail lst))
                        fi
                      in 
                      listMap (\x.x*2)[3, 5, 32]
                      ni",
    Ok (SeqExp
            (Literal (IntLit 6),
            SeqExp (Literal (IntLit 10),SeqExp (Literal (IntLit 64),SeqExp (Null,Null)))));
    "ListMap div ", "let listMap f lst = 
                        if size lst == 0
                        then []
                        else append (f (head lst))
                             (listMap f (tail lst))
                        fi
                      in 
                      listMap (\x.x/2)[10, 2, 84]
                      ni",
    Ok (SeqExp
            (Literal (IntLit 5),
            SeqExp (Literal (IntLit 1),SeqExp (Literal (IntLit 42),SeqExp (Null,Null)))))
    "ListMap sub ", "let listMap f lst = 
                        if size lst == 0
                        then []
                        else append (f (head lst))
                             (listMap f (tail lst))
                        fi
                      in 
                      listMap (\x.x-2)[2, 1, 11]
                      ni",
    Ok (SeqExp
            (Literal (IntLit 0),
            SeqExp (Literal (IntLit -1),SeqExp (Literal (IntLit 9),SeqExp (Null,Null)))));
    "ListReduce add ", "let listReduce f lst =
                          let reducer f acc lst =
                              if size lst == 0
                              then acc
                              else reducer f (f acc (head lst)) (tail lst)
                              fi
                          in
                              if size lst == 0
                              then 0 
                              else reducer f (head lst) (tail lst) 
                              fi
                          ni
                        in
                          let sum a b = 
                              a + b
                          in 
                              listReduce sum [120, 2, 3, 4]
                          ni
                        ni",
    Ok  (Literal (IntLit 129));
    "ListReduce sub ", "let listReduce f lst =
                          let reducer f acc lst =
                              if size lst == 0
                              then acc
                              else reducer f (f acc (head lst)) (tail lst)
                              fi
                          in
                              if size lst == 0
                              then 0 
                              else reducer f (head lst) (tail lst) 
                              fi
                          ni
                        in
                          let sub a b = 
                              a - b
                          in 
                              listReduce sub [120, 2, 3, 4]
                          ni
                        ni",
    Ok  (Literal (IntLit 111));
    "ListReduce mul ", "let listReduce f lst =
                          let reducer f acc lst =
                              if size lst == 0
                              then acc
                              else reducer f (f acc (head lst)) (tail lst)
                              fi
                          in
                              if size lst == 0
                              then 0 
                              else reducer f (head lst) (tail lst) 
                              fi
                          ni
                        in
                          let mul a b = 
                              a * b
                          in 
                              listReduce mul [120, 2, 3, 4]
                          ni
                        ni",
    Ok  (Literal (IntLit 2880));
    "ListReduce div ", "let listReduce f lst =
                          let reducer f acc lst =
                              if size lst == 0
                              then acc
                              else reducer f (f acc (head lst)) (tail lst)
                              fi
                          in
                              if size lst == 0
                              then 0 
                              else reducer f (head lst) (tail lst) 
                              fi
                          ni
                        in
                          let div a b = 
                              a / b
                          in 
                              listReduce div [120, 2, 3, 4]
                          ni
                        ni",
    Ok  (Literal (IntLit 5));
    "ListFold add ", "let listFold f acc lst = 
                        if size lst == 0
                        then acc
                        else listFold f (f acc (head lst)) (tail lst)
                        fi
                      in
                          listFold(\x y.x+y) 0 [10, 2, 12]
                      ni",
    Ok  (Literal (IntLit 24));
    "ListFold sub ", "let listFold f acc lst = 
                        if size lst == 0
                        then acc
                        else listFold f (f acc (head lst)) (tail lst)
                        fi
                      in
                          listFold(\x y.x-y) 36 [10, 2, 12]
                      ni",
    Ok  (Literal (IntLit 12));
    "ListFold mul ", "let listFold f acc lst = 
                        if size lst == 0
                        then acc
                        else listFold f (f acc (head lst)) (tail lst)
                        fi
                      in
                          listFold(\x y.x*y) 1 [5, 2, 14]
                      ni",
    Ok  (Literal (IntLit 140));
    "ListFold div ", "let listFold f acc lst = 
                        if size lst == 0
                        then acc
                        else listFold f (f acc (head lst)) (tail lst)
                        fi
                      in
                          listFold(\x y.x/y) 120 [4, 3, 2]
                      ni",
    Ok  (Literal (IntLit 5));
    "ListSplitAt idx ", "let listSplitAt idx lst = 
                          let splitter lhs rhs idx =
                              if size lhs == idx || size rhs == 0
                              then [lhs, rhs]
                              else splitter (append (head rhs) lhs) (tail rhs) idx
                              fi
                          in 
                          splitter [] lst idx
                          ni
                        in
                        listSplitAt 1 [1,2,3]
                        ni",
    Ok (SeqExp
       (SeqExp (Literal (IntLit 1),SeqExp (Null,Null)),
        SeqExp
          (SeqExp
            (Literal (IntLit 2),SeqExp (Literal (IntLit 3),SeqExp (Null,Null))),
          SeqExp (Null,Null))));
    "ListSplitAt idx2", "let listReverse lst = 
                           let reverser lst revlst  = 
                             if size lst == 0
                             then revlst
                             else reverser (tail lst) (append (head lst) revlst)
                             fi
                           in
                             reverser lst []
                           ni
                         in
                           let listSplitAt idx lst = 
                              let splitter lhs rhs idx =
                                if size lhs == idx || size rhs == 0
                                then [lhs, rhs]
                                else splitter (append (head rhs) lhs) (tail rhs) idx
                                fi
                              in 
                                let halfReversed = splitter [] lst idx in
                                  (\\ a b. [a , b] ) (listReverse (head halfReversed)) (head (tail halfReversed))
                                ni
                              ni
                            in
                              listSplitAt (-2) [1,2,3,-2,14,11,47]
                            ni
                          ni",
    Ok (SeqExp
         (SeqExp
            (Literal (IntLit 1),
             SeqExp
               (Literal (IntLit 2),
                SeqExp
                  (Literal (IntLit 3),
                   SeqExp
                     (Literal (IntLit -2),
                      SeqExp
                        (Literal (IntLit 14),
                         SeqExp
                           (Literal (IntLit 11),
                            SeqExp (Literal (IntLit 47),SeqExp (Null,Null)))))))),
          SeqExp (SeqExp (Null,Null),SeqExp (Null,Null))))
    "ListFind Int true", "let equalTo int1 int2 =
                            int1==int2
                          in
                            let listFind f int lst = 
                                if size lst == 0
                                then false
                                else 
                                  if f (head lst) int
                                  then true 
                                  else listFind f int (tail lst)
                                  fi
                                fi
                            in
                              listFind equalTo 2 [1, 2, 3, 4]
                            ni
                          ni",
    Ok (Literal (BoolLit true));
    "ListFind Int false", "let equalTo int1 int2 =
                             int1==int2
                           in
                             let listFind f int lst = 
                                 if size lst == 0
                                 then false
                                 else 
                                   if f (head lst) int
                                   then true 
                                   else listFind f int (tail lst)
                                   fi
                                 fi
                             in
                               listFind equalTo 5 [1, 2, 3, 4]
                             ni
                           ni",
    Ok (Literal (BoolLit false));
    "ListFind Str true", "let listFind f int lst = 
                            if size lst == 0
                            then false
                            else 
                              if f (head lst) int
                              then true 
                              else listFind f int (tail lst)
                              fi
                            fi
                          in
                            listFind strEq \"Fabio\" [\"Fabio\", \"Marco\", \"Oliver\", \"Szymon\"]
                          ni",
    Ok (Literal (BoolLit true));
    "ListFind Str false", "let listFind f int lst = 
                             if size lst == 0
                             then false
                             else 
                               if f (head lst) int
                               then true 
                               else listFind f int (tail lst)
                               fi
                             fi
                           in
                             listFind strEq \"John\" [\"Fabio\", \"Marco\", \"Oliver\", \"Szymon\"]
                           ni",
    Ok (Literal (BoolLit false));
    "StringLength1", "let StringLength str = 
                        size (explode str)
                      in
                        StringLength \"1sdsadsvfe\"
                      ni",
    Ok (Literal (IntLit 10));
    "ListConcat ", "let listConcat lhs rhs = 
                      if size lhs == 0
                      then rhs
                      else append (head lhs) (listConcat (tail lhs) rhs)
                      fi
                    in
                      listConcat [\"a\"] [\"b\", \"c\", \"d\"]
                    ni",
    Ok
      (SeqExp
         (Literal (StringLit "a"),
          SeqExp
            (Literal (StringLit "b"),
             SeqExp
               (Literal (StringLit "c"),
                SeqExp (Literal (StringLit "d"),SeqExp (Null,Null))))));
    "StringConcat ", "let listConcat lhs rhs = 
                        if size lhs == 0
                        then rhs
                        else append (head lhs) (listConcat (tail lhs) rhs)
                        fi
                      in
                        let stringConcat lhs rhs = 
                            if size (explode lhs) == 0
                            then rhs
                            else implode (listConcat (explode lhs) (explode rhs))
                            fi
                        in 
                            stringConcat \"abc\" \"def\"
                        ni
                      ni",
    Ok (Literal (StringLit "abcdef"));
    "ListReverse ", "let listReverse lst = 
                        let reverser lst revlst  = 
                          if size lst == 0
                          then revlst
                          else reverser (tail lst) (append (head lst) revlst)
                          fi
                        in
                          reverser lst []
                        ni
                      in
                        listReverse [\"a\", \"bcd\", \"e\"]
                      ni",
    Ok
      (SeqExp
         (Literal (StringLit "e"),
          SeqExp
            (Literal (StringLit "bcd"),
                SeqExp (Literal (StringLit "a"),SeqExp (Null,Null)))));
    "ListItem inbound", "let listItem idx lst =    
                            let looper step idx lst =  
                              if size lst == 0
                              then 0 # undefined behaviour
                              else
                                if step == idx
                                then head lst
                                else looper (step + 1) idx (tail lst)
                                fi
                              fi
                            in
                              looper 0 idx lst
                            ni
                          in
                            listItem 1 [\"a\", \"b\", \"c\", \"d\"]
                          ni",
    Ok  (Literal (StringLit "b"));
    "ListItem out of bound", "let listItem idx lst =    
                                let looper step idx lst =  
                                  if size lst == 0
                                  then 0 # undefined behaviour
                                  else
                                    if step == idx
                                    then head lst
                                    else looper (step + 1) idx (tail lst)
                                    fi
                                  fi
                                in
                                  looper 0 idx lst
                                ni
                              in
                                listItem 7 [\"a\", \"b\", \"c\", \"d\"]
                              ni",
    Ok  (Literal (IntLit 0));
    "Simplified lexer",
    "let listReverse lst = 
        let reverser lst revlst  = 
          if size lst == 0 
          then revlst
          else reverser (tail lst) (append (head lst) revlst)
          fi
        in
          reverser lst []
        ni
      in

      let listConcat lhs rhs = 
              if size lhs == 0
              then rhs
              else append (head lhs) (listConcat (tail lhs) rhs)
              fi
            in

      let listSplitAt idx lst = 
              let splitter lhs rhs idx =
                if size lhs == idx || size rhs == 0
                then [lhs, rhs]
                else splitter (append (head rhs) lhs) (tail rhs) idx
                fi
              in 
              let halfReversed = splitter [] lst idx in
                (\ a b. [a , b] ) (listReverse (head halfReversed)) (head (tail halfReversed))
              ni ni
            in


    let stringAppend lhs rhs =
        implode (listConcat (explode lhs) (explode rhs))
    in

    let matchId chars input =
        if size input == 0 
        then chars
        else 
            if ! ( strEq (head input) \" \")
            then matchId (chars+1) (tail input)
            else chars
            fi
        fi
    in 

    let lexer program lst = 
        let headIs lst c =
            strEq (head lst) c
        in
        let lexNgram input lst = 
            if size input == 0
            then lst 
            else
            if headIs input \" \" 
            then lexNgram (tail input) lst
            else
                let chars = matchId 0 input in
                let token = implode (head (listSplitAt chars input)) in
                let rest = head (tail (listSplitAt chars input)) in
                    append (stringAppend \"Id \" token) (lexNgram rest lst)
                ni ni ni
             fi fi 
        in
            lexNgram (explode program) lst
        ni ni
    in
        lexer \"a b\" []
    ni ni ni
    ni ni ni",
    Ok (SeqExp (Literal (StringLit "Id a"), SeqExp (Literal (StringLit "Id b"),SeqExp (Null,Null))));
]
  
