﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spooksoft.VisualStateManager.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spooksoft.VisualStateManager.Test
{
    [TestClass]
    public class NegateConditionTests
    {
        [TestMethod]
        public void SimpleTest1()
        {
            // Arrange

            var simpleCondition = Condition.Simple(false);
            var negateCondition = Condition.Negate(simpleCondition);

            // Assert

            Assert.AreEqual(true, negateCondition.Value);
        }

        [TestMethod]
        public void NotificationTest()
        {
            // Arrange

            var simpleCondition = Condition.Simple(false);
            var negateCondition = Condition.Negate(simpleCondition);
            bool? notification = null;
            negateCondition.PropertyChanged += (s, e) => { notification = negateCondition.Value; };

            // Act

            simpleCondition.Value = true;

            // Assert

            Assert.AreEqual(false, negateCondition.Value);
        }
    }
}
