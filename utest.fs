
\
\  UNIT TEST FRAMWORK FOR GFORTH
\
\ use
\ :UNIT-TEST <name> .... [ASSERT-TRUE|ASSERT-FALSE] ;
\ to define a unit test. it shoule leave [TRUE|FALSE] before the assert is called
\
\ run all tests using RUN-TESTS
\ check individual test by index using CHECK-TEST




\ number of unit tests we prepare space for
200 CONSTANT UNIT-TEST-LIMIT

CREATE <UNIT-TEST-TOKENS> UNIT-TEST-LIMIT CELL * ALLOT

\ init counter of tests
0 <UNIT-TEST-TOKENS> !
: ADD-TEST ( ca -- )
  <UNIT-TEST-TOKENS> @ UNIT-TEST-LIMIT >
  IF
      s" UNIT TESTS HAVE RUN OUT OF SPACE" EXCEPTION THROW
  THEN
  <UNIT-TEST-TOKENS> DUP @ 1+ CELL * + !
  <UNIT-TEST-TOKENS> @ 1+ <UNIT-TEST-TOKENS> !
;

: TEST-XT ( n -- )
  1+ CELL * <UNIT-TEST-TOKENS> + @
;

: RUN-TEST ( n -- )
  TEST-XT
  EXECUTE
;

: TEST-NAME ( n -- )
  TEST-XT >NAME NAME>STRING
;

: CHECK-TEST ( n -- )
  DUP
  CR TEST-NAME TYPE
  RUN-TEST
  IF
        ." ........ passed"
  ELSE
          ." ........ *FAILED*"
  THEN
  CR
;

: RUN-TESTS
  CR
  TRUE
  <UNIT-TEST-TOKENS> @  0
  ?DO
        I . ." ...."
        I DUP TEST-NAME TYPE RUN-TEST
        DUP
        IF
            ." ........ passed"
        ELSE
            ." ........ *FAILED*"
	    THEN
  	    CR
        AND
  LOOP
  CR
  IF
    ." ALL TESTS PASSED"
  ELSE
   ." *****    SOME UNIT TESTS FAILED   ******"
  THEN
  CR
;


: ASSERT-TRUE ;
: ASSERT-FALSE 0= ;


: :UNIT-TEST : LATESTXT ADD-TEST ;




