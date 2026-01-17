using Personal_Finance_Manager.Undo;
namespace Personal_Finance_Manager.Undo.Mementos;

public sealed record DeleteTransactionMemento(DateTime CreatedDate, int TransactionId) : IOperationMemento
{
    public string OperationName => "DeleteTransaction";
}
