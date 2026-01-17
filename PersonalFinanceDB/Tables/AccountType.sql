CREATE TABLE Account.AccountType
(
    Id INT IDENTITY NOT NULL PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL
)

INSERT INTO Account.AccountType (Name)
VALUES
('Checking'),
('Savings' ),
('Credit'  ) 
