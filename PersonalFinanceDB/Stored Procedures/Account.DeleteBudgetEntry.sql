CREATE PROCEDURE Account.DeleteBudgetEntry
(
    @BudgetEntryId INT,
    @OwnerUserId NVARCHAR(450)
)
AS SET NOCOUNT ON

DELETE be FROM Account.BudgetEntry be
INNER JOIN Account.Budget b ON be.BudgetId = b.Id
WHERE be.Id = @BudgetEntryId AND be.OwnerUserId = @OwnerUserId AND b.OwnerUserId = @OwnerUserId