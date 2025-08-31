using System.ComponentModel;

namespace WPF_UndoDataGrid
{
    public class Person : INotifyPropertyChanged
    {
        private string _name = "";
        public string Name { get => _name; set { if (_name != value) { _name = value; OnPropertyChanged(nameof(Name)); } } }

        private int _age;
        public int Age { get => _age; set { if (_age != value) { _age = value; OnPropertyChanged(nameof(Age)); } } }

        private string _city = "";
        public string City { get => _city; set { if (_city != value) { _city = value; OnPropertyChanged(nameof(City)); } } }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
