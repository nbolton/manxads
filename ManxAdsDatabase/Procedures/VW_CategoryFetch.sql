IF EXISTS (SELECT * FROM sysobjects WHERE type = 'V' AND name = 'VW_CategoryFetch')
	BEGIN
		DROP  View VW_CategoryFetch
	END
GO

CREATE View VW_CategoryFetch AS

SELECT
	c.CategoryId, c.Title, c.Priority, c.HasImage, c.Description,
	l.LatestListing, l.ListingCount
FROM Categories AS c
LEFT JOIN (
	SELECT TOP 100 Percent
		innerc.CategoryId,
		MAX(innerl.CreateDate) AS LatestListing,
		COUNT(innerl.listingId) AS ListingCount
	FROM Listings AS innerl
	LEFT JOIN ListingCategories AS innerlc
		ON innerlc.ListingId = innerl.ListingId
	LEFT JOIN Categories AS innerc
		ON innerc.CategoryId = innerlc.CategoryId
	WHERE innerl.Enabled = 1
	GROUP BY innerc.CategoryId
) AS l
	ON l.CategoryId = c.CategoryId

GO