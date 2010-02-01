IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'ListingCount')
	BEGIN
		DROP  Procedure  ListingCount
	END

GO

CREATE Procedure ListingCount
(
	@FromDate datetime,
	@Enabled bit,
	@SellerId int,
	@Count int OUTPUT
)
AS

IF @SellerId != 0
BEGIN
	SELECT @Count = COUNT(ListingId)
	FROM Listings
	WHERE CreateDate >= @FromDate
	AND Enabled = @Enabled
	AND UserId = @SellerId
	AND Archive = 0
END
ELSE
BEGIN
	SELECT @Count = COUNT(ListingId)
	FROM Listings
	WHERE CreateDate >= @FromDate
	AND Enabled = @Enabled
	AND Archive = 0
END

GO