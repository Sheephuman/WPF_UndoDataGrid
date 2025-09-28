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
    public class ConsCell<IChangeAction> : ICollection<IChangeAction>
    {
        private readonly IChangeAction? head;
        private readonly ConsCell<IChangeAction>? tail;
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
        public ConsCell(IChangeAction value, ConsCell<IChangeAction> tail)
        {
            this.head = value;
            this.tail = tail;
        }

        /// <summary>
        /// IEnumerable から ConsCell を構築する
        /// </summary>



        private ConsCell(IEnumerator<IChangeAction> itor)
        {
            if (itor.MoveNext())
            {
                this.head = itor.Current;
                this.tail = new ConsCell<IChangeAction>(itor);
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
        public IChangeAction Head
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
        public ConsCell<IChangeAction> Tail
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
        public ConsCell<IChangeAction> Push(IChangeAction head) => new ConsCell<IChangeAction>(head, this);

        internal ConsCell<IChangeAction> CompositePush(
    IChangeAction head,
    ConsCell<IChangeAction> tail
) => new ConsCell<IChangeAction>(head, tail);


        /// <summary>
        /// このリストの末尾に別のリストを連結する
        /// </summary>
        public ConsCell<IChangeAction> Concat(ConsCell<IChangeAction> second)
        {
            if (this.isTerminal || head is null)
                return second;
            return this.tail!.Concat(second).Push(this.head);
        }

        /// <summary>
        /// 要素を含むかどうか
        /// </summary>
        public bool Contains(IChangeAction item)
        {
            for (ConsCell<IChangeAction> p = this; !p.isTerminal; p = p.tail!)
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
                for (ConsCell<IChangeAction> p = this; !p.isTerminal; p = p.tail!)
                {
                    c++;
                }
                return c;
            }
        }


        /// <summary>
        /// foreach に対応
        /// </summary>
        public IEnumerator<IChangeAction> GetEnumerator()
        {
            for (ConsCell<IChangeAction> p = this; !p.isTerminal; p = p.tail!)
            {
                if (p.head is null) throw new ArgumentException();
                yield return p.head;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.GetEnumerator();


        public const string Player = nameof(Player);

        #region ICollection<T> 実装（読み取り専用）

        bool ICollection<IChangeAction>.IsReadOnly => true;

        public bool IsReadOnly => throw new NotImplementedException();

        void ICollection<IChangeAction>.CopyTo(IChangeAction[] array, int arrayIndex)
        {
            for (ConsCell<IChangeAction> p = this; !p.isTerminal; p = p.tail!)
            {
                if (array.Length <= arrayIndex)
                    throw new ArgumentOutOfRangeException(nameof(arrayIndex));

                if (p.head is null) throw new ArgumentException();
                array[arrayIndex++] = p.head;
            }
        }

        void ICollection<IChangeAction>.Add(IChangeAction item) => throw new NotSupportedException();
        void ICollection<IChangeAction>.Clear() => throw new NotSupportedException();
        bool ICollection<IChangeAction>.Remove(IChangeAction item) => throw new NotSupportedException();

        public void Apply()
        {
            throw new NotImplementedException();
        }

        public void Revert()
        {
            throw new NotImplementedException();
        }

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

        IEnumerator<IChangeAction> IEnumerable<IChangeAction>.GetEnumerator()
        {
            throw new NotImplementedException();
        }



        #endregion
    }
}