namespace Ratcow.DynamicInterface.Tests;

public class ChainingAttributedMapperComplexTests: BaseTest
{
    [Fact]
    public void ChainingAttributedMapper_ComplexExample()
    {
        var engine = new ChainingAttributedMapper();

        Assert.NotNull(engine);

        var detour = new ComplexExample();
        var fallback = new ComplexExampleEx();

        var dynamicType = engine.CreateType<IComplexExampleEx>(detour, fallback);
        VerifyType_Double_Instance(dynamicType, nameof(ComplexExampleEx), typeof(ComplexExample), typeof(ComplexExampleEx));

        // test actual instance

        var resultant = engine.CreateInstance<IComplexExampleEx>(detour, fallback);

        Assert.NotNull(resultant);

        resultant.Simple0();
        resultant.Simple1(10);

        resultant.Simple1Out(out var r);
        System.Diagnostics.Debug.WriteLine($"Received Simple1Out(out {r})");

        var result = resultant.Simple0Return();
        System.Diagnostics.Debug.WriteLine($"Received Simple0Return -> {result}");
        
        resultant.NotIncluded(); // we should be able to call this
    }
}
