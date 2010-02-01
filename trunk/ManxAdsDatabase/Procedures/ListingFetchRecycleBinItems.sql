ALTER PROCEDURE ListingFetchRecycleBinItems
(
	@updateBeforeDateTime datetime
)
AS

SELECT * FROM VW_ListingFetch 
WHERE Enabled = 0
AND UpdateDate < @updateBeforeDateTime