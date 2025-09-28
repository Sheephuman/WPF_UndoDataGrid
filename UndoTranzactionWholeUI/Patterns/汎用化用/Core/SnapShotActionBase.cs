using System.Collections.ObjectModel;
using UndoTransaction_SnapShot.Generics.Container;

namespace UndoTransaction_SnapShot.Patterns.汎用化用.Core
{
    public class SnapShotActionBase<T>
    {
        private readonly ConsCell<ObservableCollection<T>> _snapshot;
    }
}