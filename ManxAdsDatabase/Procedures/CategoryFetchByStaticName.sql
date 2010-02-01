IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'CategoryFetchByStaticName')
	BEGIN
		DROP  Procedure  CategoryFetchByStaticName
	END

GO

CREATE Procedure CategoryFetchByStaticName
(
	@StaticName nvarchar(50)
)
AS

SELECT * FROM VW_CategoryFetch
WHERE StaticName = @StaticName