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