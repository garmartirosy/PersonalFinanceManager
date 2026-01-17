CREATE PROCEDURE Account.GetBudgetsByUser
(
    @OwnerUserId NVARCHAR(450)
)
AS
SET NOCOUNT ON

SELECT
    b.Id AS Id,
    b.Name,
    b.OwnerUserId,
    b.Balance,
    b.DateCreated
FROM Account.Budget b
WHERE b.OwnerUserId = @OwnerUserId
ORDER BY b.DateCreated DESC, b.Id DESC