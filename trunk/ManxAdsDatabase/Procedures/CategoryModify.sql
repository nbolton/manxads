IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'CategoryModify')
	BEGIN
		DROP  Procedure  CategoryModify
	END

GO

CREATE Procedure CategoryModify
(
	@CategoryId int,
	@Title nvarchar(50),
	@HasImage bit,
	@Priority int,
	@Description nvarchar(120)
)
AS

UPDATE Categories SET
	Title = @Title,
	Priority = @Priority,
	HasImage = @HasImage,
	Description = @Description
WHERE CategoryId = @CategoryId