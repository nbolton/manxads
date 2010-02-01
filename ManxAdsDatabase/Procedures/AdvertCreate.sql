alter Procedure AdvertCreate
(
	@AdvertiserId int,
    @SizeType int,
    @FormatType int,
    @PositionType int,
    @Title nvarchar(50),
    @SiteWide bit,
    @RotateFrequency int,
    @Hyperlink nvarchar(200),
    @Authorised bit,
    @Html nvarchar(MAX),
    @InsertId int OUTPUT
)
AS

INSERT INTO Adverts
	(UserId,
	SizeType,
	FormatType,
	PositionType,
	Title,
	SiteWide,
	RotateFrequency,
	Hyperlink,
	Authorised,
	Html)
VALUES
	(@AdvertiserId,
	@SizeType,
	@FormatType,
	@PositionType,
	@Title,
	@SiteWide,
	@RotateFrequency,
	@Hyperlink,
	@Authorised,
	@Html)
	
SET @InsertId = SCOPE_IDENTITY()

GO