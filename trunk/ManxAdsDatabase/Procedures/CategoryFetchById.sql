IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'CategoryFetchById')
	BEGIN
		DROP  Procedure  CategoryFetchById
	END

GO

CREATE Procedure CategoryFetchById
(
	@CategoryId int
)
AS

SELECT * FROM VW_CategoryFetch
WHERE CategoryId = @CategoryId