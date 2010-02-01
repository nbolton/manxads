IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'CategoryCreate')
	BEGIN
		DROP  Procedure  CategoryCreate
	END

GO

CREATE Procedure CategoryCreate
(
	@Title nvarchar(50),
	@Priority int,
	@HasImage bit,
	@Description nvarchar(120),
	@InsertId int OUTPUT
)
AS

INSERT INTO
Categories (Title, Priority, HasImage, Description)
VALUES (@Title, @Priority, @HasImage, @Description)

SET @InsertId = SCOPE_IDENTITY()