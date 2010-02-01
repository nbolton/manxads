IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'AdvertFetchByPosition')
	BEGIN
		DROP  Procedure  AdvertFetchByPosition
	END

GO

CREATE Procedure AdvertFetchByPosition
(
	@PositionType int
)
AS

SELECT * FROM VW_AdvertFetch
WHERE PositionType = @PositionType
AND SiteWide = 1
AND Authorised = 1

GO 