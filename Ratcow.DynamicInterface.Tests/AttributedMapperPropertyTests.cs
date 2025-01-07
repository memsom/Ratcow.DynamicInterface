namespace Ratcow.DynamicInterface.Tests;

public class AttributedMapperPropertyTests : BaseTest
{
    [Fact]
    public void AttributedMapper_PropertyTests_Property_Single_Int32_Type()
    {
        var testValue = 10;

        var engine = new AttributedMapper();

        var instance = new PropertySingleInt32
        {
            Test = testValue,
        };

        Assert.Equal(testValue, instance.Test);

        var resultant = engine.CreateType<IPropertySingleInt32>(instance);

        VerifyType_Single_Instance(resultant, nameof(PropertySingleInt32), typeof(PropertySingleInt32));
    }

    [Fact]
    public void AttributedMapper_PropertyTests_Property_Single_Int32_Instance()
    {
        var testValue = 10;
        var newTestValue = 5;

        var engine = new AttributedMapper();

        var instance = new PropertySingleInt32
        {
            Test = testValue,
        };

        Assert.Equal(testValue, instance.Test);

        IPropertySingleInt32 resultant;

        resultant = engine.CreateInstance<IPropertySingleInt32>(instance);

        VerifyType_Single_Instance(resultant.GetType(), nameof(PropertySingleInt32), typeof(PropertySingleInt32));

        Assert.Equal(testValue, resultant.TestI32);
        Assert.Equal(instance.Test, resultant.TestI32);

        //adjust the value to check the property can be written to
        resultant.TestI32 = newTestValue;

        Assert.Equal(newTestValue, resultant.TestI32);
        Assert.Equal(instance.Test, resultant.TestI32);
    }

    [Fact]
    public void AttributedMapper_PropertyTests_Property_Single_String_Type()
    {
        var testValue = "Hello, world";

        var engine = new AttributedMapper();

        var instance = new PropertySingleString
        {
            Test = testValue,
        };

        Assert.Equal(testValue, instance.Test);

        var resultant = engine.CreateType<IPropertySingleString>(instance);

        VerifyType_Single_Instance(resultant, nameof(PropertySingleString), typeof(PropertySingleString));
    }

    [Fact]
    public void AttributedMapper_PropertyTests_Property_Single_String_Instance()
    {
        var testValue = "Hello, world";
        var newTestValue = "Goodbye, moon";

        var engine = new AttributedMapper();

        var instance = new PropertySingleString
        {
            Test = testValue,
        };

        Assert.Equal(testValue, instance.Test);

        var resultant = engine.CreateInstance<IPropertySingleString>(instance);

        VerifyType_Single_Instance(resultant.GetType(), nameof(PropertySingleString), typeof(PropertySingleString));

        Assert.Equal(testValue, resultant.TestS);
        Assert.Equal(instance.Test, resultant.TestS);

        //adjust the value to check the property can be written to
        resultant.TestS = newTestValue;

        Assert.Equal(newTestValue, resultant.TestS);
        Assert.Equal(instance.Test, resultant.TestS);
    }

    private const string PropertyDoubleStringInt32 = "PropertyDoubleStringInt32"; // apparently this name is pulled out of thin air....

    [Fact]
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

        Assert.Equal(testValueS, instanceS.Test);
        Assert.Equal(testValueI32, instanceI32.Test);

        var resultant = engine.CreateType<IPropertyDoubleStringInt32>(instanceS, instanceI32);

        VerifyType_Double_Instance(resultant, PropertyDoubleStringInt32, typeof(PropertySingleString), typeof(PropertySingleInt32));
    }

    [Fact]
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

        Assert.Equal(testValueS, instanceS.Test);
        Assert.Equal(testValueI32, instanceI32.Test);

        IPropertyDoubleStringInt32 resultant;
        resultant = engine.CreateInstance<IPropertyDoubleStringInt32>(instanceS, instanceI32);

        VerifyType_Double_Instance(resultant.GetType(), PropertyDoubleStringInt32, typeof(PropertySingleString), typeof(PropertySingleInt32));

        Assert.Equal(resultant.TestS, testValueS);
        Assert.Equal(resultant.TestS, instanceS.Test);

        Assert.Equal(resultant.TestI32, testValueI32);
        Assert.Equal(resultant.TestI32, instanceI32.Test);

        //adjust the value to check the property can be written to
        resultant.TestS = newTestValueS;
        resultant.TestI32 = newTestValueI32;

        Assert.Equal(newTestValueS, resultant.TestS);
        Assert.Equal(instanceS.Test, resultant.TestS);

        Assert.Equal(newTestValueI32, resultant.TestI32);
        Assert.Equal(instanceI32.Test, resultant.TestI32);
    }


}
