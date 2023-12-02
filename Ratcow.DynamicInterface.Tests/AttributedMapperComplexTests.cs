using Ratcow.DynamicInterface.Tests.Data;
using Ratcow.DynamicInterface.Tests.Data.Interfaces;
using Ratcow.DynamicInterface.Tests.Support;

namespace Ratcow.DynamicInterface.Tests;

[TestFixture]
public class AttributedMapperComplexTests: BaseTest
{
    [Test]
    public void AttributedMapper_ComplexExample()
    {
        var engine = new AttributedMapper();

        Assert.IsNotNull(engine, "Engine can not be null");

        var instance = new ComplexExample();

        VerifyType_Single_Instance(engine.CreateType<IComplexExample>(instance), nameof(ComplexExample), typeof(ComplexExample));

        // test actual instance

        var resultant = engine.CreateInstance<IComplexExample>(instance);

        Assert.IsNotNull(resultant);

        resultant.Simple0();
        resultant.Simple1(10);

        resultant.Simple1Out(out var r);
        System.Diagnostics.Debug.WriteLine($"Received Simple1Out(out {r})");

        var result = resultant.Simple0Return();
        System.Diagnostics.Debug.WriteLine($"Received Simple0Return -> {result}");
    }
}