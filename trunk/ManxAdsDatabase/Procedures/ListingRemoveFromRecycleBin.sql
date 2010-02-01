CREATE PROCEDURE ListingRemoveFromRecycleBin
(
	@listingId int
)
AS

UPDATE Listings
SET Archive = 1
WHERE ListingId = @listingId