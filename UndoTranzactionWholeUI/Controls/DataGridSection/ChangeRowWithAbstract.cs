using AdapterInterface;
using System.Windows.Controls;
using UndoTransaction_SnapShot.Extensions;
using UndoTransaction_SnapShot.MVVM.Model;

namespace UndoTransaction_SnapShot
{
    public class ChangeRowWithAbstract : ChangeBase, IChangeAction
    {

        private readonly IList<Person> _itemsSource;
        public bool _hasMultiValue;
        public readonly List<Person> _deltaValue;

        public bool IsSnaped { get; }


        public ChangeRowWithAbstract(DataGridCellInfo cell, object? oldValue, object? newValue, IList<Person> itemsSource)
            : base(cell, oldValue, newValue, itemsSource)
        {
            _itemsSource = itemsSource;
        }


        public ChangeRowWithAbstract(DataGridCellInfo cell, object? oldValue, object? newValue, IList<Person> itemsSource, List<Person> deltaValue)
            : base(cell, oldValue, newValue, itemsSource)
        {
            _itemsSource = itemsSource;
            _deltaValue = deltaValue;
        }

        public ChangeRowWithAbstract(DataGridCellInfo cell, object? oldValue, object? newValue, IList<Person> itemsSource, List<Person> deltaValue, bool isSnaped) : this(cell, oldValue, newValue, itemsSource, deltaValue)
        {
            _itemsSource = itemsSource;
            _deltaValue = deltaValue;
            IsSnaped = isSnaped;
        }


        public void ApplyMultiValues()
        {
            if (_deltaValue.Count > 0)
                _itemsSource.AddColections(NewValue, _deltaValue);

        }

        public override void Apply()
        {
            if (NewValue is null) throw new ArgumentNullException(nameof(NewValue));

            //if (_hasMultiValue )
            //{
            //    _itemsSource.AddColections(NewValue, _deltaValue);
            //}
            //else
            _itemsSource.Add((Person)NewValue);
        }

        public override void Revert()
        {

            if (OldValue == null && NewValue != null)
            {
                if (_deltaValue.Count > 0)
                {
                    _itemsSource.RemoveColections(NewValue, _deltaValue);


                }

                else
                    _itemsSource.Remove((Person)NewValue);

            }
            else
            {
                SetCellValue(Cell, OldValue);

            }

        }


    }



}
