namespace WPF_UndoDataGrid.classes
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Data;

    namespace WPF_UndoDataGrid
    {
        /// <summary>
        /// DataGrid に対する「セルの変更」を表現するクラス。
        /// 
        /// 【責務】
        /// - セルの編集（値の変更）を 1 単位の「変更」として保持する
        /// - 変更前の値 (OldValue) と変更後の値 (NewValue) を記録する
        /// - 対象セルを特定するための DataGridCellInfo を保持する
        /// - Revert() で変更を取り消す (Undo)
        /// - Apply() で変更を再実行する (Redo)
        /// 
        /// 【非責務】
        /// - Undo/Redo の履歴管理（これは UndoManager が担当）
        /// - DataGrid の UI 更新やバインディング制御
        /// - 行追加/削除（それは ChangeGrid が担当）
        /// </summary>
        public class ChangeCell
        {

            /// <summary>
            /// どのセルが変更対象かを表す情報
            /// </summary>
            public DataGridCellInfo Cell { get; }


            public object? OldValue { get; }
            public object? NewValue { get; }

            public ChangeCell(DataGridCellInfo cell, object? oldValue, object? newValue)
            {
                Cell = cell;
                OldValue = oldValue;
                NewValue = newValue;
            }

            /// <summary>
            /// セルに新しい値を適用 (Redo)
            /// </summary>
            public void Apply()
            {
                if (NewValue is null)
                    throw new ArgumentNullException(nameof(NewValue));

                SetCellValue(Cell, NewValue);
            }

            /// <summary>
            /// セルを元の値に戻す (Undo)
            /// </summary>
            public void Revert()
            {


                SetCellValue(Cell, OldValue);
            }

            /// <summary>
            /// DataGrid の指定セルに値を設定する処理。
            /// </summary>
            private void SetCellValue(DataGridCellInfo cellInfo, object? value)
            {
                if (cellInfo.Item == null || cellInfo.Column == null)
                    return;

                if (cellInfo.Column is DataGridBoundColumn boundColumn)
                {
                    if (boundColumn.Binding is Binding binding)
                    {
                        // cellInfo.Item の実際の型から、Binding で指定されたプロパティをリフレクションで取得する
                        var prop = cellInfo.Item.GetType().GetProperty(binding.Path.Path);
                        if (prop != null)
                        {
                            // 直接バインド先のプロパティへ値を代入
                            prop.SetValue(cellInfo.Item, value);
                        }

                    }
                }
            }
        }
    }

}
