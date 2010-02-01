IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'PasswordResetComplete')
	BEGIN
		DROP  Procedure  PasswordResetComplete
	END

GO

CREATE Procedure PasswordResetComplete
(
	@AuthCode nvarchar(50),
	@UserId int OUTPUT
)
AS

DECLARE @CompleteDate datetime

SELECT
	@UserId = UserId,
	@CompleteDate = CompleteDate
FROM PasswordResets
WHERE AuthCode = @AuthCode

IF NOT @CompleteDate IS NULL
	SET @UserId = NULL

IF NOT @UserId IS NULL
BEGIN
	UPDATE PasswordResets SET
		CompleteDate = GETDATE()
	WHERE AuthCode = @AuthCode
END

GO
