CREATE TABLE Account.TransactionType
(
    Id TINYINT NOT NULL PRIMARY KEY,
    Name VARCHAR(50) NOT NULL
)

INSERT INTO Account.TransactionType VALUES (1, 'Income'), (2, 'Expense')