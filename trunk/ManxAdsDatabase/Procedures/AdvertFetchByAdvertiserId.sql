create procedure AdvertFetchByAdvertiserId
(
	@AdvertiserId int
)
as

SELECT * FROM VW_AdvertFetch
WHERE AdvertiserId = @AdvertiserId

go