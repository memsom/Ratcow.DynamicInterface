namespace Ratcow.DynamicInterface.Tests.Data;

public class ComplexExampleEx: IComplexExampleEx
{
    public void NotIncluded() { System.Diagnostics.Debug.WriteLine("Called NotIncluded()");}
    
    // is we do not replace any of the implementation below, we will hit a NotImplementedException - this is intentional
    // because we want to prove we have detoured all of the methods.
    
    public void Simple0()
    {
        throw new NotImplementedException();
    }

    public void Simple1(int one)
    {
        throw new NotImplementedException();
    }

    public void Simple1Out(out int one)
    {
        throw new NotImplementedException();
    }

    public int Simple0Return()
    {
        throw new NotImplementedException();
    }

    public int Simple1Return(int one)
    {
        throw new NotImplementedException();
    }
}