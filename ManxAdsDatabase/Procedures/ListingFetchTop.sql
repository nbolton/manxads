ALTER Procedure ListingFetchTop
(
	@Limit int,
	@OnlyImages bit
)
AS

SELECT TOP 10
	l.ListingId, l.Title, l.Details, l.PriceValue,
	l.PriceType, li.ListingImageId AS MasterImageId, l.Enabled
FROM Listings AS l
INNER JOIN ListingImages AS li
	ON li.ListingId = l.ListingId
	AND li.Master = 1
INNER JOIN Users AS u
	ON u.UserId = l.UserId
	AND (u.BanUntil <= GETDATE() OR u.BanUntil IS NULL)
WHERE l.Enabled = 1
ORDER BY BoostDate DESC

GO