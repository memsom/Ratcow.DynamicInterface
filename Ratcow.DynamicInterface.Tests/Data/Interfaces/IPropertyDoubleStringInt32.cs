namespace Ratcow.DynamicInterface.Tests.Data.Interfaces;

/// <summary>
/// This interface adds nothing, but is testing composition of
/// multiple instances using the code from previous single tests.
/// </summary>
public interface IPropertyDoubleStringInt32: IPropertySingleString, IPropertySingleInt32
{
}