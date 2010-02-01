IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'UserFetchByTraderType')
	BEGIN
		DROP  Procedure  UserFetchByTraderType
	END

GO

CREATE Procedure UserFetchByTraderType
(
	@TraderType int
)
AS

SELECT * FROM VW_UserFetch
WHERE TraderType = @TraderType
AND ListingCount >= 5
ORDER BY TradingName ASC

GO 