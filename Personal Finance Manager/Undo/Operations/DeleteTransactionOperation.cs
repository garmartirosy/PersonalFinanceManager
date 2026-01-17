using Personal_Finance_Manager.Data.Repositories;
using Personal_Finance_Manager.Undo.Mementos;

namespace Personal_Finance_Manager.Undo.Operations;

public sealed class DeleteTransactionOperation : IUndoableOperation
{
    private readonly ITransactionRepository _transactionRepo;
    private readonly string _ownerUserId;
    private readonly int _transactionId;

    public DeleteTransactionOperation(
        ITransactionRepository transactionRepo,
        string ownerUserId,
        int transactionId)
    {
        _transactionRepo = transactionRepo;
        _ownerUserId = ownerUserId;
        _transactionId = transactionId;
    }

    public async Task<IOperationMemento> Execute()
    {
        var ok = await _transactionRepo.DeleteTransaction(_transactionId, _ownerUserId);

        return new DeleteTransactionMemento(DateTime.UtcNow, _transactionId);
    }

    public async Task Undo(IOperationMemento memento)
    {
        var typed = (DeleteTransactionMemento)memento;

        await _transactionRepo.UndoDeleteTransaction(_ownerUserId, typed.TransactionId);
    }
}
