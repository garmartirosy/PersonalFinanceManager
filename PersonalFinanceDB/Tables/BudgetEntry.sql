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


