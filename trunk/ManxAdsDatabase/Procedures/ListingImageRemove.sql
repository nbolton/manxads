IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'ListingImageRemove')
	BEGIN
		DROP  Procedure  ListingImageRemove
	END

GO

CREATE Procedure ListingImageRemove
(
	@ListingImageId int
)
AS

DELETE FROM ListingImages
WHERE ListingImageId = @ListingImageId

GO