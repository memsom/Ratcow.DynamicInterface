namespace Ratcow.DynamicInterface.Tests.Data;

public class PropertySingleInt32
{
    [PropertyImplementation(Interface = typeof(IPropertySingleInt32), Name = nameof(IPropertySingleInt32.TestI32))]
    public Int32? Test { get; set; }
}