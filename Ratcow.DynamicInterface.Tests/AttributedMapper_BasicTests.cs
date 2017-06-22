using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ratcow.DynamicInterface.Tests
{
    [TestClass]
    public class AttributedMapper_BasicTests: BaseTest
    {


        [TestMethod]
        public void AttributedMapper_BasicTest_InstantiateEngine()
        {
            var engine = new AttributedMapper();

            Assert.IsNotNull(engine, "Engine can not be null");
        }

        [TestMethod]
        public void AttributedMapper_BasicTest_OneType()
        {
            var engine = new AttributedMapper();

            Assert.IsNotNull(engine, "Engine can not be null");

            var instance = new object();

            Type resultant = null;

            resultant = engine.CreateType<IBasic>(instance);

            VerifyType_Single_Instance(resultant, "Basic", typeof(object));
        }

        [TestMethod]
        public void AttributedMapper_BasicTest_OneInstance()
        {
            var engine = new AttributedMapper();

            Assert.IsNotNull(engine, "Engine can not be null");

            var instance = new object();

            IBasic resultant = null;

            resultant = engine.CreateInstance<IBasic>(instance);

            Assert.IsNotNull(resultant, "Resultant can not be null");

            var type = resultant.GetType();

            VerifyType_Single_Instance(type, "Basic", typeof(object));
        }
    }
}
