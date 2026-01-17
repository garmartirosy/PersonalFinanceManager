namespace Personal_Finance_Manager.Undo
{
    public interface IUndoableOperation
    {
        Task<IOperationMemento> Execute();
        Task Undo(IOperationMemento memento);
    }
}
