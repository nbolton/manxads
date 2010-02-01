IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'UserFetchById')
	BEGIN
		DROP  Procedure  UserFetchById
	END

GO

CREATE Procedure UserFetchById
(
	@UserId int
)
AS

SELECT * FROM VW_UserFetch
WHERE UserId = @UserId

GO
