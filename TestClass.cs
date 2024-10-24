using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;

namespace BlazorMemoryInfo;

public partial class TestClass
{
    private static readonly ParentClass _parent = new();
    private static readonly HashSet<ChildClass> _objects = [];

    [JSExport]
    public static string AllocateObjects(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Console.WriteLine($"Allocating object {i}");
            var child = new ChildClass(_parent);
            _objects.Add(child);
        }

        return GC.GetTotalMemory(forceFullCollection: false).ToString();
    }

    [JSExport]
    public static void DisposeObjects()
    {
        foreach (var child in _objects)
        {
            child.Dispose();
        }
        // _objects.Clear();
    }
}
