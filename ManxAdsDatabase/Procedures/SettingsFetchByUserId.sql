IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SettingsFetchByUserId')
	BEGIN
		DROP  Procedure  SettingsFetchByUserId
	END

GO

CREATE Procedure SettingsFetchByUserId
(
	@UserId int
)
AS

SELECT s.KeyName, us.KeyValue
FROM UserSettings AS us
LEFT JOIN Settings AS s
	ON us.SettingId = s.SettingId
WHERE us.UserId = @UserId

GO