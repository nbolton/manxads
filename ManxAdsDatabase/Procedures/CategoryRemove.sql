IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'CategoryRemove')
	BEGIN
		DROP  Procedure  CategoryRemove
	END

GO

CREATE Procedure CategoryRemove
(
	@CategoryId int
)
AS

DELETE FROM Categories WHERE CategoryId = @CategoryId