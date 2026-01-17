CREATE PROCEDURE Account.GetAccountById(
    @AccountId INT,
    @OwnerUserId NVARCHAR(450)
)
AS
SET NOCOUNT ON

SELECT
    a.Id AS Id,
    a.Name,
    a.OwnerUserId,
    a.AccountTypeId,
    at.Name AS AccountType,
    a.Balance,
    a.DateCreated,
    a.IsActive
FROM Account.Account a INNER JOIN Account.AccountType at 
ON a.AccountTypeId = at.Id
WHERE a.Id = @AccountId AND a.OwnerUserId = @OwnerUserId

