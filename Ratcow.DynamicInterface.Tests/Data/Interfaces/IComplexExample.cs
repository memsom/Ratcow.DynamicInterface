namespace Ratcow.DynamicInterface.Tests.Data.Interfaces;

public interface IComplexExample
{
    bool HasOutParam(string input, string output);
    void NotImplemented();
}