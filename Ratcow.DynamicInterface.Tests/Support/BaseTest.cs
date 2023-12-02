namespace Ratcow.DynamicInterface.Tests.Support;

public class BaseTest
{
    /// <summary>
    /// Boiler plate code to verify the type.
    /// </summary>
    protected void VerifyType_Single_Instance(Type? resultant, string expectedTypeName, Type expectedConstructorType)
    {
        Assert.IsNotNull(resultant, "Resultant can not be null");

        if (resultant is null)
        {
            Assert.Fail(); // this is here only to make the compiler happy about nullables
            return;
        }

        Assert.That(expectedTypeName, Is.EqualTo(resultant.Name), $"Resultant Type.Name should equal '{expectedTypeName}'");

        var fields = resultant.GetFields();

        Assert.IsNotNull(fields, $"{expectedTypeName} should contain fields");

        Assert.That(fields.Length, Is.EqualTo(1), $"{expectedTypeName} should include one field");

        var constructors = resultant.GetConstructors();

        Assert.IsNotNull(constructors, $"{expectedTypeName} should contain constructors");

        Assert.That(constructors.Length, Is.EqualTo(1), $"{expectedTypeName} should include one constructors");

        var constructorParams = constructors[0].GetParameters();

        Assert.That(constructorParams.Length, Is.EqualTo(1), $"{expectedTypeName} constructors should include one parameter");

        Assert.That(expectedConstructorType, Is.EqualTo(constructorParams[0].ParameterType), $"{expectedTypeName} constructors should include one parameter or type {expectedConstructorType}");
    }

    /// <summary>
    /// Boiler plate code to verify the type.
    /// </summary>
    protected void VerifyType_Double_Instance(Type? resultant, string expectedTypeName, Type expectedConstructorType1, Type expectedConstructorType2)
    {
        Assert.IsNotNull(resultant, "Resultant can not be null");

        if (resultant is null)
        {
            Assert.Fail(); // this is here only to make the compiler happy about nullables
            return;
        }

        Assert.That(expectedTypeName, Is.EqualTo(resultant.Name), $"Resultant Type.Name should equal '{expectedTypeName}'");

        var fields = resultant.GetFields();

        Assert.IsNotNull(fields, $"{expectedTypeName} should contain fields");

        Assert.That(fields.Length, Is.EqualTo(2), $"{expectedTypeName} should include two fields");

        var constructors = resultant.GetConstructors();

        Assert.IsNotNull(constructors, $"{expectedTypeName} should contain constructors");

        Assert.That(constructors.Length, Is.EqualTo(1), $"{expectedTypeName} should include one constructors");

        var constructorParams = constructors[0].GetParameters();

        Assert.That(constructorParams.Length, Is.EqualTo(2), $"{expectedTypeName} constructors should include two parameter");

        Assert.That(expectedConstructorType1, Is.EqualTo(constructorParams[0].ParameterType), $"{expectedTypeName} constructors should include one parameter or type {expectedConstructorType1}");

        Assert.That(expectedConstructorType2, Is.EqualTo(constructorParams[1].ParameterType), $"{expectedTypeName} constructors should include one parameter or type {expectedConstructorType2}");
    }
}