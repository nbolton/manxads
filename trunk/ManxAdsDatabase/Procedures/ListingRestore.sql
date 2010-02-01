ALTER PROCEDURE ListingRestore
(
	@listingID int
) 
AS

UPDATE Listings SET
	[Enabled] = 1,
	BoostDate = GETDATE()
WHERE ListingID = @listingID

GO
