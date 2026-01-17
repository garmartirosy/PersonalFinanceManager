namespace Personal_Finance_Manager.Undo;

public sealed class UndoService
{
    private readonly Dictionary<string, (IUndoableOperation Op, IOperationMemento Mem)> _last
        = new();

    public async Task ExecuteAndRemember(string ownerUserId, IUndoableOperation op)
    {
        var mem = await op.Execute();
        _last[ownerUserId] = (op, mem);
    }

    public async Task<bool> UndoLast(string ownerUserId)
    {
        if (!_last.TryGetValue(ownerUserId, out var item))
            return false; 

        await item.Op.Undo(item.Mem);

        _last.Remove(ownerUserId);
        return true;
    }

    public bool HasUndo(string ownerUserId)
    {
        return _last.ContainsKey(ownerUserId);
    }
}
