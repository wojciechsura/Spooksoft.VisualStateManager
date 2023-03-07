using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spooksoft.VisualStateManager.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spooksoft.VisualStateManager.Test
{
    [TestClass]
    public class CompositeConditionTests
    {
        [TestMethod]
        public void AndTest1()
        {
            // Arrange

            var simpleCondition1 = Condition.Simple(false);
            var simpleCondition2 = Condition.Simple(true);

            var compositeCondition = Condition.Composite(CompositeCondition.CompositionKind.And, simpleCondition1, simpleCondition2);

            // Assert

            Assert.AreEqual(false, compositeCondition.Value);
        }

        [TestMethod]
        public void AndTest2()
        {
            // Arrange

            var simpleCondition1 = Condition.Simple(false);
            var simpleCondition2 = Condition.Simple(false);

            var compositeCondition = Condition.Composite(CompositeCondition.CompositionKind.And, simpleCondition1, simpleCondition2);

            // Act

            simpleCondition1.Value = true;
            simpleCondition2.Value = true;

            // Assert

            Assert.AreEqual(true, compositeCondition.Value);
        }

        [TestMethod]
        public void OrTest1()
        {
            // Arrange

            var simpleCondition1 = Condition.Simple(false);
            var simpleCondition2 = Condition.Simple(true);

            var compositeCondition = Condition.Composite(CompositeCondition.CompositionKind.Or, simpleCondition1, simpleCondition2);

            // Assert

            Assert.AreEqual(true, compositeCondition.Value);
        }

        [TestMethod]
        public void OrTest2()
        {
            // Arrange

            var simpleCondition1 = Condition.Simple(false);
            var simpleCondition2 = Condition.Simple(true);

            var compositeCondition = Condition.Composite(CompositeCondition.CompositionKind.Or, simpleCondition1, simpleCondition2);

            // Act

            simpleCondition1.Value = false;
            simpleCondition2.Value = false;

            // Assert

            Assert.AreEqual(false, compositeCondition.Value);
        }

        [TestMethod]
        public void NotificationTest()
        {
            // Arrange

            var simpleCondition1 = Condition.Simple(false);
            var simpleCondition2 = Condition.Simple(true);
            bool? notification = null;

            var compositeCondition = Condition.Composite(CompositeCondition.CompositionKind.Or, simpleCondition1, simpleCondition2);
            compositeCondition.PropertyChanged += (s, e) => { notification = compositeCondition.Value; };

            // Act

            simpleCondition1.Value = false;
            simpleCondition2.Value = false;

            // Assert

            Assert.AreEqual(false, notification);
        }
    }
}
