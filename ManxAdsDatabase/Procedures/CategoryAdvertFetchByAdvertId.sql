IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'CategoryAdvertFetchByAdvertId')
	BEGIN
		DROP  Procedure  CategoryAdvertFetchByAdvertId
	END

GO

CREATE Procedure CategoryAdvertFetchByAdvertId
(
	@AdvertId int
)
AS

SELECT CategoryAdvertId, CategoryId, AdvertId, RotateFrequency
FROM CategoryAdverts WHERE @AdvertId = AdvertId

GO

