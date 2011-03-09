alter procedure KeywordGetIds
(
	@KeywordsXml xml
)
as

select KeywordId, xk.Keyword as Word from
(
	select r.k.value('.', 'varchar(max)') as Keyword
	from @KeywordsXml.nodes('//Keyword') r(k)
) as xk
left join Keywords as k on k.Word = xk.Keyword
