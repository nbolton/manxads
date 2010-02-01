IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'UserCheckEmailAddress')
	BEGIN
		DROP  Procedure  UserCheckEmailAddress
	END

GO

CREATE Procedure UserCheckEmailAddress
(
	@EmailAddress nvarchar(100),
	@EmailInUse bit OUTPUT
)
AS

DECLARE @UsageCount int

SELECT @UsageCount = COUNT(*)
FROM Users WHERE EmailAddress = @EmailAddress

IF @UsageCount <> 0 SET @EmailInUse = 1

GO
