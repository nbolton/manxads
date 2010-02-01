IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'LocationFetchByTitle')
	BEGIN
		DROP  Procedure  LocationFetchByTitle
	END

GO

CREATE Procedure LocationFetchByTitle
(
	@Title nvarchar(50)
)
AS

SELECT * FROM VW_LocationFetch
WHERE Location = @Title

GO