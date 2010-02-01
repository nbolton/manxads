ALTER Procedure ListingRemove
(
	@ListingId int
)
AS

DELETE FROM Listings
WHERE ListingId = @ListingId

GO