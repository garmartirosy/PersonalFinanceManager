using Microsoft.Extensions.Logging;

namespace Personal_Finance_Manager.Observers;

public class AuditLogObserver : ITransactionObserver
{
    private readonly ILogger<AuditLogObserver> _logger;

    public AuditLogObserver(ILogger<AuditLogObserver> logger)
    {
        _logger = logger;
    }

    public Task Handle(TransactionEvent evt)
    {
        _logger.LogInformation("Audit: {Action} transaction {TxId} for user {UserId}",
            evt.Action, evt.TransactionId, evt.UserId);

        return Task.CompletedTask;
    }
}
