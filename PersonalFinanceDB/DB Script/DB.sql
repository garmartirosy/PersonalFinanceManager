USE PersonalFinanceDB
GO

CREATE SCHEMA Account
GO



CREATE TABLE Account.AccountType
(
    Id INT IDENTITY NOT NULL PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL
)
GO

INSERT INTO Account.AccountType (Name)
VALUES
('Checking'),
('Savings' ),
('Credit'  ) 
GO


CREATE TABLE Account.Account
(
    Id INT IDENTITY NOT NULL PRIMARY KEY,
    OwnerUserId NVARCHAR(450) NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    AccountTypeId INT NOT NULL,
    Balance DECIMAL(15, 2) NOT NULL DEFAULT (0),
    DateCreated DATETIME2 NOT NULL DEFAULT (GETDATE()),
    IsActive BIT NOT NULL DEFAULT (1),

    CONSTRAINT FK_Account_OwnerUserId
        FOREIGN KEY (OwnerUserId) REFERENCES dbo.AspNetUsers(Id),

    CONSTRAINT FK_Account_AccountType
        FOREIGN KEY (AccountTypeId) REFERENCES Account.AccountType(Id)
)
GO



CREATE TABLE Account.TransactionType
(
    Id TINYINT NOT NULL PRIMARY KEY,
    Name VARCHAR(50) NOT NULL
)
GO


INSERT INTO Account.TransactionType VALUES (1, 'Income'), (2, 'Expense')
GO

CREATE TABLE Account.[Transaction]
(
	Id INT IDENTITY NOT NULL PRIMARY KEY,
	AccountId INT NOT NULL,
	Description VARCHAR(255) NULL,
	DestinationName VARCHAR(100) NOT NULL,
	TransactionTypeId TINYINT NOT NULL,
    DateCreated DATETIME2 NOT NULL DEFAULT (GETDATE()),
	Amount DECIMAL(15,2) NOT NULL,
	IsActive BIT NOT NULL DEFAULT (1)

	CONSTRAINT FK_Account_Transaction_AccountId FOREIGN KEY (AccountId) REFERENCES Account.Account(Id),
	CONSTRAINT FK_Account_Transaction_TransactionTypeId FOREIGN KEY (TransactionTypeId) REFERENCES Account.TransactionType(Id)

)
GO



CREATE TABLE Account.Budget
(

    Id INT IDENTITY NOT NULL PRIMARY KEY,
    OwnerUserId NVARCHAR(450) NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Balance DECIMAL(15, 2) NOT NULL DEFAULT (0),
    DateCreated DATETIME2 NOT NULL DEFAULT (GETDATE()),

    CONSTRAINT FK_Budget_OwnerUserId FOREIGN KEY (OwnerUserId) REFERENCES dbo.AspNetUsers(Id)
)
GO



CREATE TABLE Account.BudgetEntry
(

    Id INT IDENTITY NOT NULL PRIMARY KEY,
    BudgetId INT NOT NULL,
    OwnerUserId NVARCHAR(450) NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Amount DECIMAL(15, 2) NOT NULL

    CONSTRAINT FK_BudgetEntry_BudgetId FOREIGN KEY (BudgetId) REFERENCES Account.Budget(Id),
    CONSTRAINT FK_BudgetEntry_OwnerUserId FOREIGN KEY (OwnerUserId) REFERENCES dbo.AspNetUsers(Id)
)
GO





CREATE PROCEDURE Account.CreateAccount (
	@OwnerUserId NVARCHAR(450),
    @Name NVARCHAR(100),
    @AccountTypeId INT,
    @Balance DECIMAL(15, 2) = 0
)
AS SET NOCOUNT ON


INSERT INTO Account.Account (OwnerUserId, Name, AccountTypeId, Balance)
VALUES (@OwnerUserId, @Name, @AccountTypeId, @Balance)
GO


   
CREATE PROCEDURE Account.CreateBudget
(
    @OwnerUserId NVARCHAR(450),
    @Name NVARCHAR(100),
    @Balance DECIMAL(15, 2) = 0
)
AS SET NOCOUNT ON

INSERT INTO Account.Budget (OwnerUserId, Name, Balance)
VALUES (@OwnerUserId, @Name, @Balance)
GO



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
GO



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
GO



