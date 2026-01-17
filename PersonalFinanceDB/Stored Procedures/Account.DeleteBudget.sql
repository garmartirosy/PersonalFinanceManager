CREATE PROCEDURE Account.DeleteBudget
(
    @Id INT,
    @OwnerUserId NVARCHAR(450)
)
AS SET NOCOUNT ON
SET XACT_ABORT ON


BEGIN TRY
    BEGIN TRANSACTION

    DELETE Account.BudgetEntry
    WHERE BudgetId = @Id AND OwnerUserId = @OwnerUserId

    DELETE Account.Budget
    WHERE Id = @Id AND OwnerUserId = @OwnerUserId

    COMMIT TRANSACTION
    RETURN 0
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH