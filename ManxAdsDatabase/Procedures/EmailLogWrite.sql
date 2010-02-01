ALTER PROCEDURE EmailLogWrite
(
	@toEmail nvarchar(MAX),
	@fromEmail nvarchar(MAX),
	@senderEmail nvarchar(MAX),
	@subject nvarchar(MAX),
	@body nvarchar(MAX)
)
AS

INSERT INTO EmailLog
	(WriteDate, ToEmail, FromEmail, SenderEmail, Subject, Body)
VALUES
	(GETDATE(), @toEmail, @fromEmail, @senderEmail, @subject, @body)

GO