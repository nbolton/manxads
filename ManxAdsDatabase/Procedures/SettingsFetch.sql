IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SettingsFetch')
	BEGIN
		DROP  Procedure  SettingsFetch
	END

GO

CREATE Procedure SettingsFetch AS

SELECT KeyName, KeyValue
FROM Settings

GO