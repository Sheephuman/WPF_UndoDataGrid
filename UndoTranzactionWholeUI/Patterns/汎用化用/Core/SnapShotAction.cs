using AdapterInterface;
using System.Collections.ObjectModel;
using UndoTransaction_SnapShot.Generics.Container;
using UndoTransaction_SnapShot.MVVM.Model;

namespace UndoTransaction_SnapShot.Patterns.汎用化用.Core
{
    public class SnapShotAction<T> : IChangeAction
    {
        private readonly ConsCell<ObservableCollection<T>> _snapShot;
        private readonly ObservableCollection<T> _target;



        public SnapShotAction(ObservableCollection<T> target, ConsCell<ObservableCollection<T>> snapShot)
        {
            _target = target;
            _snapShot = snapShot;
        }

        public void Apply()
        {
            Restore(_snapShot.Head);
        }
        public void Revert()
        {

            var prev = _snapShot.Tail;
            // ConsCell なら Previous にたどって戻せる
            if (_snapShot.Tail != null && !prev.IsEmpty)
                Restore(_snapShot.Tail.Head);
        }

        private void Restore(ObservableCollection<T> snapshot)
        {

            _target.Clear();
            foreach (var item in snapshot)
                _target.Add(Clone(item));
        }
        private T Clone(T item)
        {
            return (T)(object)new Person
            {
                Name = ((Person)(object)item).Name,
                Age = ((Person)(object)item).Age,
                City = ((Person)(object)item).City
            };
        }



    }
}
