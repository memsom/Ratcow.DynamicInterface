using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ratcow.DynamicInterface.Tests
{
    [TestClass]
    public class AttributedMapper_BasicTests
    {
        /// <summary>
        /// Boiler plate code to verify the type.
        /// </summary>
        /// <param name="resultant"></param>
        void VerifyType(Type resultant)
        {
            Assert.IsNotNull(resultant, "Resultant can not be null");

            Assert.AreEqual(resultant.Name, "Basic", "Resultant Type.Name shopuld equal 'Basic'");

            var fields = resultant.GetFields();

            Assert.IsNotNull(fields, "Basic should contain fields");

            Assert.AreEqual(fields.Length, 1, "Basic should include one field");

            var constructors = resultant.GetConstructors();

            Assert.IsNotNull(constructors, "Basic should contain constructors");

            Assert.AreEqual(constructors.Length, 1, "Basic should include one constructors");

            var constructorParams = constructors[0].GetParameters();

            Assert.AreEqual(constructorParams.Length, 1, "Basic constructors should include one parameter");

            Assert.AreEqual(constructorParams[0].ParameterType, typeof(object), "Basic constructors should include one parameter or type object");
        }

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

            VerifyType(resultant);
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

            VerifyType(type);
        }
    }
}
