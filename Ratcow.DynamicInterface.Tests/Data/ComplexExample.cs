using Ratcow.DynamicInterface.Tests.Data.Interfaces;

namespace Ratcow.DynamicInterface.Tests.Data;

public class ComplexExample
{
    //[MethodImplementation(Interface = typeof(IComplexExample), Name = nameof(IComplexExample.HasOutParam))]
    public bool HasOutParam(string input, string output)
    {
        return true;
    }

    [MethodImplementation(Interface = typeof(IComplexExample), Name = nameof(IComplexExample.Simple0))]
    public void Simple0()
    {
        System.Diagnostics.Debug.WriteLine("Called Simple0");
    }

    [MethodImplementation(Interface = typeof(IComplexExample), Name = nameof(IComplexExample.Simple1))]
    public void Simple1(int one)
    {
        System.Diagnostics.Debug.WriteLine($"Called Simple1({one})");
    }
}