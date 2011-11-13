alter procedure UserFetchBanned
as

select * from VW_UserFetch
where BanUntil is not null and BanUntil > getdate()

go
