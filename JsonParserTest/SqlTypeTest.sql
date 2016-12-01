use myDB

declare @myJson json.SqlJson

SET @myJson=N'{"a":1, "b":[1,2,3]}'

select @myJson.Query(N'$.b').ToString()

select 
	json.ScalarArray_AGG(val).ToString()
from
(
	VALUES
	(N'1'),
	(N'b'),
	(N'c'),
	(N'2e-1'),
	(N'e')
) as ds (val)

select 
	json.ScalarObject_AGG(k,val,2).ToString()
from
(
	VALUES
	(N'a',N'1'),
	(N'b',N'b'),
	(N'c',N'c'),
	(N'c',N'2e-1'),
	(N'c',N'e')
) as ds (k,val)







