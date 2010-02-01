IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'ListingFetchLimit')
	BEGIN
		DROP  Procedure  ListingFetchLimit
	END

GO

CREATE Procedure ListingFetchLimit
(
	@Index int,
	@Length int
)
AS

SELECT * FROM (
	
	SELECT TOP (@Length) * FROM (
		
		SELECT TOP (@Length + @Index) *
		FROM VW_ListingFetch
		WHERE Enabled = 1
		ORDER BY BoostDate DESC
		
	) AS InnerOrder
	ORDER BY BoostDate ASC
	
) AS OuterOrder
ORDER BY BoostDate DESC

GO