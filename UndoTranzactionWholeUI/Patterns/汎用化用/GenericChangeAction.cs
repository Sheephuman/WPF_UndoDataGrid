using AdapterInterface;
using System.Diagnostics;

public class GenericChangeAction : IChangeAction
{
    private readonly Action _apply;
    private readonly Action _revert;
    private readonly Action _replace;
    private readonly string? _description;

    public GenericChangeAction(Action apply, Action replace, string? description = null)
    {
        _apply = apply;
        _replace = replace;
        _description = description;
    }

    public void Apply()
    {
        Debug.WriteLine(_description);
        _apply();
    }

    public void Replace()
    {
        _replace();
    }

    public void Revert() => _revert();

    public override string ToString() => _description ?? base.ToString();
}
