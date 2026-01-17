CREATE PROCEDURE Account.CreateBudgetEntry
(
    @BudgetId INT,
    @OwnerUserId NVARCHAR(450),
    @Name NVARCHAR(100),
    @Amount DECIMAL(15, 2)
)
AS SET NOCOUNT ON

IF NOT EXISTS (
    SELECT *
    FROM Account.Budget
    WHERE Id = @BudgetId AND OwnerUserId = @OwnerUserId
)
RETURN 1

INSERT INTO Account.BudgetEntry (BudgetId, OwnerUserId, Name, Amount)
VALUES (@BudgetId, @OwnerUserId, @Name, @Amount)

RETURN 0
