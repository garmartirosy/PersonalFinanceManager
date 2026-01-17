CREATE PROCEDURE Account.GetTransactionById(
    @TransactionId INT,
    @OwnerUserId NVARCHAR(450)
)
AS
SET NOCOUNT ON

SELECT
    t.Id,
    t.AccountId,
    t.Description,
    t.DestinationName,
    t.TransactionTypeId,
    t.Amount,
    t.DateCreated
FROM Account.[Transaction] t INNER JOIN Account.Account a ON a.Id = t.AccountId
WHERE t.Id = @TransactionId AND t.IsActive = 1 AND a.OwnerUserId = @OwnerUserId AND a.IsActive = 1
