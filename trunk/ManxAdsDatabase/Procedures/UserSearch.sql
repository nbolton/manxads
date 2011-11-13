ALTER PROCEDURE UserSearch
(
	@term nvarchar(MAX)
)
AS

SELECT * FROM VW_UserFetch 
WHERE Forename LIKE '%' + @term + '%'
OR Surname LIKE '%' + @term + '%'
OR (Forename + ' ' + Surname) LIKE '%' + @term + '%'
OR EmailAddress LIKE '%' + @term + '%'

GO