ALTER Procedure ListingFetchBase AS

SELECT ListingId, Title, Details, DetailsType, CreateDate, UpdateDate, BoostDate, ExpiryNotified
FROM Listings
WHERE Enabled = 1
AND Archive = 0

GO