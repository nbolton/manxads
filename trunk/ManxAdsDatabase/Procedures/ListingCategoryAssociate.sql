IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'ListingCategoryAssociate')
	BEGIN
		DROP  Procedure  ListingCategoryAssociate
	END

GO

CREATE Procedure ListingCategoryAssociate
(
	@ListingId int,
	@CategoryId int
)
AS

INSERT INTO ListingCategories
	(ListingId, CategoryId)
VALUES
	(@ListingId, @CategoryId)

GO