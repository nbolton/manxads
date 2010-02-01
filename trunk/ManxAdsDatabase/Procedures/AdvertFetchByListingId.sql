IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'AdvertFetchByListingId')
	BEGIN
		DROP  Procedure  AdvertFetchByListingId
	END

GO

CREATE Procedure AdvertFetchByListingId
(
	@ListingId int,
	@PositionType int
)
AS

SELECT a.*, ca.RotateFrequency
FROM CategoryAdverts AS ca
LEFT JOIN VW_AdvertsFetch AS a
	ON a.AdvertId = ca.AdvertId
LEFT JOIN ListingCategories AS lc
	ON lc.CategoryId = ca.CategoryId
WHERE lc.Listingid = @ListingId
AND a.PositionType = @PositionType
AND a.Authorised = 1


GO

