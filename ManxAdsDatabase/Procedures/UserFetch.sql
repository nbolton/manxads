IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'UserFetch')
	BEGIN
		DROP  Procedure  UserFetch
	END

GO

CREATE Procedure UserFetch AS

SELECT * FROM VW_UserFetch
ORDER BY EmailAddress

GO