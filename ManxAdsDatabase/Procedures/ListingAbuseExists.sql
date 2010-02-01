CREATE PROCEDURE ListingAbuseExists
(
	@ListingId int,
	@ReporterId int
)
AS

SELECT COUNT(ListingAbuseId)
FROM ListingAbuse
WHERE ListingId = @ListingId
AND ReporterId = @ReporterId