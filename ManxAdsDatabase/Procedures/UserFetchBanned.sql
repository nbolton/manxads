create procedure UserFetchBanned
as

select * from VW_UserFetch
where BanUntil is not null

go
