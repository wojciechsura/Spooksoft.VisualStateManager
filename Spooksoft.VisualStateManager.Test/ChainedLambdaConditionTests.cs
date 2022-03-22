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
    public class ChainedLambdaConditionTests
    {
        [TestMethod]
        public void SimpleTest1()
        {
            // Arrange

            A a = new A();
            a.Prop1 = true;
            ChainedLambdaCondition<A> condition = new ChainedLambdaCondition<A>(a, x => x.Prop1, false);

            // Assert

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void SimpleTest2()
        {
            // Arrange

            A a = new A();
            a.B.Prop1 = true;
            ChainedLambdaCondition<A, B> condition = new ChainedLambdaCondition<A, B>(a, x => x.B, b => b.Prop1, false);

            // Assert

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void UpdateValueTest1()
        {
            // Arrange

            A a = new A();
            ChainedLambdaCondition<A> condition = new ChainedLambdaCondition<A>(a, x => x.Prop1, false);
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
            ChainedLambdaCondition<A, B> condition = new ChainedLambdaCondition<A, B>(a, x => x.B, b => b.Prop1, false);
            bool? notification = null;
            condition.PropertyChanged += (s, e) => { notification = condition.Value; };

            // Act

            a.B.Prop1 = true;

            // Assert

            Assert.AreEqual(true, condition.Value);
            Assert.AreEqual(true, notification);
        }

        [TestMethod]
        public void UpdateValueTest3()
        {
            // Arrange

            A a = new A();
            ChainedLambdaCondition<A, B, C> condition = new ChainedLambdaCondition<A, B, C>(a, x => x.B, b => b.C, c => c.Prop1, false);
            bool? notification = null;
            condition.PropertyChanged += (s, e) => { notification = condition.Value; };

            // Act

            a.B.C.Prop1 = true;

            // Assert

            Assert.AreEqual(true, condition.Value);
            Assert.AreEqual(true, notification);
        }

        [TestMethod]
        public void ExpressionTest1()
        {
            // Arrange

            A a = new A();
            ChainedLambdaCondition<A> condition = new ChainedLambdaCondition<A>(a, x => x.Prop1 && x.Prop2, false);
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
        public void ExpressionTest3()
        {
            // Arrange

            A a = new A();
            ChainedLambdaCondition<A> condition = new ChainedLambdaCondition<A>(a, x => x.Prop3 > 10, false);
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
            ChainedLambdaCondition<A, B> condition = new ChainedLambdaCondition<A, B>(a, x => x.B, b => b.Prop1, true);

            // Assert

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void NullTest2()
        {
            // Arrange

            A a = new A();
            a.B.C = null;
            ChainedLambdaCondition<A, B, C> condition = new ChainedLambdaCondition<A, B, C>(a, x => x.B, b => b.C, c => c.Prop1, true);

            // Assert

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void ReplaceTest1()
        {
            // Arrange

            A a = new A();
            ChainedLambdaCondition<A, B, C> condition = new ChainedLambdaCondition<A, B, C>(a, x => x.B, b => b.C, c => c.Prop1, false);
            bool? notification = null;
            condition.PropertyChanged += (s, e) => { notification = condition.Value; };

            // Act
            B b1 = new B();
            b1.C.Prop1 = true;
            a.B = b1;

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

            ChainedLambdaCondition<A, B, C> condition = new ChainedLambdaCondition<A, B, C>(a, x => x.B, b => b.C, c => c.Prop1, false);

            // Act

            a.B = new B();

            // Assert

            Assert.AreEqual(false, oldB.HavePropertyChangeHandlers());
            Assert.AreEqual(false, oldB.C.HavePropertyChangeHandlers());
        }

        [TestMethod]
        public void MethodCallTest1()
        {
            // Arrange

            A a = new A();
            a.B.List.Add(5);

            ChainedLambdaCondition<A, B> condition = new ChainedLambdaCondition<A, B>(a, x => x.B, b => b.List.Any(), false);

            // Assert

            Assert.AreEqual(true, condition.Value);
        }
    }
}
