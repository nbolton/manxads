IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'ListingFetchTop')
	BEGIN
		DROP  Procedure  ListingFetchTop
	END

GO

CREATE Procedure ListingFetchTop
(
	@Limit int,
	@OnlyImages bit
)
AS

IF @OnlyImages = 1
BEGIN
	SELECT TOP (@Limit) * FROM VW_ListingFetch
	WHERE Enabled = 1
	AND MasterImageId != 0
	ORDER BY BoostDate DESC
END
ELSE
BEGIN
	SELECT TOP (@Limit) * FROM VW_ListingFetch
	WHERE Enabled = 1
	ORDER BY BoostDate DESC
END

GO