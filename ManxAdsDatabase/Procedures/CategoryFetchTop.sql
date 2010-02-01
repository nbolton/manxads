IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'CategoryFetchTop')
	BEGIN
		DROP  Procedure  CategoryFetchTop
	END

GO

CREATE Procedure CategoryFetchTop
(
	@Limit int
)
AS

SELECT TOP (@Limit)
	c.CategoryId, c.Title, l.ListingCount
FROM Categories AS c
LEFT JOIN (
	SELECT TOP 100 Percent
		innerc.CategoryId,
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
ORDER BY c.Priority ASC