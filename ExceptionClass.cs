using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;

namespace BlazorMemoryInfo;

public partial class ExceptionClass
{
    private static readonly HashSet<object> _objects = [];

    static ExceptionClass()
    {
        var largeArray = new byte[int.MaxValue];
        _objects.Add(largeArray);
    }

    [JSExport]
    public static void EmptyMethod()
    {
    }
}
