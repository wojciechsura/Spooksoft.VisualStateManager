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

            var simpleCondition = new SimpleCondition(false);
            var negateCondition = new NegateCondition(simpleCondition);

            // Assert

            Assert.AreEqual(true, negateCondition.GetValue());
        }

        [TestMethod]
        public void NotificationTest()
        {
            // Arrange

            var simpleCondition = new SimpleCondition(false);
            var negateCondition = new NegateCondition(simpleCondition);
            bool? notification = null;
            negateCondition.ValueChanged += (s, e) => { notification = e.Value; };

            // Act

            simpleCondition.Value = true;

            // Assert

            Assert.AreEqual(false, negateCondition.GetValue());
        }
    }
}