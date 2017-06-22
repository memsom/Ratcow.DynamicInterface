using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratcow.DynamicInterface.Tests
{
    public class BaseTest
    {
        /// <summary>
        /// Boiler plate code to verify the type.
        /// </summary>
        protected void VerifyType_Single_Instance(Type resultant, string expectedTypeName, Type expectedConstructorType)
        {
            Assert.IsNotNull(resultant, "Resultant can not be null");

            Assert.AreEqual(resultant.Name, expectedTypeName, $"Resultant Type.Name should equal '{expectedTypeName}'");

            var fields = resultant.GetFields();

            Assert.IsNotNull(fields, $"{expectedTypeName} should contain fields");

            Assert.AreEqual(fields.Length, 1, $"{expectedTypeName} should include one field");

            var constructors = resultant.GetConstructors();

            Assert.IsNotNull(constructors, $"{expectedTypeName} should contain constructors");

            Assert.AreEqual(constructors.Length, 1, $"{expectedTypeName} should include one constructors");

            var constructorParams = constructors[0].GetParameters();

            Assert.AreEqual(constructorParams.Length, 1, $"{expectedTypeName} constructors should include one parameter");

            Assert.AreEqual(constructorParams[0].ParameterType, expectedConstructorType, $"{expectedTypeName} constructors should include one parameter or type {expectedConstructorType}");
        }

        /// <summary>
        /// Boiler plate code to verify the type.
        /// </summary>
        protected void VerifyType_Double_Instance(Type resultant, string expectedTypeName, Type expectedConstructorType1, Type expectedConstructorType2)
        {
            Assert.IsNotNull(resultant, "Resultant can not be null");

            Assert.AreEqual(resultant.Name, expectedTypeName, $"Resultant Type.Name should equal '{expectedTypeName}'");

            var fields = resultant.GetFields();

            Assert.IsNotNull(fields, $"{expectedTypeName} should contain fields");

            Assert.AreEqual(fields.Length, 2, $"{expectedTypeName} should include two fields");

            var constructors = resultant.GetConstructors();

            Assert.IsNotNull(constructors, $"{expectedTypeName} should contain constructors");

            Assert.AreEqual(constructors.Length, 1, $"{expectedTypeName} should include one constructors");

            var constructorParams = constructors[0].GetParameters();

            Assert.AreEqual(constructorParams.Length, 2, $"{expectedTypeName} constructors should include two parameter");

            Assert.AreEqual(constructorParams[0].ParameterType, expectedConstructorType1, $"{expectedTypeName} constructors should include one parameter or type {expectedConstructorType1}");

            Assert.AreEqual(constructorParams[1].ParameterType, expectedConstructorType2, $"{expectedTypeName} constructors should include one parameter or type {expectedConstructorType2}");
        }
    }
}
