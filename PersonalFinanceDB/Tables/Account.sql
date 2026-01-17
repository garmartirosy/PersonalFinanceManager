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
);
