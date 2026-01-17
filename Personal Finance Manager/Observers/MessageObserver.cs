using Personal_Finance_Manager.Services;

namespace Personal_Finance_Manager.Observers;

public class MessageObserver : ITransactionObserver
{
    private readonly MessageService _msgs;

    public MessageObserver(MessageService msgs)
    {
        _msgs = msgs;
    }

    public Task Handle(TransactionEvent evt)
    {
        string msg = "Transaction updated.";

        if (evt.Action == "Deleted")
            msg = "Transaction deleted.";
        else if (evt.Action == "Undone")
            msg = "Undo completed.";
        else if (evt.Action == "Created")
            msg = "Transaction created.";

        _msgs.Set(evt.UserId, msg);
        return Task.CompletedTask;
    }
}
