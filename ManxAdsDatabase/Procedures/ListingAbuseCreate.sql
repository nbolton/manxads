ALTER PROCEDURE ListingAbuseCreate
(
	@ListingId int,
	@ReporterId int,
	@Reason nvarchar(MAX)
)
AS

INSERT INTO ListingAbuse
	(ListingId, ReporterId, CreateDate, Reason)
VALUES
	(@ListingId, @ReporterId, GETDATE(), @Reason)