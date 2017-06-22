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

            Assert.AreEqual<Int32>(resultant.TestI32, testValue, "Resultant did not match testValue");
            Assert.AreEqual<Int32>(resultant.TestI32, instance.Test, "Resultant did not match instance (testValue)");

            //adjust the value to check the property can be written to
            resultant.TestI32 = newTestValue;

            Assert.AreEqual<Int32>(resultant.TestI32, newTestValue, "Resultant did not match testValue");
            Assert.AreEqual<Int32>(resultant.TestI32, instance.Test, "Resultant did not match instance (newTestValue)");
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

            Assert.AreEqual<string>(resultant.TestS, testValue, "Resultant did not match testValue");
            Assert.AreEqual<string>(resultant.TestS, instance.Test, "Resultant did not match instance (testValue)");

            //adjust the value to check the property can be written to
            resultant.TestS = newTestValue;

            Assert.AreEqual<string>(resultant.TestS, newTestValue, "Resultant did not match newTestValue");
            Assert.AreEqual<string>(resultant.TestS, instance.Test, "Resultant did not match instance (newTestValue)");
        }

        [TestMethod]
        public void AttributedMapper_PropertyTests_Property_Double_StringInt32_Type()
        {
            var testValueS = "Hello, world";
            var testValueI32 = 10;

            var engine = new AttributedMapper();

            var instanceS = new Property_Single_String
            {
                Test = testValueS,
            };

            var instanceI32 = new Property_Single_Int32
            {
                Test = testValueI32,
            };

            Assert.AreEqual<string>(instanceS.Test, testValueS);
            Assert.AreEqual<Int32>(instanceI32.Test, testValueI32);

            Type resultant;

            resultant = engine.CreateType<IProperty_Double_StringInt32>(instanceS, instanceI32);

            VerifyType_Double_Instance(resultant, "Property_Double_StringInt32", typeof(Property_Single_String), typeof(Property_Single_Int32));
        }

        [TestMethod]
        public void AttributedMapper_PropertyTests_Property_Double_StringInt32_Instance()
        {
            var testValueS = "Hello, world";
            var testValueI32 = 10;
            var newTestValueS = "Goodbye, moon";
            var newTestValueI32 = 5;

            var engine = new AttributedMapper();

            var instance = new Property_Single_String
            {
                Test = testValueS,
            };

            var instanceS = new Property_Single_String
            {
                Test = testValueS,
            };

            var instanceI32 = new Property_Single_Int32
            {
                Test = testValueI32,
            };

            Assert.AreEqual<string>(instanceS.Test, testValueS);
            Assert.AreEqual<Int32>(instanceI32.Test, testValueI32);

            IProperty_Double_StringInt32 resultant;
            resultant = engine.CreateInstance<IProperty_Double_StringInt32>(instanceS, instanceI32);

            VerifyType_Double_Instance(resultant.GetType(), "Property_Double_StringInt32", typeof(Property_Single_String), typeof(Property_Single_Int32));

            Assert.AreEqual<string>(resultant.TestS, testValueS, "Resultant did not match testValueS");
            Assert.AreEqual<string>(resultant.TestS, instanceS.Test, "Resultant did not match instance (testValueS)");

            Assert.AreEqual<Int32>(resultant.TestI32, testValueI32, "Resultant did not match testValueI32");
            Assert.AreEqual<Int32>(resultant.TestI32, instanceI32.Test, "Resultant did not match instance (testValueI32)");

            //adjust the value to check the property can be written to
            resultant.TestS = newTestValueS;
            resultant.TestI32 = newTestValueI32;

            Assert.AreEqual<string>(resultant.TestS, newTestValueS, "Resultant did not match newTestValueS");
            Assert.AreEqual<string>(resultant.TestS, instanceS.Test, "Resultant did not match instance (newTestValueS)");

            Assert.AreEqual<Int32>(resultant.TestI32, newTestValueI32, "Resultant did not match newTestValueI32");
            Assert.AreEqual<Int32>(resultant.TestI32, instanceI32.Test, "Resultant did not match instance (newTestValueI32)");
        }


    }
}
