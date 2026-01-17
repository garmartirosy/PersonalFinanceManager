CREATE PROCEDURE Account.GetBudgetEntriesByBudget
(
    @BudgetId INT,
    @OwnerUserId NVARCHAR(450)
)
AS
SET NOCOUNT ON

SELECT
    be.Id AS Id,
    be.BudgetId,
    be.OwnerUserId,
    be.Name,
    be.Amount
FROM Account.BudgetEntry be
INNER JOIN Account.Budget b ON be.BudgetId = b.Id
WHERE be.BudgetId = @BudgetId AND be.OwnerUserId = @OwnerUserId AND b.OwnerUserId = @OwnerUserId
ORDER BY be.Id DESC