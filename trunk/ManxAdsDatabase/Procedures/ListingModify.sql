IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'ListingModify')
	BEGIN
		DROP  Procedure  ListingModify
	END

GO

CREATE Procedure ListingModify
(
	@ListingId int,
	@SellerId int,
    @Title nvarchar(50),
    @Details text,
    @PriceValue money,
    @PriceType int,
    @CreateDate datetime,
    @UpdateDate datetime,
    @BoostDate datetime,
    @ShowLandline bit,
    @ShowMobile bit,
    @ShowLocation bit,
    @Enabled bit,
    @DetailsType int,
    @LocationId int
)
AS

UPDATE Listings SET
	UserId = @SellerId,
	Title = @Title,
	Details = @Details,
	PriceValue = @PriceValue,
	PriceType = @PriceType,
	CreateDate = @CreateDate,
	UpdateDate = @UpdateDate,
	BoostDate = @BoostDate,
	ShowLandline = @ShowLandline,
	ShowMobile = @ShowMobile,
	ShowLocation = @ShowLocation,
	LocationId = @LocationId,
	Enabled = @Enabled,
    DetailsType = @DetailsType
WHERE ListingId = @ListingId

GO