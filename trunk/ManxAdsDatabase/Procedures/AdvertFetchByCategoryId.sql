IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'AdvertFetchByCategoryId')
	BEGIN
		DROP  Procedure  AdvertFetchByCategoryId
	END

GO

CREATE Procedure AdvertFetchByCategoryId
(
	@CategoryId int,
	@PositionType int
)
AS

SELECT a.*, ca.RotateFrequency
FROM CategoryAdverts AS ca
LEFT JOIN VW_AdvertFetch AS a
	ON a.AdvertId = ca.AdvertId
WHERE ca.CategoryId = @CategoryId
AND a.PositionType = @PositionType
AND a.Authorised = 1


GO

