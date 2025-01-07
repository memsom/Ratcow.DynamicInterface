namespace Ratcow.DynamicInterface.Tests;

public class AttributedMapperComplexTests: BaseTest
{
    [Fact]
    public void AttributedMapper_ComplexExample()
    {
        var engine = new AttributedMapper();

        Assert.NotNull(engine);

        var instance = new ComplexExample();

        VerifyType_Single_Instance(engine.CreateType<IComplexExample>(instance), nameof(ComplexExample), typeof(ComplexExample));

        // test actual instance

        var resultant = engine.CreateInstance<IComplexExample>(instance);

        Assert.NotNull(resultant);

        resultant.Simple0();
        resultant.Simple1(10);

        resultant.Simple1Out(out var r);
        System.Diagnostics.Debug.WriteLine($"Received Simple1Out(out {r})");

        var result = resultant.Simple0Return();
        System.Diagnostics.Debug.WriteLine($"Received Simple0Return -> {result}");
    }
}
