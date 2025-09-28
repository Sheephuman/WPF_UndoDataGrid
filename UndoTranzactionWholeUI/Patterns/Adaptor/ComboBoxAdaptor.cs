using System.Windows.Controls;
using UndoTransaction_SnapShot.Patterns.AbstractSectiion;

namespace UndoTransaction_SnapShot.Patterns.Adaptor
{
    internal class ComboBoxAdaptor : IValueAccessor<object?>
    {

        private readonly ComboBox _comboBox;
        public ComboBoxAdaptor(ComboBox comboBox) => _comboBox = comboBox;

        public object? Value
        {
            get => _comboBox.SelectedItem;
            set => _comboBox.SelectedItem = value;
        }
    }
}
