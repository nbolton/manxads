IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'LocationFetchById')
	BEGIN
		DROP  Procedure  LocationFetchById
	END

GO

CREATE Procedure LocationFetchById
(
	@LocationId int
)
AS

SELECT LocationId, Location
FROM Locations
WHERE LocationId = @LocationId

GO
