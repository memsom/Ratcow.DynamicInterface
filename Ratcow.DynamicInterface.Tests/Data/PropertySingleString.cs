using Ratcow.DynamicInterface.Tests.Data.Interfaces;

namespace Ratcow.DynamicInterface.Tests.Data;

public class PropertySingleString
{
    [PropertyImplementation(Interface = typeof(IPropertySingleString), Name = nameof(IPropertySingleString.TestS))]
    public string? Test { get; set; }
}