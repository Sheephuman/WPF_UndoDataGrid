namespace WPF_UndoDataGrid.classes
{
    public class UndoManager
    {

        // Undo用のスタック（直近の変更が積まれている）
        private ConsCell<ChangeGrid> undoStack = new ConsCell<ChangeGrid>();


        // Redo用のスタック（Undoした変更が積まれる）
        private ConsCell<ChangeGrid> redoStack = new ConsCell<ChangeGrid>();

        // 新しい操作を積む
        public void AddChange(ChangeGrid change)
        {
            undoStack = undoStack.Push(change);
            redoStack = new ConsCell<ChangeGrid>(); // 新規操作でRedoは消える
        }






        // Undo: 最新の変更を元に戻す
        public void Undo()
        {
            if (undoStack.IsEmpty)
                return;

            ChangeGrid change = undoStack.Head;
            change.Revert();  // 変更を取り消す
            undoStack = undoStack.Tail;  //最新を取り除き、次の変更が先頭になる
            redoStack = redoStack.Push(change); // Redoスタックへ移す
        }

        // Redo: Undoした変更を再適用
        public void Redo()
        {
            if (redoStack.IsEmpty)
                return;

            var change = redoStack.Head;
            change.Apply();
            redoStack = redoStack.Tail;
            undoStack = undoStack.Push(change);
        }

    }
}
