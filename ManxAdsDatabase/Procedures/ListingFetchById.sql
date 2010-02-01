IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'ListingFetchById')
	BEGIN
		DROP  Procedure  ListingFetchById
	END

GO

CREATE Procedure ListingFetchById
(
	@ListingId int
)
AS

SELECT * FROM VW_ListingFetch
WHERE ListingId = @ListingId

GO