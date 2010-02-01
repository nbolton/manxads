ALTER PROCEDURE UserEmptyRecycleBin
(
	@UserId int
)
AS

UPDATE dbo.Listings
SET Archive = 1
WHERE UserId = @UserId AND Enabled = 0