namespace WPF_UndoDataGrid.classes
{
    /// <summary>
    /// Undo/Redo を管理するジェネリッククラス
    /// T は IChangeAction を実装した型である必要がある
    /// </summary>
    public class UndoManager<T> where T : IChangeAction
    {
        // Undo用のスタック
        private ConsCell<T> undoStack = new ConsCell<T>();

        // Redo用のスタック
        private ConsCell<T> redoStack = new ConsCell<T>();

        /// <summary>
        /// 新しい操作を追加
        /// </summary>
        public void AddChange(T change)
        {
            undoStack = undoStack.Push(change);
            redoStack = new ConsCell<T>(); // 新規操作でRedoはクリア
        }

        /// <summary>
        /// Undo: 最新の変更を元に戻す
        /// </summary>
        public void Undo()
        {
            if (undoStack.IsEmpty)
                return;

            T change = undoStack.Head;
            change.Revert();
            undoStack = undoStack.Tail;
            redoStack = redoStack.Push(change);
        }

        /// <summary>
        /// Redo: Undoした変更を再適用
        /// </summary>
        public void Redo()
        {
            if (redoStack.IsEmpty)
                return;

            T change = redoStack.Head;
            change.Apply();
            redoStack = redoStack.Tail;
            undoStack = undoStack.Push(change);
        }
    }

    /// <summary>
    /// Undo/Redo 対象の共通インターフェース
    /// </summary>
    public interface IChangeAction
    {
        void Apply();
        void Revert();
    }
}
