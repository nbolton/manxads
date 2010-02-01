ALTER View VW_ListingFetch AS

SELECT
	l.UserId AS SellerId, l.ListingId, l.Title, l.Details, l.PriceValue,
	l.PriceType, l.CreateDate, l.UpdateDate, l.BoostDate,
	l.ShowLandline, l.ShowMobile, l.ShowLocation, l.LocationId,
	li.ListingImageId AS MasterImageId, l.Enabled, l.UserId, l.DetailsType,
	l.PageHits
FROM Listings AS l
LEFT JOIN ListingImages AS li
	ON li.ListingId = l.ListingId
	AND li.Master = 1
INNER JOIN Users AS u
	ON u.UserId = l.UserId
	AND (u.BanUntil <= GETDATE() OR u.BanUntil IS NULL)
WHERE l.Archive = 0

GO