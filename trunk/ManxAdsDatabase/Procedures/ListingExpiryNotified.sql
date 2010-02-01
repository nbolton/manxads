CREATE PROCEDURE ListingExpiryNotified
(
	@listingID int
)
AS

UPDATE Listings SET ExpiryNotified = 1 WHERE ListingID = @listingID

GO