IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'AdvertFetchById')
	BEGIN
		DROP  Procedure  AdvertFetchById
	END

GO

CREATE Procedure AdvertFetchById
(
	@AdvertId int
)
AS

SELECT * FROM VW_AdvertFetch
WHERE AdvertId = @AdvertId

GO