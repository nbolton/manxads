IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'UserFetchByEmail')
	BEGIN
		DROP  Procedure  UserFetchByEmail
	END

GO

CREATE Procedure UserFetchByEmail
(
	@EmailAddress nvarchar(200)
)
AS

SELECT * FROM VW_UserFetch
WHERE EmailAddress = @EmailAddress

GO
