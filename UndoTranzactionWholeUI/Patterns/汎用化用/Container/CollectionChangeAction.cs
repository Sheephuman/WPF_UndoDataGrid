using AdapterInterface;

namespace UndoTransaction_SnapShot.Patterns.Generics.Container
{
    internal class CollectionChangeAction<T> : IChangeAction
    {

        private readonly IList<T> _collection;
        private readonly T _item;
        private readonly bool _isAdd; // true=追加, false=削除


        public CollectionChangeAction(IList<T> collection, T item, bool isAdd)
        {
            _collection = collection;
            _item = item;
            _isAdd = isAdd;
        }

        public void Apply()
        {
            if (_isAdd)
                _collection.Add(_item);
            else
                _collection.Remove(_item);
        }


        public void Revert()
        {
            if (_collection is null)

                //  throw new NullReferenceException(nameof(_collection)+ nameof(_collection) +  " is null");

                if (_isAdd)
                    _collection.Remove(_item);
                else
                    _collection.Add(_item);


        }


    }
}
