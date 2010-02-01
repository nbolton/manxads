IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'ListingFetchBySellerId')
	BEGIN
		DROP  Procedure  ListingFetchBySellerId
	END

GO

CREATE Procedure ListingFetchBySellerId
(
	@SellerId int,
	@Enabled bit
)
AS

SELECT * FROM VW_ListingFetch
WHERE UserId = @SellerId
AND Enabled = @Enabled
ORDER BY BoostDate DESC

GO