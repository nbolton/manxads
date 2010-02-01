IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'ListingImageCreate')
	BEGIN
		DROP  Procedure  ListingImageCreate
	END

GO

CREATE Procedure ListingImageCreate
(
	@ListingId int,
	@Master bit,
	@InsertId int OUTPUT
)
AS

If @Master = 1
BEGIN
	UPDATE ListingImages SET Master = 0
	WHERE ListingId = @ListingId
END

INSERT INTO ListingImages (ListingId, Master)
VALUES (@ListingId, @Master)

SET @InsertId = SCOPE_IDENTITY()

GO