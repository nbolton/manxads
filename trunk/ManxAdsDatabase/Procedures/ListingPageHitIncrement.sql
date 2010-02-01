 CREATE PROCEDURE ListingPageHitIncrement
 (
	@ListingId int
 )
 AS
 
 UPDATE Listings SET PageHits = PageHits + 1 WHERE ListingID = @ListingID
 
 GO