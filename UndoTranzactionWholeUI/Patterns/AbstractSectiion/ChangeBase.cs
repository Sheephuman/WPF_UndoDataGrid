using AdapterInterface;
using System.Windows.Controls;
using System.Windows.Data;
using UndoTransaction_SnapShot.MVVM.Model;

namespace UndoTransaction_SnapShot
{
    /// <summary>
    /// DataGrid に対する「変更」の抽象基底クラス。
    /// セル編集・行追加削除を共通の枠組みで扱う。
    /// </summary>
    public abstract class ChangeBase : IChangeAction
    {
        public DataGridCellInfo Cell { get; }
        public object? OldValue { get; }
        public object? NewValue { get; }

        readonly IList<Person> _itemsorce;


        protected ChangeBase(DataGridCellInfo cell, object? oldValue, object? newValue, IList<Person> itemSorce
          )
        {
            Cell = cell;
            OldValue = oldValue;
            NewValue = newValue;
            _itemsorce = itemSorce;



        }

        public abstract void Apply();



        public abstract void Revert();

        /// <summary>
        /// 共通の「セルに値を設定する」処理。
        /// </summary>
        protected void SetCellValue(DataGridCellInfo cellInfo, object? value)
        {
            if (cellInfo.Item == null || cellInfo.Column == null)
                return;

            if (cellInfo.Column is DataGridBoundColumn boundColumn
                && boundColumn.Binding is Binding binding)
            {
                var prop = cellInfo.Item.GetType().GetProperty(binding.Path.Path);
                if (prop != null)
                {
                    prop.SetValue(cellInfo.Item, value);
                }
            }
        }
    }

}
