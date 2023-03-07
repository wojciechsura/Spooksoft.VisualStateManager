using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spooksoft.VisualStateManager.Conditions;
using Spooksoft.VisualStateManager.Test.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spooksoft.VisualStateManager.Test
{
    [TestClass]
    public class PropertyWatchConditionTests
    {
        [TestMethod]
        public void SimpleTest1()
        {
            // Arrange

            A a = new A();
            a.Prop1 = true;
            var condition = Condition.PropertyWatch(a, x => x.Prop1, false);

            // Assert

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void NotificationTest()
        {
            // Arrange

            A a = new A();
            a.Prop1 = true;
            var condition = Condition.PropertyWatch(a, x => x.Prop1, false);
            bool? notification = null;
            condition.PropertyChanged += (s, e) => { notification = condition.Value; };

            // Act

            a.Prop1 = false;

            // Assert

            Assert.AreEqual(false, notification);
        }
    }
}
