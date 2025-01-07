namespace Ratcow.DynamicInterface.Tests;

public class AttributedMapperBasicTests: BaseTest
{
    [Fact]
    public void AttributedMapper_BasicTest_InstantiateEngine()
    {
        var engine = new AttributedMapper();

        Assert.NotNull(engine);
    }

    [Fact]
    public void AttributedMapper_BasicTest_OneType()
    {
        var engine = new AttributedMapper();

        Assert.NotNull(engine);

        var instance = new object();

        var resultant = engine.CreateType<IBasic>(instance);

        VerifyType_Single_Instance(resultant, "Basic", typeof(object));
    }

    [Fact]
    public void AttributedMapper_BasicTest_OneInstance()
    {
        var engine = new AttributedMapper();

        Assert.NotNull(engine);

        var instance = new object();

        var resultant = engine.CreateInstance<IBasic>(instance);

        Assert.NotNull(resultant);

        var type = resultant.GetType();

        VerifyType_Single_Instance(type, "Basic", typeof(object));
    }
}
