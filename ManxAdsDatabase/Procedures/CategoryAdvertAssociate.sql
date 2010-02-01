IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'CategoryAdvertAssociate')
	BEGIN
		DROP  Procedure  CategoryAdvertAssociate
	END

GO

CREATE Procedure CategoryAdvertAssociate
(
	@CategoryId int,
	@AdvertId int,
	@RotateFrequency int,
	@InsertId int OUTPUT
)
AS

INSERT INTO CategoryAdverts
	(CategoryId, AdvertId, RotateFrequency)
VALUES
	(@CategoryId, @AdvertId, @RotateFrequency)
	
SET @InsertId = SCOPE_IDENTITY()

GO