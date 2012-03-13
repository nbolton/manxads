ALTER Procedure ListingFetchBaseNoBan AS

SELECT l.ListingId, l.Title, l.Details, l.DetailsType, l.CreateDate, l.UpdateDate, l.BoostDate, l.ExpiryNotified
FROM Listings as l
INNER JOIN Users AS u
	ON u.UserId = l.UserId
	AND (u.BanUntil <= GETDATE() OR u.BanUntil IS NULL)
WHERE l.Enabled = 1
AND l.Archive = 0

GO
