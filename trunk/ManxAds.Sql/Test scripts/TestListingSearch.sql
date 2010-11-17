DECLARE @wordsXml xml
DECLARE @anyKeywords bit
DECLARE @maxDate datetime
DECLARE @minDate datetime
DECLARE @maxPrice numeric(18,0)
DECLARE @minPrice numeric(18,0)
DECLARE @categoryId int
DECLARE @locationId int
DECLARE @sellerId int
DECLARE @resultsLimit int
DECLARE @startIndex int

set @wordsXml = '<Root><Word>or</Word><Word>arabian</Word><Word>or</Word><Word>x</Word><Word>or</Word><Word>quarter</Word><Word>or</Word><Word>horse</Word></Root>'
set @anyKeywords = 1
set @maxDate = '01/01/2050 00:00:00'
set @minDate = '01/01/2000 00:00:00'
set @maxPrice = 922337203685477.5807
set @minPrice = 0
set @categoryId = -1
set @locationId = -1
set @sellerId = -1
set @resultsLimit = -1
set @startIndex = -1

EXECUTE [ManxAdsWebsite].[dbo].[ListingSearch] 
   @wordsXml
  ,@anyKeywords
  ,@maxDate
  ,@minDate
  ,@maxPrice
  ,@minPrice
  ,@categoryId
  ,@locationId
  ,@sellerId
  ,@resultsLimit
  ,@startIndex
  