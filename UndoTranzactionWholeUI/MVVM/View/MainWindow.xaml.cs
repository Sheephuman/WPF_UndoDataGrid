using AdapterInterface;
using System.Collections.ObjectModel;
using System.Reflection;
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



        Person _person;

        bool isSnaped;

        private void SnapSHotButton_Click(object sender, RoutedEventArgs e)
        {


            if (changeRows != null)
                _undoManager.Snap(People, changeRows);


            if (SnapshotButton.IsChecked.HasValue)
                isSnaped = true;
            else
                isSnaped = false;
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

        List<Person> deltaValue = new();

        ChangeRowWithAbstract changeRows;
        GenericChangeAction genericChange;

        private void AddDataButton_Click(object sender, RoutedEventArgs e)
        {

            var newPerson = PersonCreater.RandomPerson();



            // 現在の状態をスナップショットとしてコピー
            var oldState = new ObservableCollection<Person>(
                People.Select(p => new Person { Name = p.Name, Age = p.Age, City = p.City })
            );

            // 変更後（追加後）の状態を準備

            var newState = new ObservableCollection<Person>(
                People.Concat(new[] { newPerson }).Select(p => new Person { Name = p.Name, Age = p.Age, City = p.City })
            );

            // Replace 関数をラムダ内で呼び出す
            var genericChange2 = new GenericChangeAction(
                apply: () => Replace(People, newState),
                replace: () => Replace(People, oldState),
                description: $"Add {newPerson.Name} snapshot"
            );


            // GenericChangeAction に直接ラムダで定義する
            var genericChange = new GenericChangeAction(
                apply: () => People.Add(newPerson),
                revert: () => People.Remove(newPerson),
                description: $"Add person: {newPerson.Name}"
            );

            // 2. UndoManager に積む
            _undoManager.AddChange(genericChange);

            genericChange.Apply();
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

        }

        private void CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            if (e.EditAction != DataGridEditAction.Commit)
                return;

            // DataGrid のバインディング情報を取得
            var binding = (e.Column as DataGridBoundColumn)?.Binding as System.Windows.Data.Binding;
            if (binding == null) return;

            var person = (Person)e.Row.Item;
            string property = binding.Path.Path; // ← これが property

            // PropertyInfo を取得
            PropertyInfo? prop = person.GetType().GetProperty(property);
            if (prop == null) return;

            var oldValue = prop.GetValue(person);

            // EditingElement から新しい値を取得
            string? newText = (e.EditingElement as TextBox)?.Text;
            if (newText == null) return;

            object? newValue = newText;
            if (prop.PropertyType != typeof(string))
            {
                // 型変換
                newValue = Convert.ChangeType(newText, prop.PropertyType);
            }

            // GenericChangeAction で Undo/Redo 操作を登録
            var genericChange = new GenericChangeAction(
                apply: () => prop.SetValue(person, newValue),
                revert: () => prop.SetValue(person, oldValue),
                description: $"Cell edit: {property} from {oldValue} to {newValue}"
            );

            // UndoManager に登録
            _undoManager.AddChange(genericChange);
        }
    }
}
