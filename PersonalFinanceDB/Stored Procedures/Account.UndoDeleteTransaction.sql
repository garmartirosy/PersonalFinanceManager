CREATE PROCEDURE Account.UndoDeleteTransaction(
    @OwnerUserId NVARCHAR(450),
    @TransactionId INT
)
AS
SET NOCOUNT ON
SET XACT_ABORT ON

BEGIN TRY
    BEGIN TRANSACTION

    DECLARE @AccountId INT
    DECLARE @TransactionTypeId TINYINT
    DECLARE @Amount DECIMAL(15,2)

    SELECT
        @AccountId = t.AccountId,
        @TransactionTypeId = t.TransactionTypeId,
        @Amount = t.Amount
    FROM Account.[Transaction] t
    INNER JOIN Account.Account a
        ON a.Id = t.AccountId
    WHERE t.Id = @TransactionId
      AND t.IsActive = 0
      AND a.OwnerUserId = @OwnerUserId
      AND a.IsActive = 1

    IF @AccountId IS NULL BEGIN
        ROLLBACK TRANSACTION
        RETURN 1
    END

    UPDATE Account.[Transaction]
    SET IsActive = 1
    WHERE Id = @TransactionId
      AND IsActive = 0

    UPDATE Account.Account
    SET Balance = Balance + IIF(@TransactionTypeId = 1, 1, -1) * @Amount
    WHERE Id = @AccountId

    COMMIT TRANSACTION
    RETURN 0
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH
