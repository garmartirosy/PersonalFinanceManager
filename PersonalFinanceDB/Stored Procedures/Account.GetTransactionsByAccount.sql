CREATE PROCEDURE Account.GetTransactionsByAccount
(
    @AccountId INT,
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
    tt.Name AS TransactionType,
    t.Amount,
    t.DateCreated
FROM Account.[Transaction] t INNER JOIN Account.Account a ON a.Id = t.AccountId
                             INNER JOIN Account.TransactionType tt ON tt.Id = t.TransactionTypeId
WHERE t.AccountId = @AccountId
    AND a.OwnerUserId = @OwnerUserId
    AND t.IsActive = 1
    AND a.IsActive = 1
ORDER BY t.Id DESC
