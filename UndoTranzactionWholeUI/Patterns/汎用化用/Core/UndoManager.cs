using AdapterInterface;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UndoTransaction_SnapShot.Generics.Container;
using UndoTransaction_SnapShot.MVVM.Model;
using UndoTransaction_SnapShot.Patterns.汎用化用.Core;

namespace UndoTransaction_SnapShot
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
            index++;
            redoStack = new ConsCell<T>(); // 新規操作でRedoはクリア


            DumpStacks();

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

            if (!undoStack.IsEmpty && !undoStack.Tail.IsEmpty)
                Debug.WriteLine("Undo要素数:" + undoStack.Count());

            redoStack = redoStack.Push(change);
        }

        public void Undo(ChangeRowWithAbstract changeRows)
        {
            int PreviousCount = undoStack.Count - 1;

            if (undoStack.IsEmpty)
                return;

            T change = undoStack.Head;


            change.Revert();



            for (int i = 1; undoStack.Count > i; i++)
                undoStack = undoStack.Tail;


            //    Debug.WriteLine($"{undoStack.Count:番目}" + undoStack[undoStack.Count - 1].ToString());

            Debug.WriteLine(undoStack.ToString());



            int Stackindex = redoStack.fromCounter();

            Debug.WriteLine("Undo要素数:" + undoStack.Count());

            Debug.WriteLine("--------");

            Debug.WriteLine("redoStack要素数" + redoStack.Count());
            //for (int x = 0; changeRows._deltaValue.Count > x;)
            //{

            if (Stackindex == 0)
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

        public void Redo(ChangeRowWithAbstract changeRow)
        {
            if (redoStack.IsEmpty)
                return;

            T change = redoStack.Head;


            Debug.WriteLine("redoStack要素数" + redoStack.Count());

            changeRow.ApplyMultiValues();

            redoStack = redoStack.Tail;

            undoStack = undoStack.Push(change);

        }


        int index = 0;

        internal void Snap(ObservableCollection<Person> people, ChangeRowWithAbstract changeRow)
        {


            if (changeRow is null)
                return;


            if (changeRow._deltaValue is null)
                return;



            changeRow._deltaValue.Clear();

            changeRow._hasMultiValue = true;

            // その時点の People をディープコピー
            var copy = new ObservableCollection<Person>(
                people.Select(p => new Person { Name = p.Name, Age = p.Age, City = p.City })
            );

            foreach (var p in copy)
                Debug.WriteLine($"{p.Name}, {p.Age}, {p.City}");

            // SnapShotAction にラップして渡す
            var act = new SnapShotAction<Person>(
                people,
                new ConsCell<ObservableCollection<Person>>(copy, new ConsCell<ObservableCollection<Person>>())
            );



            var undoManager = new UndoManager<SnapShotAction<Person>>();



            // Undo スタックに積む
            undoManager.AddChange(act);


        }



        public void DumpStacks()
        {
            Debug.WriteLine("=== UndoStack ===");
            Debug.WriteLine(undoStack.ToString());

            Debug.WriteLine("=== RedoStack ===");
            Debug.WriteLine(redoStack.ToString());
        }

        public void DumpUndoStacks()
        {
            Debug.WriteLine("=== UndoStack ===");
            Debug.WriteLine(undoStack.ToString());
        }
        public void DumpRedoStacks()
        {
            Debug.WriteLine("=== RedoStack ===");
            Debug.WriteLine(redoStack.ToString());
        }

    }
}
