alter Procedure AdvertModify
(
	@AdvertId int,
	@AdvertiserId int,
    @SizeType int,
    @FormatType int,
    @PositionType int,
    @Title nvarchar(50),
    @SiteWide bit,
    @RotateFrequency int,
    @Hyperlink nvarchar(200),
    @Html nvarchar(MAX),
    @Authorised bit
)
AS

UPDATE Adverts SET
	UserId = @AdvertiserId,
    SizeType = @SizeType,
    FormatType = @FormatType,
    PositionType = @PositionType,
    Title = @Title,
    SiteWide = @SiteWide,
    RotateFrequency = @RotateFrequency,
    Hyperlink = @Hyperlink,
    Authorised = @Authorised,
    Html = @Html
WHERE AdvertId = @AdvertId

GO