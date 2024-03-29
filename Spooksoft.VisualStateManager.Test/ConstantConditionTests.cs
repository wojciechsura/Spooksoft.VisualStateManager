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
    public class ConstantConditionTests
    {
        [TestMethod]    
        public void TrueConditionTest()
        {
            // Arrange

            var condition = Condition.True();

            // Assert

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void FalseConditionTest()
        {
            // Arrange

            var condition = Condition.False();

            // Assert

            Assert.AreEqual(false, condition.Value);
        }
    }
}
