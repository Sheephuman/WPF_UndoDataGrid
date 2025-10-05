using AdapterInterface;
using System.Text;

namespace UndoTransaction_SnapShot.Generics.Container
{

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
    /// </summary>
    public class ConsCell<T> : ICollection<T>
    {
        private readonly T? head;
        private readonly ConsCell<T>? tail;
        public readonly bool isTerminal;

        /// <summary>
        /// 空リスト（終端セル）を作成する
        /// </summary>
        public ConsCell()
        {
            this.isTerminal = true;
        }


        public bool _hasMultiValue { get; }


        /// <summary>
        /// 値と次のセルを指定して新しい ConsCell を作成する
        /// </summary>
        public ConsCell(T value, ConsCell<T> tail)
        {
            this.head = value;
            this.tail = tail;
        }

        public ConsCell(T value, ConsCell<T> tail, bool hasMultiValue)
        {
            this.head = value;
            this.tail = tail;
            _hasMultiValue = hasMultiValue;
        }
        public ConsCell(T value, ConsCell<T> tail, ChangeRowWithAbstract changeRows)
        {
            this.head = value;
            this.tail = tail;


        }


        /// <summary>
        /// IEnumerable から ConsCell を構築する
        /// </summary>



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
                if (head is null)
                    throw new ArgumentNullException(nameof(head));
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
        public ConsCell<T> Push(T head) => new ConsCell<T>(head, this, false);

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
            set
            {
                value = Count;

            }

        }




        public T this[int index]
        {
            get
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index));

                ConsCell<T> current = this;

                for (int i = 0; i < index; i++)
                {
                    if (current.IsEmpty)
                        throw new ArgumentOutOfRangeException(nameof(index), "Index exceeds list length.");
                    current = current.Tail;
                }

                if (current.IsEmpty)
                    throw new ArgumentOutOfRangeException(nameof(index), "Index exceeds list length.");

                return current.Head;
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

        public int fromCounter()
        {

            var sb = new StringBuilder();
            int idx = 0;
            foreach (var item in this)
            {
                sb.AppendLine($"[{idx++}] {item}");
            }
            return idx;
        }

        public override string ToString()
        {
            if (IsEmpty) return "Empty";

            // デバッグ用に Index を付けて出力
            var sb = new StringBuilder();
            int idx = 0;
            foreach (var item in this)
            {
                sb.AppendLine($"[{idx++}] {item}");
            }
            return sb.ToString();
        }


        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.GetEnumerator();


        public const string Player = nameof(Player);

        #region ICollection<T> 実装（読み取り専用）


        public bool IsReadOnly => throw new NotImplementedException();






        public void Add(IChangeAction item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void CopyTo(IChangeAction[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(IChangeAction item)
        {
            throw new NotImplementedException();
        }



        public void Add(T item)
        {
            throw new NotImplementedException();
        }


        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            throw new NotImplementedException();
        }






        #endregion
    }

}