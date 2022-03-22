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
    public class LambdaConditionTests
    {
        [TestMethod]
        public void SimpleTest1()
        {
            // Arrange

            A a = new A();
            a.Prop1 = true;
            LambdaCondition<A> condition = new LambdaCondition<A>(a, x => x.Prop1, false);

            // Assert

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void SimpleTest2()
        {
            // Arrange

            A a = new A();
            a.B.Prop1 = true;
            LambdaCondition<A> condition = new LambdaCondition<A>(a, x => x.B.Prop1, false);

            // Assert

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void UpdateValueTest1()
        {
            // Arrange

            A a = new A();
            LambdaCondition<A> condition = new LambdaCondition<A>(a, x => x.Prop1, false);
            bool? notification = null;
            condition.PropertyChanged += (s, e) => { notification = condition.Value; };

            // Act

            a.Prop1 = true;

            // Assert

            Assert.AreEqual(true, condition.Value);
            Assert.AreEqual(true, notification);
        }

        [TestMethod]
        public void UpdateValueTest2()
        {
            // Arrange

            A a = new A();
            LambdaCondition<A> condition = new LambdaCondition<A>(a, x => x.B.Prop1, false);
            bool? notification = null;
            condition.PropertyChanged += (s, e) => { notification = condition.Value; };

            // Act

            a.B.Prop1 = true;

            // Assert

            Assert.AreEqual(true, condition.Value);
            Assert.AreEqual(true, notification);
        }

        [TestMethod]
        public void ExpressionTest1()
        {
            // Arrange

            A a = new A();
            LambdaCondition<A> condition = new LambdaCondition<A>(a, x => x.Prop1 && x.Prop2, false);
            bool? notification = null;
            condition.PropertyChanged += (s, e) => { notification = condition.Value; };

            // Act

            a.Prop1 = true;
            a.Prop2 = true;

            // Assert

            Assert.AreEqual(true, condition.Value);
            Assert.AreEqual(true, notification);
        }

        [TestMethod]
        public void ExpressionTest2()
        {
            // Arrange

            A a = new A();
            LambdaCondition<A> condition = new LambdaCondition<A>(a, x => x.B.C.Prop1 && x.B.Prop2, false);
            bool? notification = null;
            condition.PropertyChanged += (s, e) => { notification = condition.Value; };

            // Act

            a.B.C.Prop1 = true;
            a.B.Prop2 = true;

            // Assert

            Assert.AreEqual(true, condition.Value);
            Assert.AreEqual(true, notification);
        }

        [TestMethod]
        public void ExpressionTest3()
        {
            // Arrange

            A a = new A();
            LambdaCondition<A> condition = new LambdaCondition<A>(a, x => x.Prop3 > 10, false);
            bool? notification = null;
            condition.PropertyChanged += (s, e) => { notification = condition.Value; };

            // Act

            a.Prop3 = 20;

            // Assert

            Assert.AreEqual(true, condition.Value);
            Assert.AreEqual(true, notification);
        }


        [TestMethod]
        public void NullTest1()
        {
            // Arrange

            A a = new A();
            a.B = null;
            LambdaCondition<A> condition = new LambdaCondition<A>(a, x => x.B.Prop1, true);

            // Assert

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void NullTest2()
        {
            // Arrange

            A a = new A();
            a.B.C = null;
            LambdaCondition<A> condition = new LambdaCondition<A>(a, x => x.B.Prop1 && x.B.C.Prop1, true);

            // Assert

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void ReplaceTest1()
        {
            // Arrange

            A a = new A();
            LambdaCondition<A> condition = new LambdaCondition<A>(a, x => x.B.C.Prop1, false);
            bool? notification = null;
            condition.PropertyChanged += (s, e) => { notification = condition.Value; };

            // Act
            B b = new B();
            b.C.Prop1 = true;
            a.B = b;

            // Assert

            Assert.AreEqual(true, condition.Value);
            Assert.AreEqual(true, notification);
        }

        [TestMethod]
        public void ReplaceTest2()
        {
            // Arrange

            A a = new A();
            a.B.Prop1 = true;
            LambdaCondition<A> condition = new LambdaCondition<A>(a, x => x.B.Prop1 && x.B.C.Prop1, false);
            bool? notification = null;
            condition.PropertyChanged += (s, e) => { notification = condition.Value; };

            // Act

            C c = new C();
            c.Prop1 = true;
            a.B.C = c;

            // Assert

            Assert.AreEqual(true, condition.Value);
            Assert.AreEqual(true, notification);
        }

        [TestMethod]
        public void PropertyChangedEventCleanupTest1()
        {
            // Arrange

            A a = new A();
            B oldB = a.B;

            LambdaCondition<A> condition = new LambdaCondition<A>(a, x => x.B.C.Prop1, false);

            // Act

            a.B = new B();

            // Assert

            Assert.AreEqual(false, oldB.HavePropertyChangeHandlers());
            Assert.AreEqual(false, oldB.C.HavePropertyChangeHandlers());
        }
    }
}
