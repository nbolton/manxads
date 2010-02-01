IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'UserVerify')
	BEGIN
		DROP  Procedure  UserVerify
	END

GO

CREATE Procedure UserVerify
(
    @AuthCode nvarchar(50),
    @UserId int OUTPUT
)
AS

DECLARE @VerificationId int

SELECT
	@VerificationId = VerificationId,
	@UserId = UserId
FROM Verifications
WHERE AuthCode = @AuthCode

IF NOT @VerificationId IS NULL
BEGIN
	UPDATE Verifications
	SET VerifyDate = GETDATE()
	WHERE VerificationId = @VerificationId
	
	UPDATE Users
	SET IsVerified = 1
	WHERE UserId = @UserId
END

GO