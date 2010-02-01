IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'UserRemove')
	BEGIN
		DROP  Procedure  UserRemove
	END

GO

CREATE Procedure UserRemove
(
	@UserId int
)
AS

DELETE FROM Users
WHERE UserId = @UserId

GO