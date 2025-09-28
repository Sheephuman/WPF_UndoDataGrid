namespace AdapterInterface
{

    public interface IUndoableControl<T>
    {
        T Value { get; set; }
    }
    public interface IChangeAction
    {
        void Apply();  // Redo 相当
        void Revert(); // Undo 相当
    }


    public abstract class UndoableControl<T> : IUndoableControl<T>
    {
        public abstract T Value { get; set; }
    }

}