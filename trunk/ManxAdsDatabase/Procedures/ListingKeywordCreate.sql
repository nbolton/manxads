IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'ListingKeywordCreate')
	BEGIN
		DROP  Procedure  ListingKeywordCreate
	END

GO

CREATE Procedure ListingKeywordCreate
(
	@Word nvarchar(50),
	@ListingId int,
	@Weight float
)
AS

DECLARE @KeywordId int

SELECT @KeywordId = KeywordId
FROM Keywords WHERE Word = @Word

IF @KeywordId IS NULL
BEGIN
	INSERT INTO Keywords (Word) VALUES (@Word)
	SET @KeywordId = SCOPE_IDENTITY()
END

INSERT INTO
ListingKeywords (KeywordId, ListingId, Weight)
VALUES (@KeywordId, @ListingId, @Weight)

GO