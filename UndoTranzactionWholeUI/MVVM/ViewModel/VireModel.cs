using System.Collections.ObjectModel;
using UndoTransaction_SnapShot.MVVM.Model;

namespace UndoTransaction_SnapShot.ViewModel
{
    internal class DataGridViewModel : BindableBase
    {
        MainWindow _main;

        public DataGridViewModel(MainWindow main)
        {

            _main = main;


            _main.datagrid1.ItemsSource = new ObservableCollection<Person>
        {
            new Person { Name = "Alice sheep", Age = 20,City = "羊飼い市" },
            new Person { Name = "Bob Merey", Age = 30,City = "羊蹄山"},
        };
        }



    }
}
