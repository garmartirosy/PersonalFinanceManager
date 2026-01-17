CREATE PROCEDURE Account.DeleteAccount (
	@AccountId INT,
	@OwnerUserId NVARCHAR(450)
)
AS
SET NOCOUNT ON

UPDATE Account.Account SET IsActive = 0 WHERE Id = @AccountId AND OwnerUserId = @OwnerUserId

IF @@ROWCOUNT = 0 RETURN 1

RETURN 0

