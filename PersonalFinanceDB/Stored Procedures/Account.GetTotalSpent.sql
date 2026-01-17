CREATE PROCEDURE Account.GetTotalSpent(
	@BudgetId INT
)
AS SET NOCOUNT ON

SELECT ISNULL(SUM(Amount), 0) TotalSpent FROM Account.BudgetEntry WHERE BudgetId = @BudgetId

