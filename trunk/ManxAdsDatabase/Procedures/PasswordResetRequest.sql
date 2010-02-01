IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'PasswordResetRequest')
	BEGIN
		DROP  Procedure  PasswordResetRequest
	END

GO

CREATE Procedure PasswordResetRequest
(
	@AuthCode nvarchar(50),
	@Hostname nvarchar(100),
	@IPAddress nvarchar(15),
	@EmailAddress nvarchar(100)
)
AS

DECLARE @UserId int

SELECT @UserId = UserId FROM Users
WHERE EmailAddress = @EmailAddress

IF @UserId <> 0
BEGIN
	INSERT INTO PasswordResets
		(UserId, AuthCode, Hostname, IPAddress, RequestDate)
	VALUES
		(@UserId, @AuthCode, @Hostname, @IPAddress, GETDATE())
END

GO
