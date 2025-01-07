namespace Ratcow.DynamicInterface.Tests.Data;

public class ComplexExample
{
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

    [MethodImplementation(Interface = typeof(IComplexExample), Name = nameof(IComplexExample.Simple1Out))]
    public void Simple1Out(out int one)
    {
        one = 3;
        System.Diagnostics.Debug.WriteLine($"Called Simple1(out {one})");
    }

    [MethodImplementation(Interface = typeof(IComplexExample), Name = nameof(IComplexExample.Simple0Return))]
    public int Simple0Return()
    {
        System.Diagnostics.Debug.WriteLine("Called Simple0Return");
        return 1;
    }

    [MethodImplementation(Interface = typeof(IComplexExample), Name = nameof(IComplexExample.Simple1Return))]
    public int Simple1Return(int one)
    {
        System.Diagnostics.Debug.WriteLine($"Called Simple1Return({one})");

        return one;
    }
}