using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spooksoft.VisualStateManager.Conditions;
using Spooksoft.VisualStateManager.Test.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            ChainedLambdaCondition<A> condition = Condition.ChainedLambda(a, x => x.Prop1, false);

            // Assert

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void SimpleTest2()
        {
            // Arrange

            A a = new A();
            a.B.Prop1 = true;
            ChainedLambdaCondition<A, B> condition = Condition.ChainedLambda(a, x => x.B, b => b.Prop1, false);

            // Assert

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void UpdateValueTest1()
        {
            // Arrange

            A a = new A();
            ChainedLambdaCondition<A> condition = Condition.ChainedLambda(a, x => x.Prop1, false);
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
            ChainedLambdaCondition<A, B> condition = Condition.ChainedLambda(a, x => x.B, b => b.Prop1, false);
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
            ChainedLambdaCondition<A, B, C> condition = Condition.ChainedLambda(a, x => x.B, b => b.C, c => c.Prop1, false);
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
            ChainedLambdaCondition<A> condition = Condition.ChainedLambda(a, x => x.Prop1 && x.Prop2, false);
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
            ChainedLambdaCondition<A> condition = Condition.ChainedLambda(a, x => x.Prop3 > 10, false);
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
            ChainedLambdaCondition<A, B> condition = Condition.ChainedLambda(a, x => x.B, b => b.Prop1, true);

            // Assert

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void NullTest2()
        {
            // Arrange

            A a = new A();
            a.B.C = null;
            ChainedLambdaCondition<A, B, C> condition = Condition.ChainedLambda(a, x => x.B, b => b.C, c => c.Prop1, true);

            // Assert

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void ReplaceTest1()
        {
            // Arrange

            A a = new A();
            ChainedLambdaCondition<A, B, C> condition = Condition.ChainedLambda(a, x => x.B, b => b.C, c => c.Prop1, false);
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

            ChainedLambdaCondition<A, B, C> condition = Condition.ChainedLambda(a, x => x.B, b => b.C, c => c.Prop1, false);

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

            ChainedLambdaCondition<A, B> condition = Condition.ChainedLambda(a, x => x.B, b => b.List.Any(), false);

            // Assert

            Assert.AreEqual(true, condition.Value);
        }

        // Collection change tests -------------------------------------------

        [TestMethod]
        public void CollectionCountChainedTest()
        {
            // Arrange

            A a = new A();
            ChainedLambdaCondition<A, B> condition = Condition.ChainedLambda(a, x => x.B, b => b.Items.Any(), false);

            // Act & Assert

            Assert.AreEqual(false, condition.Value);

            a.B.Items.Add("item");

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void CollectionCountChainedNotificationTest()
        {
            // Arrange

            A a = new A();
            ChainedLambdaCondition<A, B> condition = Condition.ChainedLambda(a, x => x.B, b => b.Items.Any(), false);
            bool? notification = null;
            condition.PropertyChanged += (s, e) => { notification = condition.Value; };

            // Act

            a.B.Items.Add("item");

            // Assert

            Assert.AreEqual(true, condition.Value);
            Assert.AreEqual(true, notification);
        }

        [TestMethod]
        public void CollectionFirstElementTest()
        {
            // Arrange

            A a = new A();
            a.Items.Add("a");
            a.Items.Add("b");
            a.Items.Add("target");

            ChainedLambdaCondition<A> condition = Condition.ChainedLambda(a,
                x => x.Items.FirstOrDefault() == "target", false);

            // Act & Assert - initially "a" is first, condition is false

            Assert.AreEqual(false, condition.Value);

            // Move "target" to first position
            a.Items.Move(2, 0);

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void CollectionFirstElementReplaceTest()
        {
            // Arrange

            A a = new A();
            a.Items.Add("initial");

            ChainedLambdaCondition<A> condition = Condition.ChainedLambda(a,
                x => x.Items.FirstOrDefault() == "target", false);

            Assert.AreEqual(false, condition.Value);

            // Act - replace first element using indexer

            a.Items[0] = "target";

            // Assert

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void CollectionContainsTest()
        {
            // Arrange

            A a = new A();
            ChainedLambdaCondition<A> condition = Condition.ChainedLambda(a,
                x => x.Items.Contains("target"), false);

            // Act & Assert

            Assert.AreEqual(false, condition.Value);

            a.Items.Add("other");
            Assert.AreEqual(false, condition.Value);

            a.Items.Add("target");
            Assert.AreEqual(true, condition.Value);

            a.Items.Remove("target");
            Assert.AreEqual(false, condition.Value);
        }

        [TestMethod]
        public void CollectionReplacementChainedTest()
        {
            // Arrange

            A a = new A();
            a.B.Items.Add("item");
            var oldB = a.B;
            ChainedLambdaCondition<A, B> condition = Condition.ChainedLambda(a, x => x.B, b => b.Items.Any(), false);

            Assert.AreEqual(true, condition.Value);

            // Act - replace B (new B has empty Items)

            a.B = new B();

            // Assert

            Assert.AreEqual(false, condition.Value);

            // Old B's collection changes should NOT trigger updates
            oldB.Items.Add("another");
            Assert.AreEqual(false, condition.Value);

            // New B's collection changes SHOULD trigger updates
            a.B.Items.Add("new item");
            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void CollectionPropertyReplacementChainedTest()
        {
            // Arrange

            A a = new A();
            a.Items.Add("item");
            var oldItems = a.Items;

            ChainedLambdaCondition<A> condition = Condition.ChainedLambda(a,
                x => x.Items.Contains("item"), false);

            Assert.AreEqual(true, condition.Value);

            // Act - replace the Items collection itself

            a.Items = new ObservableCollection<string>();

            // Assert

            Assert.AreEqual(false, condition.Value);

            // Old collection changes should NOT trigger updates
            oldItems.Add("something");
            Assert.AreEqual(false, condition.Value);

            // New collection changes SHOULD trigger updates
            a.Items.Add("item");
            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void CollectionNullIntermediateChainedTest()
        {
            // Arrange

            A a = new A();
            a.B = null;
            ChainedLambdaCondition<A, B> condition = Condition.ChainedLambda(a, x => x.B, b => b.Items.Any(), true);

            // Assert - defaultValue used because B is null

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void CollectionNullIntermediateRecoveryChainedTest()
        {
            // Arrange

            A a = new A();
            a.B = null;
            ChainedLambdaCondition<A, B> condition = Condition.ChainedLambda(a, x => x.B, b => b.Items.Any(), true);

            Assert.AreEqual(true, condition.Value);

            // Act - assign B and add items

            a.B = new B();
            Assert.AreEqual(false, condition.Value);

            a.B.Items.Add("item");
            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void CollectionClearTest()
        {
            // Arrange

            A a = new A();
            a.Items.Add("a");
            a.Items.Add("b");
            a.Items.Add("c");

            ChainedLambdaCondition<A> condition = Condition.ChainedLambda(a,
                x => x.Items.Contains("b"), false);

            Assert.AreEqual(true, condition.Value);

            // Act

            a.Items.Clear();

            // Assert

            Assert.AreEqual(false, condition.Value);
        }
    }
}
