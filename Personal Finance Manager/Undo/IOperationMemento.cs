namespace Personal_Finance_Manager.Undo
{
    public interface IOperationMemento
    {
        DateTime CreatedDate { get; }
        string OperationName { get; }
    }
}
