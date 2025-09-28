using AdapterInterface;
using System.Collections.ObjectModel;

namespace UndoTransaction_SnapShot.Patterns.汎用化用.Core
{
    public class AddRowAction<T> : IChangeAction
    {
        private ObservableCollection<T> _collection;
        private T _item;

        public AddRowAction(ObservableCollection<T> collection, T item)
        {
            _collection = collection;
            _item = item;
        }

        public void Apply()
        {
            _collection.Add(_item);
        }

        public void Revert()
        {
            _collection.Remove(_item);
        }
    }
}
