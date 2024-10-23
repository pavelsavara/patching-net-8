using System;
using System.ComponentModel;

namespace BlazorMemoryInfo;

public sealed class ParentClass
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public void NotifyChilderen()
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Foo"));
    }
}