CREATE PROCEDURE Account.DeleteAccount (
	@AccountId INT,
	@OwnerUserId NVARCHAR(450)
)
AS
SET NOCOUNT ON

UPDATE Account.Account SET IsActive = 0 WHERE Id = @AccountId AND OwnerUserId = @OwnerUserId

IF @@ROWCOUNT = 0 RETURN 1

RETURN 0
GO



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
GO



CREATE PROCEDURE Account.DeleteBudgetEntry
(
    @BudgetEntryId INT,
    @OwnerUserId NVARCHAR(450)
)
AS SET NOCOUNT ON

DELETE be FROM Account.BudgetEntry be
INNER JOIN Account.Budget b ON be.BudgetId = b.Id
WHERE be.Id = @BudgetEntryId AND be.OwnerUserId = @OwnerUserId AND b.OwnerUserId = @OwnerUserId
GO



CREATE PROCEDURE Account.DeleteTransaction(
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
    INNER JOIN Account.Account a ON a.Id = t.AccountId
    WHERE t.Id = @TransactionId
      AND t.IsActive = 1
      AND a.OwnerUserId = @OwnerUserId
      AND a.IsActive = 1

    IF @AccountId IS NULL
    BEGIN
        ROLLBACK TRANSACTION
        RETURN 1
    END

    UPDATE Account.[Transaction]
    SET IsActive = 0
    WHERE Id = @TransactionId AND IsActive = 1

    UPDATE Account.Account
    SET Balance = Balance - IIF(@TransactionTypeId = 1, 1, -1) * @Amount
    WHERE Id = @AccountId

    COMMIT TRANSACTION
    RETURN 0
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH
GO


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
GO



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
GO



CREATE PROCEDURE Account.GetBudgetById
(
    @Id INT,
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
WHERE b.Id = @Id AND b.OwnerUserId = @OwnerUserId
GO


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
GO



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
GO


CREATE PROCEDURE Account.GetTotalSpent(
	@BudgetId INT
)
AS SET NOCOUNT ON

SELECT ISNULL(SUM(Amount), 0) TotalSpent FROM Account.BudgetEntry WHERE BudgetId = @BudgetId
GO




CREATE PROCEDURE Account.GetTransactionById(
    @TransactionId INT,
    @OwnerUserId NVARCHAR(450)
)
AS
SET NOCOUNT ON

SELECT
    t.Id,
    t.AccountId,
    t.Description,
    t.DestinationName,
    t.TransactionTypeId,
    t.Amount,
    t.DateCreated
FROM Account.[Transaction] t INNER JOIN Account.Account a ON a.Id = t.AccountId
WHERE t.Id = @TransactionId AND t.IsActive = 1 AND a.OwnerUserId = @OwnerUserId AND a.IsActive = 1
GO




CREATE PROCEDURE Account.GetTransactionsByAccount
(
    @AccountId INT,
    @OwnerUserId NVARCHAR(450)
)
AS
SET NOCOUNT ON

SELECT
    t.Id,
    t.AccountId,
    t.Description,
    t.DestinationName,
    t.TransactionTypeId,
    tt.Name AS TransactionType,
    t.Amount,
    t.DateCreated
FROM Account.[Transaction] t INNER JOIN Account.Account a ON a.Id = t.AccountId
                             INNER JOIN Account.TransactionType tt ON tt.Id = t.TransactionTypeId
WHERE t.AccountId = @AccountId
    AND a.OwnerUserId = @OwnerUserId
    AND t.IsActive = 1
    AND a.IsActive = 1
ORDER BY t.Id DESC
GO



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
GO



CREATE PROCEDURE Account.UpdateAccount(
	@Id INT,
	@OwnerUserId NVARCHAR(450),
	@Name NVARCHAR(100),
	@AccountTypeId INT,
	@Balance DECIMAL(15, 2)
)
AS SET NOCOUNT ON

UPDATE Account.Account SET Name = @Name, AccountTypeId = @AccountTypeId, Balance = @Balance
WHERE Id = @Id AND OwnerUserId = @OwnerUserId AND IsActive = 1

IF @@ROWCOUNT = 0 RETURN 1

RETURN 0






