IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SettingsModify')
	BEGIN
		DROP  Procedure  SettingsModify
	END

GO

CREATE Procedure SettingsModify
(
	@UserId int,
	@KeyName nvarchar (50),
	@KeyValue nvarchar (50)
)
AS

DECLARE @SettingId int
DECLARE @UserSettingId int

SELECT @SettingId = SettingId
FROM Settings WHERE KeyName = @KeyName

IF @SettingId IS NULL
BEGIN
	INSERT INTO
	Settings (KeyName)
	VALUES (@KeyName)
	
	SET @SettingId = SCOPE_IDENTITY()
END

IF @UserId <> -1
BEGIN

	SELECT @UserSettingId = UserSettingId
	FROM UserSettings
	WHERE UserId = @UserId
	AND SettingId = @SettingId
	
	IF NOT @UserSettingId IS NULL
	BEGIN
		UPDATE UserSettings
		SET KeyValue = @KeyValue
		WHERE UserSettingId = @UserSettingId
	END
	ELSE
	BEGIN
		INSERT INTO
		UserSettings (UserId, SettingId, KeyValue)
		VALUES (@UserId, @SettingId, @KeyValue)
	END
END
ELSE
BEGIN
	UPDATE Settings
	SET KeyValue = @KeyValue
	WHERE SettingId = @SettingId
END

GO