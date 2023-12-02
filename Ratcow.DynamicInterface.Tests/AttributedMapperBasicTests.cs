using Ratcow.DynamicInterface.Tests.Data.Interfaces;
using Ratcow.DynamicInterface.Tests.Support;

namespace Ratcow.DynamicInterface.Tests;

[TestFixture]
public class AttributedMapperBasicTests: BaseTest
{
    [Test]
    public void AttributedMapper_BasicTest_InstantiateEngine()
    {
        var engine = new AttributedMapper();

        Assert.IsNotNull(engine, "Engine can not be null");
    }

    [Test]
    public void AttributedMapper_BasicTest_OneType()
    {
        var engine = new AttributedMapper();

        Assert.IsNotNull(engine, "Engine can not be null");

        var instance = new object();

        var resultant = engine.CreateType<IBasic>(instance);

        VerifyType_Single_Instance(resultant, "Basic", typeof(object));
    }

    [Test]
    public void AttributedMapper_BasicTest_OneInstance()
    {
        var engine = new AttributedMapper();

        Assert.IsNotNull(engine, "Engine can not be null");

        var instance = new object();

        var resultant = engine.CreateInstance<IBasic>(instance);

        Assert.IsNotNull(resultant, "Resultant can not be null");

        var type = resultant.GetType();

        VerifyType_Single_Instance(type, "Basic", typeof(object));
    }
}