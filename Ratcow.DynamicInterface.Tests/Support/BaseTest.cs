namespace Ratcow.DynamicInterface.Tests.Support;

public class BaseTest
{
    /// <summary>
    /// Boiler plate code to verify the type.
    /// </summary>
    protected static void VerifyType_Single_Instance(Type? resultant, string expectedTypeName, Type expectedConstructorType)
    {
        Assert.NotNull(resultant);

        Assert.Equal(expectedTypeName, resultant.Name);

        var fields = resultant.GetFields();

        Assert.NotNull(fields);

        Assert.Single(fields);

        var constructors = resultant.GetConstructors();

        Assert.NotNull(constructors);

        Assert.Single(constructors);

        var constructorParams = constructors[0].GetParameters();

        Assert.Single(constructorParams);

        Assert.Equal(expectedConstructorType,constructorParams[0].ParameterType);
    }

    /// <summary>
    /// Boiler plate code to verify the type.
    /// </summary>
    protected static void VerifyType_Double_Instance(Type? resultant, string expectedTypeName, Type expectedConstructorType1, Type expectedConstructorType2)
    {
        Assert.NotNull(resultant);

        Assert.Equal(expectedTypeName, resultant.Name);

        var fields = resultant.GetFields();

        Assert.NotNull(fields);

        Assert.Equal(2, fields.Length);

        var constructors = resultant.GetConstructors();

        Assert.NotNull(constructors);

        Assert.Single(constructors);

        var constructorParams = constructors[0].GetParameters();

        Assert.Equal(2, constructorParams.Length);

        Assert.Equal(expectedConstructorType1, constructorParams[0].ParameterType);

        Assert.Equal(expectedConstructorType2, constructorParams[1].ParameterType);
    }
}
