IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'UserFetchPromoEmails')
	BEGIN
		DROP  Procedure  UserFetchPromoEmails
	END

GO

CREATE Procedure UserFetchPromoEmails AS

SELECT EmailAddress
FROM Users
WHERE EmailOptOut = 0
ORDER BY EmailAddress

GO 