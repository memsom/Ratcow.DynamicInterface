using Ratcow.DynamicInterface.Tests.Data;
using Ratcow.DynamicInterface.Tests.Data.Interfaces;
using Ratcow.DynamicInterface.Tests.Support;

namespace Ratcow.DynamicInterface.Tests;

[TestFixture]
public class AttributedMapper_PropertyTests : BaseTest
{
    [Test]
    public void AttributedMapper_PropertyTests_Property_Single_Int32_Type()
    {
        var testValue = 10;

        var engine = new AttributedMapper();

        var instance = new Property_Single_Int32
        {
            Test = testValue,
        };

        Assert.AreEqual(instance.Test, testValue);

        Type resultant;

        resultant = engine.CreateType<IProperty_Single_Int32>(instance);

        VerifyType_Single_Instance(resultant, "Property_Single_Int32", typeof(Property_Single_Int32));
    }

    [Test]
    public void AttributedMapper_PropertyTests_Property_Single_Int32_Instance()
    {
        var testValue = 10;
        var newTestValue = 5;

        var engine = new AttributedMapper();

        var instance = new Property_Single_Int32
        {
            Test = testValue,
        };

        Assert.That(testValue, Is.EqualTo(instance.Test), "Instance did not match testValue");

        IProperty_Single_Int32 resultant;

        resultant = engine.CreateInstance<IProperty_Single_Int32>(instance);

        VerifyType_Single_Instance(resultant.GetType(), "Property_Single_Int32", typeof(Property_Single_Int32));

        Assert.That(testValue, Is.EqualTo(resultant.TestI32), "Resultant did not match testValue");
        Assert.That(instance.Test, Is.EqualTo(resultant.TestI32), "Resultant did not match instance (testValue)");

        //adjust the value to check the property can be written to
        resultant.TestI32 = newTestValue;

        Assert.That(newTestValue, Is.EqualTo(resultant.TestI32), "Resultant did not match testValue");
        Assert.That(instance.Test, Is.EqualTo(resultant.TestI32), "Resultant did not match instance (newTestValue)");
    }

    [Test]
    public void AttributedMapper_PropertyTests_Property_Single_String_Type()
    {
        var testValue = "Hello, world";

        var engine = new AttributedMapper();

        var instance = new Property_Single_String
        {
            Test = testValue,
        };

        Assert.That(testValue, Is.EqualTo(instance.Test));

        Type resultant;

        resultant = engine.CreateType<IProperty_Single_String>(instance);

        VerifyType_Single_Instance(resultant, "Property_Single_String", typeof(Property_Single_String));
    }

    [Test]
    public void AttributedMapper_PropertyTests_Property_Single_String_Instance()
    {
        var testValue = "Hello, world";
        var newTestValue = "Goodbye, moon";

        var engine = new AttributedMapper();

        var instance = new Property_Single_String
        {
            Test = testValue,
        };

        Assert.That(testValue, Is.EqualTo(instance.Test), "Instance did not match testValue");

        IProperty_Single_String resultant;

        resultant = engine.CreateInstance<IProperty_Single_String>(instance);

        VerifyType_Single_Instance(resultant.GetType(), "Property_Single_String", typeof(Property_Single_String));

        Assert.That(testValue, Is.EqualTo(resultant.TestS), "Resultant did not match testValue");
        Assert.That(instance.Test, Is.EqualTo(resultant.TestS), "Resultant did not match instance (testValue)");

        //adjust the value to check the property can be written to
        resultant.TestS = newTestValue;

        Assert.That(newTestValue, Is.EqualTo(resultant.TestS), "Resultant did not match newTestValue");
        Assert.That(instance.Test, Is.EqualTo(resultant.TestS), "Resultant did not match instance (newTestValue)");
    }

    [Test]
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

        Assert.That(testValueS, Is.EqualTo(instanceS.Test));
        Assert.That(testValueI32, Is.EqualTo(instanceI32.Test));

        Type resultant;

        resultant = engine.CreateType<IProperty_Double_StringInt32>(instanceS, instanceI32);

        VerifyType_Double_Instance(resultant, "Property_Double_StringInt32", typeof(Property_Single_String), typeof(Property_Single_Int32));
    }

    [Test]
    public void AttributedMapper_PropertyTests_Property_Double_StringInt32_Instance()
    {
        var testValueS = "Hello, world";
        var testValueI32 = 10;
        var newTestValueS = "Goodbye, moon";
        var newTestValueI32 = 5;

        var engine = new AttributedMapper();

        var instanceS = new Property_Single_String
        {
            Test = testValueS,
        };

        var instanceI32 = new Property_Single_Int32
        {
            Test = testValueI32,
        };

        Assert.That(testValueS, Is.EqualTo(instanceS.Test));
        Assert.That(testValueI32, Is.EqualTo(instanceI32.Test));

        IProperty_Double_StringInt32 resultant;
        resultant = engine.CreateInstance<IProperty_Double_StringInt32>(instanceS, instanceI32);

        VerifyType_Double_Instance(resultant.GetType(), "Property_Double_StringInt32", typeof(Property_Single_String), typeof(Property_Single_Int32));

        Assert.That(testValueS, Is.EqualTo(resultant.TestS), "Resultant did not match testValueS");
        Assert.That(instanceS.Test, Is.EqualTo(resultant.TestS), "Resultant did not match instance (testValueS)");

        Assert.That(testValueI32, Is.EqualTo(resultant.TestI32), "Resultant did not match testValueI32");
        Assert.That(instanceI32.Test, Is.EqualTo(resultant.TestI32), "Resultant did not match instance (testValueI32)");

        //adjust the value to check the property can be written to
        resultant.TestS = newTestValueS;
        resultant.TestI32 = newTestValueI32;

        Assert.That(newTestValueS, Is.EqualTo(resultant.TestS), "Resultant did not match newTestValueS");
        Assert.That(instanceS.Test, Is.EqualTo(resultant.TestS), "Resultant did not match instance (newTestValueS)");

        Assert.That(newTestValueI32, Is.EqualTo(resultant.TestI32), "Resultant did not match newTestValueI32");
        Assert.That(instanceI32.Test, Is.EqualTo(resultant.TestI32), "Resultant did not match instance (newTestValueI32)");
    }


}