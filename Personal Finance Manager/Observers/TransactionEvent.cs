namespace Personal_Finance_Manager.Observers;

public class TransactionEvent
{
    public string UserId { get; set; }
    public int TransactionId { get; set; }
    public int? AccountId { get; set; }
    public string Action { get; set; }  
    public DateTime Time{ get; set; }

    public TransactionEvent(string userId, int transactionId, int? accountId, string action)
    {
        UserId = userId;
        TransactionId = transactionId;
        AccountId = accountId;
        Action = action;
        Time = DateTime.UtcNow;
    }
}
