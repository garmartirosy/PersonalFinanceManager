namespace Personal_Finance_Manager.Observers;

public class TransactionEventBus
{
    private List<ITransactionObserver> _observers = new List<ITransactionObserver>();

    public void Subscribe(ITransactionObserver observer)
    {
        _observers.Add(observer);
    }

    public async Task PublishAsync(TransactionEvent evt)
    {
        foreach (var obs in _observers)
        {
            await obs.Handle(evt);
        }
    }
}
