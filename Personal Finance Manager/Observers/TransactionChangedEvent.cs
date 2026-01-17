namespace Personal_Finance_Manager.Observers;

public record TransactionChangedEvent(
    string OwnerUserId,
    int TransactionId,
    int? AccountId,
    string Action,       
    DateTime Time
);
