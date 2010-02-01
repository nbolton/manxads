IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'ListingCreate')
	BEGIN
		DROP  Procedure  ListingCreate
	END

GO

CREATE Procedure ListingCreate
(
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
    @LocationId int,
    @Enabled bit,
    @DetailsType int,
    @InsertId int OUTPUT
)
AS

INSERT INTO Listings
	(UserId, Title, Details, PriceValue, PriceType, CreateDate, UpdateDate, 
	BoostDate, ShowLandline, ShowMobile, ShowLocation, LocationId, Enabled, DetailsType)
VALUES
	(@SellerId, @Title, @Details, @PriceValue, @PriceType, @CreateDate, @UpdateDate, 
	@BoostDate, @ShowLandline, @ShowMobile, @ShowLocation, @LocationId, @Enabled, @DetailsType)
	
SET @InsertId = SCOPE_IDENTITY()

GO