using System.Windows.Controls;

namespace WPF_UndoDataGrid.classes
{
    internal class ChangeCellwithAbstract : ChangeBase
    {

        readonly IList<Person> _itemsorce;

        public ChangeCellwithAbstract(DataGridCellInfo cell, object? oldValue, object? newValue, IList<Person> itemsorce)
     : base(cell, oldValue, newValue, itemsorce)
        {

            _itemsorce = itemsorce;

        }

        public override void Apply()
        {
            if (NewValue is null) throw new ArgumentNullException(nameof(NewValue));
            SetCellValue(Cell, NewValue);
        }

        public override void Revert()
        {
            SetCellValue(Cell, OldValue);
        }
    }
}
