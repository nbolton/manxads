IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'ListingImageFetchByListingId')
	BEGIN
		DROP  Procedure  ListingImageFetchByListingId
	END

GO

CREATE Procedure ListingImageFetchByListingId
(
	@ListingId int
)
AS

SELECT ListingImageId, Master
FROM ListingImages
WHERE ListingId = @ListingId

GO