using Ratcow.DynamicInterface.Tests.Data.Interfaces;

namespace Ratcow.DynamicInterface.Tests.Data;

public class Property_Single_Int32
{
    [PropertyImplementation(Interface = typeof(IProperty_Single_Int32), Name = "TestI32")]
    public Int32 Test { get; set; }
}