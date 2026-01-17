CREATE PROCEDURE Account.CreateAccount (
	@OwnerUserId NVARCHAR(450),
    @Name NVARCHAR(100),
    @AccountTypeId INT,
    @Balance DECIMAL(15, 2) = 0
)
AS SET NOCOUNT ON


INSERT INTO Account.Account (OwnerUserId, Name, AccountTypeId, Balance)
VALUES (@OwnerUserId, @Name, @AccountTypeId, @Balance)

   
