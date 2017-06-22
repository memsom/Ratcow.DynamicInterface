using System;

namespace Ratcow.DynamicInterface.Tests
{
    public class Property_Single_Int32
    {
        [PropertyImplementation(Interface = typeof(IProperty_Single_Int32), Name = "Test")]
        public Int32 Test { get; set; }
    }
}
