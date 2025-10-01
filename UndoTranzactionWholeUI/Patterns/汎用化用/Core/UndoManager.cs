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

        int index = 0;

        internal void Snap(ObservableCollection<Person> people)
        {

            // その時点の People をディープコピー
            var copy = new ObservableCollection<Person>(
                people.Select(p => new Person { Name = p.Name, Age = p.Age, City = p.City })
            );

            foreach (var p in copy)
                Debug.WriteLine($"{p.Name}, {p.Age}, {p.City}");

            // SnapShotAction にラップして渡す
            var act = new SnapShotAction<Person>(
                people,
                new ConsCell<ObservableCollection<Person>>(copy, new ConsCell<ObservableCollection<Person>>(), index++)
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

        /// <summary>
        /// Undo/Redo 対象の共通インターフェース
        /// </summary>
        public interface IChangeAction
        {
            void Apply();
            void Revert();
        }
    }
}
