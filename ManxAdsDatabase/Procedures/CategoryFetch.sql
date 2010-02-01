IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'CategoryFetch')
	BEGIN
		DROP  Procedure  CategoryFetch
	END

GO

CREATE Procedure CategoryFetch AS

SELECT * FROM VW_CategoryFetch
ORDER BY Priority ASC