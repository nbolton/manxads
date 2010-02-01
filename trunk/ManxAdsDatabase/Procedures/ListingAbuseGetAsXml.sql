ALTER PROCEDURE ListingAbuseGetAsXml
AS

SELECT
	l.ListingId,
	l.Title AS ListingTitle,
	l.UserId AS SellerId,
	(
		SELECT COUNT(ListingAbuseId) 
		FROM ListingAbuse 
		WHERE ListingId = l.ListingId
	) AS TotalReportCount,
	(
		SELECT
			la.ListingAbuseId,
			la.ListingId,
			la.ReporterId,
			ru.Forename + ' ' + ru.Surname AS ReporterName,
			la.Reason
		FROM ListingAbuse AS la
		INNER JOIN Listings AS l2 ON l2.ListingId = la.ListingId
		INNER JOIN Users AS ru ON ru.UserId = la.ReporterId
		WHERE l2.ListingId = l.ListingId
		AND la.AdminNotified = 0
		FOR XML PATH('Report'), TYPE
	) AS Reports,
	(
		SELECT c.Title
		FROM Categories AS c
		INNER JOIN ListingCategories AS lc ON lc.CategoryId = c.CategoryId
		WHERE lc.ListingId = l.ListingId
		FOR XML PATH('Category'), TYPE
	) AS Categories
FROM Listings AS l
INNER JOIN
(
	SELECT ListingId 
	FROM ListingAbuse
	WHERE AdminNotified = 0 
	GROUP BY ListingId
) AS l_ids ON l_ids.ListingId = l.ListingId
ORDER BY TotalReportCount DESC

FOR XML PATH('ReportGroup'), ROOT('ReportGroups')