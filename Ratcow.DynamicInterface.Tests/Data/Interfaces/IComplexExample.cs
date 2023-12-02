namespace Ratcow.DynamicInterface.Tests.Data.Interfaces;

public interface IComplexExample
{
    void Simple0();
    void Simple1(int one);
    void Simple1Out(out int one);
    int Simple0Return();
    int Simple1Return(int one);
}