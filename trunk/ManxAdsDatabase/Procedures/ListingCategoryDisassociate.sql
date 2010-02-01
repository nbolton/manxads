IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'ListingCategoryDisassociate')
	BEGIN
		DROP  Procedure  ListingCategoryDisassociate
	END

GO

CREATE Procedure ListingCategoryDisassociate
(
	@ListingId int,
	@CategoryId int
)
AS

DELETE FROM ListingCategories
WHERE ListingId = @ListingId
AND CategoryId = @CategoryId

GO