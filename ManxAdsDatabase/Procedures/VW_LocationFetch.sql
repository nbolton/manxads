IF EXISTS (SELECT * FROM sysobjects WHERE type = 'V' AND name = 'VW_LocationFetch')
	BEGIN
		DROP  View VW_LocationFetch
	END
GO

CREATE View VW_LocationFetch AS

SELECT LocationId, Location
FROM Locations

GO