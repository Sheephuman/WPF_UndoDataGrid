/// <summary>
/// 再帰的に定義された「連結リスト（Consリスト）」を表現するクラス。
/// 
/// 【責務】
/// - 先頭要素 (Head) と残りのリスト (Tail) を持つ単方向リストの基本構造
/// - 空リスト（終端セル）と要素を持つセルを区別する
/// - 要素の追加 (Push) や連結 (Concat) といった操作を提供する
///
/// 【特徴】
/// - イミュータブル設計（既存セルを変更せず、新しいセルを作成する）
/// - 空リスト判定 (IsEmpty) が可能
/// - Stack / UndoRedo の履歴構造などに利用しやすい
///
/// 【非責務】
/// - 標準コレクションの変更操作（Add, Clear, Remove は未対応）
/// </summary>
public class ConsCell<T> : ICollection<T>
{
    private readonly T? head;
    private readonly ConsCell<T>? tail;
    private readonly bool isTerminal;

    /// <summary>
    /// 空リスト（終端セル）を作成する
    /// </summary>
    public ConsCell()
    {
        this.isTerminal = true;
    }

    /// <summary>
    /// 値と次のセルを指定して新しい ConsCell を作成する
    /// </summary>
    public ConsCell(T value, ConsCell<T> tail)
    {
        this.head = value;
        this.tail = tail;
    }

    /// <summary>
    /// IEnumerable から ConsCell を構築する
    /// </summary>
    public ConsCell(IEnumerable<T> source)
        : this(EnsureNotNull(source).GetEnumerator())
    {
    }

    private static IEnumerable<T> EnsureNotNull(IEnumerable<T> source)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        return source;
    }

    private ConsCell(IEnumerator<T> itor)
    {
        if (itor.MoveNext())
        {
            this.head = itor.Current;
            this.tail = new ConsCell<T>(itor);
        }
        else
        {
            this.isTerminal = true;
        }
    }

    /// <summary>
    /// 空リストかどうかを判定
    /// </summary>
    public bool IsEmpty => this.isTerminal;

    /// <summary>
    /// 先頭要素を取得（空リストなら例外）
    /// </summary>
    public T Head
    {
        get
        {
            ErrorIfEmpty();
            if (head is null) throw new ArgumentNullException(nameof(head));
            return this.head;
        }
    }

    /// <summary>
    /// 残りのリストを取得（空リストなら例外）
    /// </summary>
    public ConsCell<T> Tail
    {
        get
        {
            ErrorIfEmpty();
            return this.tail!;
        }
    }

    private void ErrorIfEmpty()
    {
        if (this.isTerminal)
            throw new InvalidOperationException("this is empty.");
    }

    /// <summary>
    /// 新しい要素を先頭に追加し、新しい ConsCell を返す
    /// </summary>
    public ConsCell<T> Push(T head) => new ConsCell<T>(head, this);

    /// <summary>
    /// このリストの末尾に別のリストを連結する
    /// </summary>
    public ConsCell<T> Concat(ConsCell<T> second)
    {
        if (this.isTerminal || head is null)
            return second;
        return this.tail!.Concat(second).Push(this.head);
    }

    /// <summary>
    /// 要素を含むかどうか
    /// </summary>
    public bool Contains(T item)
    {
        for (ConsCell<T> p = this; !p.isTerminal; p = p.tail!)
        {
            if (p.head == null && item == null) return true;
            if (p.head != null && p.head.Equals(item)) return true;
        }
        return false;
    }

    /// <summary>
    /// 要素数を返す
    /// </summary>
    public int Count
    {
        get
        {
            int c = 0;
            for (ConsCell<T> p = this; !p.isTerminal; p = p.tail!)
            {
                c++;
            }
            return c;
        }
    }


    /// <summary>
    /// foreach に対応
    /// </summary>
    public IEnumerator<T> GetEnumerator()
    {
        for (ConsCell<T> p = this; !p.isTerminal; p = p.tail!)
        {
            if (p.head is null) throw new ArgumentException();
            yield return p.head;
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.GetEnumerator();


    public const string Player = nameof(Player);

    #region ICollection<T> 実装（読み取り専用）

    bool ICollection<T>.IsReadOnly => true;

    void ICollection<T>.CopyTo(T[] array, int arrayIndex)
    {
        for (ConsCell<T> p = this; !p.isTerminal; p = p.tail!)
        {
            if (array.Length <= arrayIndex)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));

            if (p.head is null) throw new ArgumentException();
            array[arrayIndex++] = p.head;
        }
    }

    void ICollection<T>.Add(T item) => throw new NotSupportedException();
    void ICollection<T>.Clear() => throw new NotSupportedException();
    bool ICollection<T>.Remove(T item) => throw new NotSupportedException();

    #endregion
}
