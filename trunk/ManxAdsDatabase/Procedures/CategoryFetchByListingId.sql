IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'CategoryFetchByListingId')
	BEGIN
		DROP  Procedure  CategoryFetchByListingId
	END

GO

CREATE Procedure CategoryFetchByListingId
(
	@ListingId int
)
AS

SELECT * FROM VW_CategoryFetch AS c
LEFT JOIN ListingCategories AS lc ON lc.CategoryId = c.CategoryId
WHERE lc.ListingId = @ListingId
ORDER BY Title ASC