using AdapterInterface;

namespace UndoTransaction_SnapShot.CompositeAction
{
    internal class CompositeTransaction<T> : IChangeAction
    {

        private readonly List<IChangeAction> _actions = new();


        public void Add(IChangeAction action) => _actions.Add(action);

        public void Apply()
        {
            foreach (var action in _actions.AsEnumerable().Reverse())
                action.Apply();
        }

        public CompositeTransaction(IEnumerable<IChangeAction> actions)
        {
            _actions.AddRange(actions);
        }


        public void Revert()
        {
            // Revert は逆順にしないと不整合が出る場合がある            
            foreach (var action in _actions.AsEnumerable().Reverse())
            {
                action.Revert();
            }

        }


    }
}
