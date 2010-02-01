alter procedure Authenticate
(
	@EmailAddress nvarchar(100),
	@Password nvarchar(50),
	@IpAddress nvarchar(45),
	@UserId int OUTPUT,
	@UserType int OUTPUT
)
AS

SELECT
	@UserId = UserId,
	@UserType = UserType
FROM Users
WHERE EmailAddress = @EmailAddress
AND Password = @Password

IF NOT @UserId IS NULL
BEGIN
	UPDATE Users SET 
		LastActive = GETDATE(),
		LastIp = @IpAddress
	WHERE UserId = @UserId
END