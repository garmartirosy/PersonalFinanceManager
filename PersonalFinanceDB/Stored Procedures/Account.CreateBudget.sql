CREATE PROCEDURE Account.CreateBudget
(
    @OwnerUserId NVARCHAR(450),
    @Name NVARCHAR(100),
    @Balance DECIMAL(15, 2) = 0
)
AS SET NOCOUNT ON

INSERT INTO Account.Budget (OwnerUserId, Name, Balance)
VALUES (@OwnerUserId, @Name, @Balance)
