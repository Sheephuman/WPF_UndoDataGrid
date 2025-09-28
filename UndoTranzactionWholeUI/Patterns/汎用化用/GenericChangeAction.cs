using AdapterInterface;

namespace UndoTransaction_SnapShot.Generics
{

    public class GenericChangeAction : IChangeAction
    {
        private readonly Action _apply;
        private readonly Action _revert;

        public GenericChangeAction(Action apply, Action revert)
        {
            _apply = apply;
            _revert = revert;
        }
        public void Apply() => _apply();
        public void Revert() => _revert();
    }



}
