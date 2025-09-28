using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using UndoTransaction_SnapShot.CompositeAction;
using UndoTransaction_SnapShot.Generics.Container;
using UndoTransaction_SnapShot.MVVM.Model;
using UndoTransaction_SnapShot.Patterns.汎用化用.Core;




namespace UndoTransaction_SnapShot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private UndoManager<IChangeAction> _undoManager = new();

        private ConsCell<ObservableCollection<Person>> _snapShotStack
    = new ConsCell<ObservableCollection<Person>>();



        public MainWindow()
        {
            InitializeComponent();
        }



        private void OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

        }

        Person _person;

        internal static ChangeTransaction<Person> _trans { get; set; }
        private void SnapSHotButton_Click(object sender, RoutedEventArgs e)
        {


            // その時点の People をディープコピー
            var copy = new ObservableCollection<Person>(
                People.Select(p => new Person { Name = p.Name, Age = p.Age, City = p.City })
            );

            // Stack に保存
            _snapShotStack = _snapShotStack.Push(copy);

        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {


            _undoManager.Undo();

            //_undoManager.PopToRedo(change);   // redoStack に積む
        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            _undoManager.Redo();
        }

        private void AddDataButton_Click(object sender, RoutedEventArgs e)
        {
            _person = PersonCreater.RandomPerson();

            // 1. Change を作成（まだ People に追加しない）
            var change = new ChangeRowWithAbstract(
                new DataGridCellInfo(_person, datagrid1.Columns[0]),
                oldValue: null,
                newValue: _person,
                itemsSource: People
            );

            // 2. UndoManager に積む
            _undoManager.AddChange(change);

            // 3. Apply で実際に追加
            change.Apply();

            // 4. DataGrid の ItemsSource 更新
            datagrid1.ItemsSource = People;
        }

        public ObservableCollection<Person> People { get; } = new()
        {
            new Person { Name = "Alice", Age = 28, City = "Tokyo" },
            new Person { Name = "Bob",   Age = 34, City = "Osaka" },
            new Person { Name = "Cathy", Age = 22, City = "Nagoya" },
        };

        private void UndoTransActionButton_Click(object sender, RoutedEventArgs e)
        {



            if (_snapShotStack.IsEmpty) return;

            // 最新スナップショットを取得
            ObservableCollection<Person> snap = _snapShotStack.Head;




            // DataGrid に書き戻す

            var currentStack = _snapShotStack;





            // SnapShotAction を生成して UndoManager に積む
            var act = new SnapShotAction<Person>(People, currentStack);
            _undoManager.AddChange(act);

            People.Clear();
            foreach (var p in snap)
            {
                People.Add(new Person { Name = p.Name, Age = p.Age, City = p.City });
            }

            // _undoManager.ConectUndo(currentStack, snap);
            // スタックを1つ戻す
            _snapShotStack = _snapShotStack.Tail;



        }
        private Person Clone(Person p)
        {
            if (p == null) throw new ArgumentNullException(nameof(p));

            return new Person
            {
                Name = p.Name,
                Age = p.Age,
                City = p.City
            };
        }
    }
}
