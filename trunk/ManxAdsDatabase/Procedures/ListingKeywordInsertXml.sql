alter procedure ListingKeywordInsertXml
(
	@ListingId int,
	@ListingKeywordsXml xml
)
as

insert into ListingKeywords
select
	@ListingId,
	r.k.query('KeywordId').value('.', 'int') as KeywordId,
	r.k.query('Weight').value('.', 'float') as Weight
from @ListingKeywordsXml.nodes('//ListingKeyword') r(k)
