 IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'UserCountByTraderType')
	BEGIN
		DROP  Procedure  UserCountByTraderType
	END

GO

CREATE Procedure UserCountByTraderType
(
	@TraderType int,
	@FromDate datetime,
	@Count int OUTPUT
)
AS

SELECT @Count = COUNT(UserId)
FROM Users
WHERE CreateDate > @FromDate
AND TraderType = @TraderType

GO