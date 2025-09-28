namespace UndoTransaction_SnapShot.Patterns.AbstractSectiion
{
    interface IValueAccessor<T>
    {
        T Value { get; set; }
    }
}
