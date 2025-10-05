using AdapterInterface;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using UndoTransaction_SnapShot.Generics.Container;
using UndoTransaction_SnapShot.MVVM.Model;




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

        private void Datagrid1_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

        }

        Person _person;

        bool isSnaped;

        private void SnapSHotButton_Click(object sender, RoutedEventArgs e)
        {
            if (changeRows != null)
                _undoManager.Snap(People, changeRows);


            if (SnapshotButton.IsChecked.HasValue)
                isSnaped = true;



        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            if (isSnaped)
                _undoManager.Undo(changeRows);
            else
                _undoManager.Undo();

            //_undoManager.PopToRedo(change);   // redoStack に積む
        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {


            if (isSnaped)
                _undoManager.Redo(changeRows);
            else
                _undoManager.Redo();



        }

        List<Person> deltaValue = new();

        ChangeRowWithAbstract changeRows;

        private void AddDataButton_Click(object sender, RoutedEventArgs e)
        {



            _person = PersonCreater.RandomPerson();

            bool isHasMultiValue = false;
            changeRows = new ChangeRowWithAbstract(
           new DataGridCellInfo(_person, datagrid1.Columns[0]),
           oldValue: null,
           newValue: _person,
           itemsSource: People,
           deltaValue,
           isHasMultiValue
       );


            if (!isSnaped)
            {
                isHasMultiValue = changeRows._hasMultiValue;
                deltaValue.Clear();
            }
            else
                deltaValue.Add(_person);


















            // 2. UndoManager に積む
            _undoManager.AddChange(changeRows);

            // 3. Apply で実際に追加
            changeRows.Apply();

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
            _undoManager.Undo();
        }



    }
}
