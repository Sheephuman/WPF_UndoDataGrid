using System.Windows.Controls;
using UndoTransaction_SnapShot.Patterns.AbstractSectiion;

namespace UndoTransaction_SnapShot.Patterns.Adaptor
{
    internal class TextBoxAdaptor : IValueAccessor<string>
    {
        private readonly TextBox _textBox;
        public TextBoxAdaptor(TextBox textBox) => _textBox = textBox;


        /// <summary>
        /// interface具象化部分
        /// </summary>
        public string Value
        {
            get => _textBox.Text;
            set => _textBox.Text = value;
        }

    }
}
