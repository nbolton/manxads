IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'UserModify')
	BEGIN
		DROP  Procedure  UserModify
	END

GO

CREATE Procedure UserModify
(
	@UserId int,
    @SocialTitleType int,
    @Forename nvarchar(50),
    @Surname nvarchar(50),
    @EmailAddress nvarchar(100),
    @Password nvarchar(50),
    @LandlineArea nvarchar(10),
    @LandlinePhone nvarchar(50),
    @MobileArea nvarchar(10),
    @MobilePhone nvarchar(50),
    @LocationId int,
    @UserType int,
    @EmailOptOut bit,
    @LastActive datetime,
    @CreateDate datetime,
    @UpdateDate datetime,
    @RegisterType int,
    @TradingName nvarchar(100),
    @TradingWebsite nvarchar(150),
    @TraderType int,
    @IsVerified bit,
    @VerifyAuthCode nvarchar(255),
    @BanUntil datetime
)
AS

UPDATE Users SET
    SocialTitleType = @SocialTitleType,
    Forename = @Forename,
    Surname = @Surname,
    EmailAddress = @EmailAddress,
    LandlineArea = @LandlineArea,
    LandlinePhone = @LandlinePhone,
    MobileArea = @MobileArea,
    MobilePhone = @MobilePhone,
    LocationId = @LocationId,
    UserType = @UserType,
    EmailOptOut = @EmailOptOut,
    LastActive = @LastActive,
    CreateDate = @CreateDate,
    UpdateDate = @UpdateDate,
    RegisterType = @RegisterType,
    TradingName = @TradingName,
    TradingWebsite = @TradingWebsite,
    TraderType = @TraderType,
    IsVerified = @IsVerified,
    BanUntil = @BanUntil
WHERE UserId = @UserId

IF NOT @Password IS NULL
BEGIN
	UPDATE Users SET
	Password = @Password
	WHERE UserId = @UserId
END

IF NOT @VerifyAuthCode IS NULL
	BEGIN
	IF EXISTS(SELECT * FROM Verifications WHERE UserId = @UserId)
	BEGIN
		UPDATE Verifications SET
			AuthCode = @VerifyAuthCode
		WHERE UserId = @UserId
	END
	ELSE
	BEGIN
		INSERT INTO Verifications
			(UserId, AuthCode)
		VALUES
			(@UserId, @VerifyAuthCode)
	END
	END

GO