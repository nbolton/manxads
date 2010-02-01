create procedure AdvertMonthHit
(
	@AdvertId int
)
as

declare @month int, @year int
set @month = month(getdate())
set @year = year(getdate())

-- is there a record for this month+year?
declare @AdvertMonthId int
select @AdvertMonthId = AdvertMonthId 
from AdvertMonths
where AdvertId = @AdvertId 
and [Year] = @year 
and [Month] = @month

if @AdvertMonthId is NULL
begin
	insert into AdvertMonths
		(AdvertId, [Month], [Year], Hits)
		values (@AdvertId, @month, @year, 1)
end
else
begin
	update AdvertMonths set
		Hits = Hits + 1
	where AdvertMonthId = @AdvertMonthId
end

go 