using System.Windows.Controls;

namespace WPF_UndoDataGrid.classes
{
    internal class ChangeRowWithAbstract : ChangeBase
    {

        private readonly IList<Person> _itemsSource;

        public ChangeRowWithAbstract(DataGridCellInfo cell, object? oldValue, object? newValue, IList<Person> itemsSource)
            : base(cell, oldValue, newValue, itemsSource)
        {
            _itemsSource = itemsSource;
        }

        public override void Apply()
        {
            if (NewValue is null) throw new ArgumentNullException(nameof(NewValue));
            _itemsSource.Add((Person)NewValue);
        }

        public override void Revert()
        {
            if (OldValue == null && NewValue != null)
            {
                _itemsSource.Remove((Person)NewValue);
            }
            else
            {
                SetCellValue(Cell, OldValue);
            }
        }
    }
}
