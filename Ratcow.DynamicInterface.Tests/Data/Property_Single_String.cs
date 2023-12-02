using Ratcow.DynamicInterface.Tests.Data.Interfaces;

namespace Ratcow.DynamicInterface.Tests.Data;

public class Property_Single_String
{
    [PropertyImplementation(Interface = typeof(IProperty_Single_String), Name = "TestS")]
    public string Test { get; set; }
}