ALTER Procedure ListingFetchByCategoryId
(
	@CategoryId int,
	@SortMode nvarchar(10)
)
AS

IF @SortMode = 'Boosted'
BEGIN
	SELECT * FROM VW_ListingFetch AS l
	LEFT JOIN ListingCategories AS lc
		ON lc.ListingId = l.ListingId
	WHERE lc.CategoryId = @CategoryId
	AND l.Enabled = 1
	ORDER BY l.BoostDate DESC
END
ELSE IF @SortMode = 'Listed'
BEGIN
	SELECT * FROM VW_ListingFetch AS l
	LEFT JOIN ListingCategories AS lc
		ON lc.ListingId = l.ListingId
	WHERE lc.CategoryId = @CategoryId
	AND l.Enabled = 1
	ORDER BY l.CreateDate DESC
END
ELSE IF @SortMode = 'PriceAsc'
BEGIN
	SELECT * FROM VW_ListingFetch AS l
	LEFT JOIN ListingCategories AS lc
		ON lc.ListingId = l.ListingId
	WHERE lc.CategoryId = @CategoryId
	AND l.Enabled = 1
	ORDER BY l.PriceValue ASC, l.PriceType DESC
END
ELSE IF @SortMode = 'PriceDesc'
BEGIN
	SELECT * FROM VW_ListingFetch AS l
	LEFT JOIN ListingCategories AS lc
		ON lc.ListingId = l.ListingId
	WHERE lc.CategoryId = @CategoryId
	AND l.Enabled = 1
	ORDER BY l.PriceValue DESC, l.PriceType DESC
END

GO