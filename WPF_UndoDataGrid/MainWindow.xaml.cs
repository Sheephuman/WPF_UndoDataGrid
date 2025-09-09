using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using WPF_UndoDataGrid.classes;
using WPF_UndoDataGrid.classes.WPF_UndoDataGrid;



namespace WPF_UndoDataGrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // ---- Undo用の1レコード ----
        private readonly UndoManager _undoManager = new UndoManager();

        private readonly CellUndoManager _CellundoManager = new CellUndoManager();


        readonly IList<Person> _itemsorce;

        public MainWindow()
        {
            InitializeComponent();

            _itemsorce = People;

            datagrid1.CellEditEnding += Datagrid1_CellEditEnding;
        }

        private void Datagrid1_CellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction != DataGridEditAction.Commit)
                return;

            // 編集前後の値を取り出す
            var binding = (e.Column as DataGridBoundColumn)?.Binding as System.Windows.Data.Binding;
            if (binding == null) return;

            PropertyInfo? prop = e.Row.Item.GetType().GetProperty(binding.Path.Path);
            if (prop == null) return;

            var oldValue = prop.GetValue(e.Row.Item);

            // EditingElement から新しい値を取得
            string? newText = null;
            if (e.EditingElement is TextBox tb)
                newText = tb.Text;

            object? newValue = newText;
            if (prop.PropertyType != typeof(string) && newText != null)
            {
                // 型変換
                newValue = Convert.ChangeType(newText, prop.PropertyType);
            }

            // Undo/Redo用 Change を作成
            var change = new ChangeCell(
                new DataGridCellInfo(e.Row.Item, e.Column),
                oldValue,
                newValue
            );

            _CellundoManager.AddCellChange(change);
        }

        public ObservableCollection<Person> People { get; } = new()
        {
            new Person { Name = "Alice", Age = 28, City = "Tokyo" },
            new Person { Name = "Bob",   Age = 34, City = "Osaka" },
            new Person { Name = "Cathy", Age = 22, City = "Nagoya" },
        };



        private void AddRowClick(object sender, RoutedEventArgs e)
        {
            var newPerson = RandomPerson();

            People.Add(newPerson);

            datagrid1.ItemsSource = People;

            // Change を作る（行追加なので oldValue = null）
            var change = new ChangeGrid(
                new DataGridCellInfo(newPerson, datagrid1.Columns[0]),
                oldValue: null,
                newValue: newPerson,
               itemsorece: People // IList を渡す
            );


            _undoManager.AddChange(change);
        }

        Person RandomPerson()
        {
            var random = new Random();
            string[] prefectures =
   {
            "東京", "大阪", "福岡", "北海道", "京都",
            "愛知", "沖縄", "広島", "宮城", "長野"
        };
            // 名前候補（「ひつじ○○」）
            string[] nameSuffix = { "太郎", "花子", "次郎", "美咲", "健一", "真央", "翔", "未来", "一郎", "優子" };



            var people = new List<Person>();

            var person = new Person
            {
                Name = "ひつじ" + nameSuffix[random.Next(nameSuffix.Length)],
                Age = random.Next(20, 41), // 20～40の範囲
                City = prefectures[random.Next(prefectures.Length)]
            };


            return person;

        }


        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            _undoManager.Undo();
        }


        private void OnCellEditEnding(object sender, System.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {

        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            _undoManager.Redo();
        }

        private void CellUndoButton_Click(object sender, RoutedEventArgs e)
        {
            _CellundoManager.CellUndo();
        }

        private void CellRedoButton_Click(object sender, RoutedEventArgs e)
        {
            _CellundoManager.CellRedo();
        }
    }
}