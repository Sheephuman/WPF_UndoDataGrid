using AdapterInterface;
using UndoTransaction_SnapShot.Generics.Container;

namespace UndoTransaction_SnapShot.CompositeAction
{
    internal class ChangeTransaction<T> : IChangeAction
    {

        private readonly Action<T> _apply;
        private readonly T _value;

        // Undo用のスタック
        private ConsCell<T> undoStack = new ConsCell<T>();

        // Redo用のスタック
        private ConsCell<T> redoStack = new ConsCell<T>();

        public ChangeTransaction(T Value, Action<T> apply)
        {
            _apply = apply;
            _value = Value;
        }


        private readonly List<IChangeAction> _buffer = new();



        public void Add(IChangeAction action) => _buffer.Add(action);

        public void Apply()
        {
            _apply?.Invoke(_value);

            foreach (var action in _buffer.AsEnumerable().Reverse())
            {
                action.Apply();
            }
        }

        public CompositeTransaction<object> Commit()
        {
            return new CompositeTransaction<object>(_buffer);
        }

        public void Revert()
        {
            foreach (var action in _buffer.AsEnumerable().Reverse())
                action.Revert();


        }

        internal void AddChange(T change)
        {

            undoStack = undoStack.Push(change);
            redoStack = new ConsCell<T>(); // 新規操作でRedoはクリア

        }
    }
}
