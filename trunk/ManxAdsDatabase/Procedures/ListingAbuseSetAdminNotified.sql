ALTER PROCEDURE ListingAbuseSetAdminNotified
(
	@ListingId int
)
AS

UPDATE ListingAbuse SET AdminNotified = 1 WHERE ListingId = @ListingId 