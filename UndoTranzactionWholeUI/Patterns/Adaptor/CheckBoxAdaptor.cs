using System.Windows.Controls;
using UndoTransaction_SnapShot.Patterns.AbstractSectiion;

namespace UndoTransaction_SnapShot.Patterns.Adaptor
{
    internal class CheckBoxAdaptor : IValueAccessor<bool>
    {
        private readonly CheckBox _checkBox;
        public CheckBoxAdaptor(CheckBox checkBox) => _checkBox = checkBox;

        public bool Value
        {
            get => _checkBox.IsChecked ?? false;
            set => _checkBox.IsChecked = value;
        }

    }
}
