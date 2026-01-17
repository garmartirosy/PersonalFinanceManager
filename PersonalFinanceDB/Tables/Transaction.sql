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
