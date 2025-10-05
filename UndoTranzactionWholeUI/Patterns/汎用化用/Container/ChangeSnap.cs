using AdapterInterface;
using UndoTransaction_SnapShot.Patterns.AbstractSectiion;

namespace UndoTransaction_SnapShot.Generics
{
    internal class ChangeSnap<T> : IChangeAction
    {
        private readonly IValueAccessor<T> _control;
        private readonly T _oldValue;
        private readonly T _newValue;

        public ChangeSnap(IValueAccessor<T> control, T oldValue, T newValue)
        {
            _control = control;
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public void Apply() => _control.Value = _newValue;
        public void Revert()
        {
            _control.Value = _oldValue;

        }
    }
}
