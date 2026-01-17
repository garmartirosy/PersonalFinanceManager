namespace Personal_Finance_Manager.Observers;

public interface ITransactionObserver
{
    Task Handle(TransactionEvent evt);
}
