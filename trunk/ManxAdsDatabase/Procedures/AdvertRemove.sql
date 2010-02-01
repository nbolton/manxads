IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'AdvertRemove')
	BEGIN
		DROP  Procedure  AdvertRemove
	END

GO

CREATE Procedure AdvertRemove
(
	@AdvertId int
)
AS

DELETE FROM Adverts WHERE AdvertId = @AdvertId

GO