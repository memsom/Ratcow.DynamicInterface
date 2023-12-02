using Ratcow.DynamicInterface.Tests.Data.Interfaces;

namespace Ratcow.DynamicInterface.Tests.Data;

public class ComplexExample
{
    //[MethodImplementation(Interface = typeof(IComplexExample), Name = nameof(IComplexExample.HasOutParam))]
    public bool HasOutParam(string input, string output)
    {
        return true;
    }

    public void NotImplemented()
    {
    }
}