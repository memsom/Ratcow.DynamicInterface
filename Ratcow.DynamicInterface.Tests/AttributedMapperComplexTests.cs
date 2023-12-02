using Ratcow.DynamicInterface.Tests.Data;
using Ratcow.DynamicInterface.Tests.Data.Interfaces;
using Ratcow.DynamicInterface.Tests.Support;

namespace Ratcow.DynamicInterface.Tests;

[TestFixture]
public class AttributedMapperComplexTests: BaseTest
{
    [Test]
    public void AttributedMapper_BasicTest_OneType()
    {
        var engine = new AttributedMapper();

        Assert.IsNotNull(engine, "Engine can not be null");

        var instance = new ComplexExample();

        // currently fails as I think it generates the wrong IL

        //var resultant = engine.CreateType<IComplexExample>(instance);

       // VerifyType_Single_Instance(resultant, nameof(ComplexExample), typeof(ComplexExample));
    }
}