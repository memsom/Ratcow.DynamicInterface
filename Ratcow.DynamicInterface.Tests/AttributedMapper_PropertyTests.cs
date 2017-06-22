using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Ratcow.DynamicInterface.Tests
{

    [TestClass]
    public class AttributedMapper_PropertyTests : BaseTest
    {
        [TestMethod]
        public void AttributedMapper_PropertyTests_Property_Single_Int32_Type()
        {
            var testValue = 10;

            var engine = new AttributedMapper();

            var instance = new Property_Single_Int32
            {
                Test = testValue,
            };

            Assert.AreEqual<Int32>(instance.Test, testValue);

            Type resultant;

            resultant = engine.CreateType<IProperty_Single_Int32>(instance);

            VerifyType_Single_Instance(resultant, "Property_Single_Int32", typeof(Property_Single_Int32));
        }

        [TestMethod]
        public void AttributedMapper_PropertyTests_Property_Single_Int32_Instance()
        {
            var testValue = 10;
            var newTestValue = 5;

            var engine = new AttributedMapper();

            var instance = new Property_Single_Int32
            {
                Test = testValue,
            };

            Assert.AreEqual<Int32>(instance.Test, testValue, "Instance did not match testValue");

            IProperty_Single_Int32 resultant;

            resultant = engine.CreateInstance<IProperty_Single_Int32>(instance);

            VerifyType_Single_Instance(resultant.GetType(), "Property_Single_Int32", typeof(Property_Single_Int32));

            Assert.AreEqual<Int32>(resultant.Test, testValue, "Resultant did not match testValue");
            Assert.AreEqual<Int32>(resultant.Test, instance.Test, "Resultant did not match instance (testValue)");

            //adjust the value to check the property can be written to
            resultant.Test = newTestValue;

            Assert.AreEqual<Int32>(resultant.Test, newTestValue, "Resultant did not match testValue");
            Assert.AreEqual<Int32>(resultant.Test, instance.Test, "Resultant did not match instance (newTestValue)");
        }

        [TestMethod]
        public void AttributedMapper_PropertyTests_Property_Single_String_Type()
        {
            var testValue = "Hello, world";

            var engine = new AttributedMapper();

            var instance = new Property_Single_String
            {
                Test = testValue,
            };

            Assert.AreEqual<string>(instance.Test, testValue);

            Type resultant;

            resultant = engine.CreateType<IProperty_Single_String>(instance);

            VerifyType_Single_Instance(resultant, "Property_Single_String", typeof(Property_Single_String));
        }

        [TestMethod]
        public void AttributedMapper_PropertyTests_Property_Single_String_Instance()
        {
            var testValue = "Hello, world";
            var newTestValue = "Goodbye, moon";

            var engine = new AttributedMapper();

            var instance = new Property_Single_String
            {
                Test = testValue,
            };

            Assert.AreEqual<string>(instance.Test, testValue, "Instance did not match testValue");

            IProperty_Single_String resultant;

            resultant = engine.CreateInstance<IProperty_Single_String>(instance);

            VerifyType_Single_Instance(resultant.GetType(), "Property_Single_String", typeof(Property_Single_String));

            Assert.AreEqual<string>(resultant.Test, testValue, "Resultant did not match testValue");
            Assert.AreEqual<string>(resultant.Test, instance.Test, "Resultant did not match instance (testValue)");

            //adjust the value to check the property can be written to
            resultant.Test = newTestValue;

            Assert.AreEqual<string>(resultant.Test, newTestValue, "Resultant did not match newTestValue");
            Assert.AreEqual<string>(resultant.Test, instance.Test, "Resultant did not match instance (newTestValue)");
        }
    }
}
