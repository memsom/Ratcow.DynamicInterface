using Ratcow.DynamicInterface.Tests.Data;
using Ratcow.DynamicInterface.Tests.Data.Interfaces;
using Ratcow.DynamicInterface.Tests.Support;

namespace Ratcow.DynamicInterface.Tests;

[TestFixture]
public class AttributedMapperPropertyTests : BaseTest
{
    [Test]
    public void AttributedMapper_PropertyTests_Property_Single_Int32_Type()
    {
        var testValue = 10;

        var engine = new AttributedMapper();

        var instance = new PropertySingleInt32
        {
            Test = testValue,
        };

        Assert.That(testValue, Is.EqualTo(instance.Test));

        var resultant = engine.CreateType<IPropertySingleInt32>(instance);

        VerifyType_Single_Instance(resultant, nameof(PropertySingleInt32), typeof(PropertySingleInt32));
    }

    [Test]
    public void AttributedMapper_PropertyTests_Property_Single_Int32_Instance()
    {
        var testValue = 10;
        var newTestValue = 5;

        var engine = new AttributedMapper();

        var instance = new PropertySingleInt32
        {
            Test = testValue,
        };

        Assert.That(testValue, Is.EqualTo(instance.Test), "Instance did not match testValue");

        IPropertySingleInt32 resultant;

        resultant = engine.CreateInstance<IPropertySingleInt32>(instance);

        VerifyType_Single_Instance(resultant.GetType(), nameof(PropertySingleInt32), typeof(PropertySingleInt32));

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

        var instance = new PropertySingleString
        {
            Test = testValue,
        };

        Assert.That(testValue, Is.EqualTo(instance.Test));

        var resultant = engine.CreateType<IPropertySingleString>(instance);

        VerifyType_Single_Instance(resultant, nameof(PropertySingleString), typeof(PropertySingleString));
    }

    [Test]
    public void AttributedMapper_PropertyTests_Property_Single_String_Instance()
    {
        var testValue = "Hello, world";
        var newTestValue = "Goodbye, moon";

        var engine = new AttributedMapper();

        var instance = new PropertySingleString
        {
            Test = testValue,
        };

        Assert.That(testValue, Is.EqualTo(instance.Test), "Instance did not match testValue");

        IPropertySingleString resultant;

        resultant = engine.CreateInstance<IPropertySingleString>(instance);

        VerifyType_Single_Instance(resultant.GetType(), nameof(PropertySingleString), typeof(PropertySingleString));

        Assert.That(testValue, Is.EqualTo(resultant.TestS), "Resultant did not match testValue");
        Assert.That(instance.Test, Is.EqualTo(resultant.TestS), "Resultant did not match instance (testValue)");

        //adjust the value to check the property can be written to
        resultant.TestS = newTestValue;

        Assert.That(newTestValue, Is.EqualTo(resultant.TestS), "Resultant did not match newTestValue");
        Assert.That(instance.Test, Is.EqualTo(resultant.TestS), "Resultant did not match instance (newTestValue)");
    }

    private const string PropertyDoubleStringInt32 = "PropertyDoubleStringInt32"; // apparently this name is pulled out of thin air....

    [Test]
    public void AttributedMapper_PropertyTests_Property_Double_StringInt32_Type()
    {
        var testValueS = "Hello, world";
        var testValueI32 = 10;

        var engine = new AttributedMapper();

        var instanceS = new PropertySingleString
        {
            Test = testValueS,
        };

        var instanceI32 = new PropertySingleInt32
        {
            Test = testValueI32,
        };

        Assert.That(testValueS, Is.EqualTo(instanceS.Test));
        Assert.That(testValueI32, Is.EqualTo(instanceI32.Test));

        var resultant = engine.CreateType<IPropertyDoubleStringInt32>(instanceS, instanceI32);

        VerifyType_Double_Instance(resultant, PropertyDoubleStringInt32, typeof(PropertySingleString), typeof(PropertySingleInt32));
    }

    [Test]
    public void AttributedMapper_PropertyTests_Property_Double_StringInt32_Instance()
    {
        var testValueS = "Hello, world";
        var testValueI32 = 10;
        var newTestValueS = "Goodbye, moon";
        var newTestValueI32 = 5;

        var engine = new AttributedMapper();

        var instanceS = new PropertySingleString
        {
            Test = testValueS,
        };

        var instanceI32 = new PropertySingleInt32
        {
            Test = testValueI32,
        };

        Assert.That(testValueS, Is.EqualTo(instanceS.Test));
        Assert.That(testValueI32, Is.EqualTo(instanceI32.Test));

        IPropertyDoubleStringInt32 resultant;
        resultant = engine.CreateInstance<IPropertyDoubleStringInt32>(instanceS, instanceI32);

        VerifyType_Double_Instance(resultant.GetType(), PropertyDoubleStringInt32, typeof(PropertySingleString), typeof(PropertySingleInt32));

        Assert.That(resultant.TestS, Is.EqualTo(testValueS), "Resultant did not match testValueS");
        Assert.That(resultant.TestS, Is.EqualTo(instanceS.Test), "Resultant did not match instance (testValueS)");

        Assert.That(resultant.TestI32, Is.EqualTo(testValueI32), "Resultant did not match testValueI32");
        Assert.That(resultant.TestI32, Is.EqualTo(instanceI32.Test), "Resultant did not match instance (testValueI32)");

        //adjust the value to check the property can be written to
        resultant.TestS = newTestValueS;
        resultant.TestI32 = newTestValueI32;

        Assert.That(newTestValueS, Is.EqualTo(resultant.TestS), "Resultant did not match newTestValueS");
        Assert.That(instanceS.Test, Is.EqualTo(resultant.TestS), "Resultant did not match instance (newTestValueS)");

        Assert.That(newTestValueI32, Is.EqualTo(resultant.TestI32), "Resultant did not match newTestValueI32");
        Assert.That(instanceI32.Test, Is.EqualTo(resultant.TestI32), "Resultant did not match instance (newTestValueI32)");
    }


}