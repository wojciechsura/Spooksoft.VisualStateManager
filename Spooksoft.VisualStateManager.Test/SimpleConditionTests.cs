using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spooksoft.VisualStateManager.Conditions;
using System;

namespace Spooksoft.VisualStateManager.Test
{
    [TestClass]
    public class SimpleConditionTests
    {
        [TestMethod]
        public void DefaultValueTest1()
        {
            // Arrange

            var simpleCondition = Condition.Simple(false);

            // Assert

            Assert.AreEqual(false, simpleCondition.Value);
        }

        [TestMethod]
        public void DefaultValueTest2()
        {
            // Arrange

            var simpleCondition = Condition.Simple(true);

            // Assert

            Assert.AreEqual(true, simpleCondition.Value);
        }

        [TestMethod]
        public void ChangeValueTest()
        {
            // Arrange

            var simpleCondition = Condition.Simple(false);

            // Act

            simpleCondition.Value = true;

            // Assert

            Assert.AreEqual(true, simpleCondition.Value);
        }

        [TestMethod]
        public void NotificationTest()
        {
            // Arrange

            var simpleCondition = Condition.Simple(false);
            bool? notification = null;
            simpleCondition.PropertyChanged += (s, e) => { notification = simpleCondition.Value; };

            // Act

            simpleCondition.Value = true;

            // Assert

            Assert.AreEqual(true, notification);
        }
    }
}
