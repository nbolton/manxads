IF EXISTS (SELECT * FROM sysobjects WHERE type = 'V' AND name = 'VW_UserFetch')
	BEGIN
		DROP  View VW_UserFetch
	END
GO

CREATE View VW_UserFetch AS

SELECT
	u.UserId,
	u.SocialTitleType,
	u.Forename,
	u.Surname,
	u.EmailAddress,
	u.Password,
	u.LandlineArea,
	u.LandlinePhone,
	u.MobileArea,
	u.MobilePhone,
	u.LocationId,
	u.UserType,
	u.EmailOptOut,
	u.LastActive,
	u.CreateDate,
	u.UpdateDate,
	u.RegisterType,
	u.IsVerified,
	u.TradingName,
	u.TradingWebsite,
	u.TraderType,
	u.BanUntil,
	u.LastIp,
	v.VerifyDate,
	v.AuthCode AS VerifyAuthCode,
	(
		SELECT COUNT(*) FROM Listings
		WHERE UserId = u.UserId
		AND Enabled = 1
	) AS ListingCount
FROM Users AS u
LEFT JOIN Verifications AS v
	ON v.UserId = u.UserId


GO