using WPF_UndoDataGrid.classes.WPF_UndoDataGrid;

namespace WPF_UndoDataGrid.classes
{
    public class CellUndoManager
    {
        private ConsCell<ChangeCell> cellUndoStack = new ConsCell<ChangeCell>();

        private ConsCell<ChangeCell> cellRedoStack = new ConsCell<ChangeCell>();




        public void AddCellChange(ChangeCell change)
        {
            cellUndoStack = cellUndoStack.Push(change);
            cellRedoStack = new ConsCell<ChangeCell>(); // 新規操作でRedoは消える
        }



        public void CellUndo()
        {
            if (cellUndoStack.IsEmpty)
                return;

            ChangeCell change = cellUndoStack.Head;
            change.Revert();  // 変更を取り消す
            cellUndoStack = cellUndoStack.Tail;  //最新を取り除き、次の変更が先頭になる
            cellRedoStack = cellRedoStack.Push(change); // Redoスタックへ移す
        }


        // Redo: Undoした変更を再適用
        public void CellRedo()
        {
            if (cellRedoStack.IsEmpty)
                return;

            var change = cellRedoStack.Head;
            change.Apply();
            cellRedoStack = cellRedoStack.Tail;
            cellUndoStack = cellUndoStack.Push(change);
        }

    }
}
