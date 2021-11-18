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
    public class SwitchConditionTests
    {
        [TestMethod]
        public void SimpleTest1()
        {
            // Arrange

            var switchCondition = new SwitchCondition<int>(1, 2, 3, 4);

            // Act

            switchCondition.Current = 2;

            // Assert

            Assert.AreEqual(false, switchCondition[1].GetValue());
            Assert.AreEqual(true, switchCondition[2].GetValue());
            Assert.AreEqual(false, switchCondition[3].GetValue());
            Assert.AreEqual(false, switchCondition[4].GetValue());
        }
    }
}
