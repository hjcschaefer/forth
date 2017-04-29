
: JDN ( yyyy mm dd -- jdn )
  >R                            ( Y M    ::  D )
  DUP 3 < IF -1 ELSE 0 THEN DUP >R
  SWAP 2 -                      ( Y P M-2 :: P D )
  SWAP 12 * -             ( Y R     :: P D )
  OVER                ( Y R Y :: P D )
  4800 R@ + +                   ( Y R Q   :: P D )
  ROT 4900 R> + + 100 /         ( R Q S   :: D )
  3 * 4 / -1 * ROT 367 * 12 / ROT 1461 * 4 / ( S' R' Q' :: D)
  R> 32075 - + + +
;



: INV-JDN ( jdn -- yyyy mm dd )
  68569 +   (  L  ::  )
  DUP 4 * 146097 / DUP >R  (  L   N  :: N )
  146097 * 3 + 4 / - ( L :: N )
  DUP 1 + 4000 * 1461001 / DUP >R ( L I :: I N )
  1461 * 4 / - 31 +  ( L :: I N )
  DUP 80 * 2447 / DUP >R ( L J :: J I N )
  2447 * 80 / - ( D1 :: J I N )
  R@ 11 / ( D1 L :: J I N )
  R> 2 + OVER 12 * - ( D1 L M1 ;; I N )
  SWAP  ( D1 M1 L :: I N )
  R> + R> 49 - 100 * + ( D1 M1 Y1 :: )
  SWAP ( D1 Y1 M1 :: )
  ROT ( Y1 M1 D1 :: )
;

: SUBSTR ( addr offset count -- add count )
  CHARS SWAP ROT + SWAP
;

: STR>INT ( just need a single number )
  S>NUMBER? DROP  \ should check flag
  DROP
;

: ISO8601-YEAR ( addr -- d) \ address of iso8601 string
  4 STR>INT
;

: ISO8601-MONTH ( addr -- d)
  5 2 SUBSTR STR>INT
;

: ISO8601-DAY ( addr -- d)
  8 2 SUBSTR STR>INT
;

\ parse iso 8601 date and return jdn
: PARSE-DATE ( addr count -- jdn )
  ASSERT( 10 = )  \ check length of string is 10 (according to ISO norm)

  DUP ISO8601-DAY >R
  DUP ISO8601-MONTH >R
  ISO8601-YEAR
  R> R>
  JDN
;

: DATE CREATE PARSE-DATE , DOES> @ ;

: .DATE-ISO8601 \ need to remove spaces and so on
  INV-JDN
  ROT . 45 EMIT
  SWAP . 45 EMIT
  .
;
 
\ ------------- UNIT TESTS -------------------

:unit-test t-jdn-1 2017 4 29 jdn 2457873 = assert-true ;
:unit-test t-jdn-2a 1970 2 7 jdn inv-jdn drop drop 1970 = assert-true ;
:unit-test t-jdn-2b 1970 2 7 jdn inv-jdn rot drop drop 2 = assert-true ;
:unit-test t-jdn-2c 1970 2 7 jdn inv-jdn rot drop swap drop 7 = assert-true ;
:unit-test t-substr-1a s" 1234567"  drop 3 2 substr swap drop 2 = assert-true ;
:unit-test t-substr-1b s" 1234567"  drop 3 2 substr drop c@ '4' = assert-true ;
:unit-test t-iso8601-year s" 2012-09-13" drop iso8601-year 2012 = assert-true ;
:unit-test t-iso8601-month s" 2012-09-13" drop iso8601-month 9 = assert-true ;
:unit-test t-iso8601-day s" 2012-09-13" drop iso8601-day 13 = assert-true ;


