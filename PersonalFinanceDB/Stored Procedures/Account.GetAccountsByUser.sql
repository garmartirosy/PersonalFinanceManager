CREATE PROCEDURE Account.GetAccountsByUser(
    @UserId NVARCHAR(450)
)
AS
SET NOCOUNT ON

SELECT
    a.Id,
    a.Name,
    a.OwnerUserId,
    at.Name AS AccountType,
    a.Balance,
    a.DateCreated,
    a.IsActive
FROM Account.Account a
INNER JOIN Account.AccountType at ON a.AccountTypeId = at.Id
WHERE a.OwnerUserId = @UserId
