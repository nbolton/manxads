IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'ListingKeywordFetchByWord')
	BEGIN
		DROP  Procedure  ListingKeywordFetchByWord
	END

GO

CREATE Procedure ListingKeywordFetchByWord
(
	@Word nvarchar(50)
)
AS

SELECT lk.ListingId, lk.Weight, lk.Occurances
FROM Keywords AS k
INNER JOIN ListingKeywords AS lk
ON lk.KeywordId = k.KeywordId
WHERE k.Word = @Word

GO
