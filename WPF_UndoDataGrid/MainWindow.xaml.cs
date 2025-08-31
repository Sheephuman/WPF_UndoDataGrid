using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using WPF_UndoDataGrid.classes;



namespace WPF_UndoDataGrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // ---- Undo用の1レコード ----
        private readonly UndoManager _undoManager = new UndoManager();

        IList<Person> _itemsorce;

        public MainWindow()
        {
            InitializeComponent();

            _itemsorce = People;
        }


        public ObservableCollection<Person> People { get; } = new()
        {
            new Person { Name = "Alice", Age = 28, City = "Tokyo" },
            new Person { Name = "Bob",   Age = 34, City = "Osaka" },
            new Person { Name = "Cathy", Age = 22, City = "Nagoya" },
        };



        private void OnAddRowClick(object sender, RoutedEventArgs e)
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
    }
}