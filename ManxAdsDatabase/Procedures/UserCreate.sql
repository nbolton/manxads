alter Procedure UserCreate
(
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
    @VerifyAuthCode nvarchar(255),
    @TradingName nvarchar(100),
    @TradingWebsite nvarchar(150),
    @TraderType int,
    @LastIp nvarchar(50),
    @InsertId int OUTPUT,
    @EmailOccupied bit OUTPUT
)
AS

DECLARE @EmailUsageCount int

SELECT @EmailUsageCount = COUNT(*)
FROM Users WHERE EmailAddress = @EmailAddress

IF @EmailUsageCount <> 0
BEGIN
	SET @EmailOccupied = 1
	RETURN
END

INSERT INTO
Users (
	SocialTitleType, Forename, Surname, EmailAddress, Password,
	LandlineArea, LandlinePhone, MobileArea, MobilePhone, LocationId, UserType, EmailOptOut,
	LastActive, CreateDate, UpdateDate, RegisterType, TradingName, TradingWebsite, TraderType, LastIp
)
VALUES (
	@SocialTitleType, @Forename, @Surname, @EmailAddress, @Password,
	@LandlineArea, @LandlinePhone, @MobileArea, @MobilePhone, @LocationId, @UserType, @EmailOptOut,
	@LastActive, @CreateDate, @UpdateDate, @RegisterType, @TradingName, @TradingWebsite, @TraderType, @LastIp
)

SET @InsertId = SCOPE_IDENTITY()

INSERT INTO
Verifications (UserId, AuthCode)
VALUES (@InsertId, @VerifyAuthCode)

GO