namespace Ratcow.DynamicInterface.Tests
{
    public class Property_Single_String
    {
        [PropertyImplementation(Interface = typeof(IProperty_Single_String), Name = "TestS")]
        public string Test { get; set; }
    }
}
