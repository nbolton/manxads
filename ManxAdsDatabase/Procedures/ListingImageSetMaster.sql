 IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'ListingImageSetMaster')
	BEGIN
		DROP  Procedure  ListingImageSetMaster
	END

GO

CREATE Procedure ListingImageSetMaster
(
	@ListingImageId int,
	@ListingId int
)
AS

UPDATE ListingImages SET Master = 0
WHERE ListingId = @ListingId

UPDATE ListingImages SET Master = 1
WHERE ListingImageId = @ListingImageId

GO