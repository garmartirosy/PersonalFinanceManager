CREATE PROCEDURE Account.CreateTransaction(
    @OwnerUserId NVARCHAR(450),
    @AccountId INT,
    @Description VARCHAR(255),
    @DestinationName VARCHAR(100),
    @TransactionTypeId TINYINT,
    @Amount DECIMAL(15,2),
    @TransactionId INT OUTPUT
)
AS
SET NOCOUNT ON
SET XACT_ABORT ON

IF NOT EXISTS(
    SELECT * FROM Account.Account
    WHERE Id = @AccountId AND OwnerUserId = @OwnerUserId AND IsActive = 1
)
RETURN 1

BEGIN TRY
    BEGIN TRANSACTION

    INSERT INTO Account.[Transaction] (AccountId, Description, DestinationName, TransactionTypeId, Amount)
    VALUES (@AccountId, @Description, @DestinationName, @TransactionTypeId, @Amount);

    SET @TransactionId = CAST(SCOPE_IDENTITY() AS INT)


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
