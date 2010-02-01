IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'UserCountByUserType')
	BEGIN
		DROP  Procedure  UserCountByUserType
	END

GO

CREATE Procedure UserCountByUserType
(
	@UserType int,
	@FromDate datetime,
	@Count int OUTPUT
)
AS

SELECT @Count = COUNT(UserId)
FROM Users
WHERE CreateDate > @FromDate
AND UserType = @UserType

GO