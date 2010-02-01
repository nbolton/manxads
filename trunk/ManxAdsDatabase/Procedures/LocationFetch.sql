IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'LocationFetch')
	BEGIN
		DROP  Procedure  LocationFetch
	END

GO

CREATE Procedure LocationFetch AS

SELECT * FROM VW_LocationFetch
ORDER BY Location

GO
