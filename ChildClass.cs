using System;
using System.ComponentModel;

namespace BlazorMemoryInfo;

public sealed class ChildClass : IDisposable
{
    private readonly ParentClass _parent;
    private readonly byte[] _junk = new byte[250_000];

    public ChildClass(ParentClass parent)
    {
        _parent = parent;
        _parent.PropertyChanged += OnPropertyChanged;
    }

    public void Dispose()
    {
        _parent.PropertyChanged -= OnPropertyChanged;
        GC.SuppressFinalize(this);
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
    }
}
