IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'CategoryModifyPriority')
	BEGIN
		DROP  Procedure  CategoryModifyPriority
	END

GO

CREATE Procedure CategoryModifyPriority
(
	@CategoryId int,
	@Priority int
)
AS

UPDATE Categories SET Priority = @Priority
WHERE CategoryId = @CategoryId