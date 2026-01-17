CREATE PROCEDURE Account.UpdateAccount(
	@Id INT,
	@OwnerUserId NVARCHAR(450),
	@Name NVARCHAR(100),
	@AccountTypeId INT,
	@Balance DECIMAL(15, 2)
)
AS SET NOCOUNT ON

UPDATE Account.Account SET Name = @Name, AccountTypeId = @AccountTypeId, Balance = @Balance
WHERE Id = @Id AND OwnerUserId = @OwnerUserId AND IsActive = 1

IF @@ROWCOUNT = 0 RETURN 1

RETURN 0
