alter View VW_AdvertFetch AS

select
	a.AdvertId,
	a.UserId AS AdvertiserId,
	a.SizeType,
	a.FormatType,
	a.PositionType,
	a.Title,
	a.SiteWide,
	a.RotateFrequency,
	a.Hyperlink,
	a.Authorised,
	a.Html,
	(
		select Hits from AdvertMonths
		where AdvertId = a.AdvertId
		and [Month] = month(getdate())
		and [Year] = year(getdate())
	) as HitsMonth,
	(
		select sum(Hits) from AdvertMonths
		where AdvertId = a.AdvertId
	) as HitsTotal
from Adverts as a

GO