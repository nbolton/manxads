IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'CategoryAdvertDisassociate')
	BEGIN
		DROP  Procedure  CategoryAdvertDisassociate
	END

GO

CREATE Procedure CategoryAdvertDisassociate
(
	@CategoryAdvertId int
)
AS

DELETE FROM CategoryAdverts
WHERE CategoryAdvertId = @CategoryAdvertId

GO