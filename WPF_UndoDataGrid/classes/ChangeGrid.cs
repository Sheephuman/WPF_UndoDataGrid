using System.Windows.Controls;
using WPF_UndoDataGrid;
using WPF_UndoDataGrid.classes;



/// <summary>
/// DataGrid に対する「1回の変更」を表現するクラス。
/// 
/// 【責務】
/// - セルの変更・行の追加削除などを 1 単位の「変更」として保持する
/// - 変更前の値 (OldValue) と変更後の値 (NewValue) を記録する
/// - 対象セルを特定するための DataGridCellInfo を保持する
/// - ItemsSource (IList<Person>) への参照を持ち、行追加/削除の Undo に対応する
/// - Revert() で変更を取り消す (Undo)
/// - Apply() で変更を再実行する (Redo)
///
/// 【非責務】
/// - Undo/Redo の履歴管理（これは UndoManager が担当する）
/// - DataGrid の UI 更新やバインディング制御
/// - 複数変更のまとめ（トランザクション管理）
/// </summary>
public class ChangeGrid : IChangeAction
{
    public DataGridCellInfo Cell { get; }
    public object? OldValue { get; }
    public object? NewValue { get; }


    public IList<Person> _itemsSource { get; set; }




    public ChangeGrid(DataGridCellInfo cell, object? oldValue, object? newValue, IList<Person> itemsorece)
    {
        Cell = cell;
        OldValue = oldValue;
        NewValue = newValue;

        _itemsSource = itemsorece;
    }


    /// <summary>
    ///  セルに新しい値を適用
    ///発火：Redo
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public void Apply()
    {
        if (NewValue is null) throw new ArgumentNullException(nameof(NewValue));
        SetCellValue(Cell, (Person)NewValue);
    }

    // セルを元の値に戻す
    public void Revert()
    {
        if (OldValue == null)
        {
            if (NewValue is null)
                throw new Exception("NewValue is null");
            _itemsSource.Remove((Person)NewValue); // Undo 行追加
        }
        else
        {
            SetCellValue(Cell, OldValue);
        }
    }



    /// <summary>
    /// DataGrid の指定セルに値を設定する処理。
    /// 
    /// 【処理の流れ】
    /// 1. cellInfo から対象アイテム (Item) と列 (Column) を取得。
    /// 2. 列が DataGridBoundColumn であれば、その Binding 情報を取り出す。
    /// 3. Binding.Path から対象プロパティをリフレクションで特定。
    /// 4. 本来であれば、そのプロパティに value をセットする想定。
    ///
    /// 【注意点】
    /// - 現状の実装では prop.SetValue(...) がコメントアウトされており、
    ///   代わりに _itemsSource に (Person)value を直接追加している。
    /// - そのため「セル編集」ではなく「行の追加」となり、Undo/Redo 設計と噛み合わない可能性がある。
    /// - また value を Person 型にキャストしているため、string や int など
    ///   セルの型と合わない値を渡すと ArgumentException が発生する。
    ///
    /// 【改善ポイント】
    /// - 本来は prop.SetValue(cellInfo.Item, value) を利用して
    ///   DataGrid バインド先のプロパティに値を代入するのが正しい。
    /// - _itemsSource.Add(...) は「行追加」専用の処理に分離すべき。
    /// </summary>

    public void SetCellValue(DataGridCellInfo cellInfo, object? value)
    {
        if (cellInfo.Item == null || cellInfo.Column == null)
            return;

        if (cellInfo.Column is DataGridBoundColumn boundColumn)
        {
            var binding = boundColumn.Binding as System.Windows.Data.Binding;
            if (binding == null) return;

            var prop = cellInfo.Item.GetType().GetProperty(binding.Path.Path);
            if (prop == null) return;

            if (value is null)
                throw new Exception("Value is null");


            _itemsSource.Add((Person)value);

        }
    }
}
