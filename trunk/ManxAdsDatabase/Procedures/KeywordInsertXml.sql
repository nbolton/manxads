alter procedure KeywordInsertXml
(
	@KeywordsXml xml
)
as

insert into Keywords
select r.k.value('.', 'varchar(max)') as Keyword
from @KeywordsXml.nodes('//Keyword') r(k)
