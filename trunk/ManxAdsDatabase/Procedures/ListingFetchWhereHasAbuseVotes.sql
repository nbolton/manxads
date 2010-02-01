ALTER PROCEDURE ListingFetchWhereHasAbuseVotes
(
	@AdminNotified bit
)
AS

SELECT * FROM
( 
	SELECT
		l.*,
		(
			SELECT COUNT(ListingAbuseId)
			FROM ListingAbuse AS la
			WHERE la.ListingId = l.ListingId 
			AND la.AdminNotified = @AdminNotified
		) AS NotifyMatchCount
	FROM VW_ListingFetch AS l
) AS l_outer
WHERE
	l_outer.NotifyMatchCount != 0