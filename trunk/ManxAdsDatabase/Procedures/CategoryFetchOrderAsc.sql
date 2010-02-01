IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'CategoryFetchOrderAsc')
	BEGIN
		DROP  Procedure  CategoryFetchOrderAsc
	END

GO

CREATE Procedure CategoryFetchOrderAsc AS

SELECT * FROM VW_CategoryFetch
ORDER BY Title ASC