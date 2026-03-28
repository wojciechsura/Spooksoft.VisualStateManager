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
    public class LambdaConditionTests
    {
        [TestMethod]
        public void SimpleTest1()
        {
            // Arrange

            A a = new A();
            a.Prop1 = true;
            LambdaCondition<A> condition = Condition.Lambda(a, x => x.Prop1, false);

            // Assert

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void SimpleTest2()
        {
            // Arrange

            A a = new A();
            a.B.Prop1 = true;
            LambdaCondition<A> condition = Condition.Lambda(a, x => x.B.Prop1, false);

            // Assert

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void UpdateValueTest1()
        {
            // Arrange

            A a = new A();
            LambdaCondition<A> condition = Condition.Lambda(a, x => x.Prop1, false);
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
            LambdaCondition<A> condition = Condition.Lambda(a, x => x.B.Prop1, false);
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
            LambdaCondition<A> condition = Condition.Lambda(a, x => x.Prop1 && x.Prop2, false);
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
            LambdaCondition<A> condition = Condition.Lambda(a, x => x.B.C.Prop1 && x.B.Prop2, false);
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
            LambdaCondition<A> condition = Condition.Lambda(a, x => x.Prop3 > 10, false);
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
            LambdaCondition<A> condition = Condition.Lambda(a, x => x.B.Prop1, true);

            // Assert

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void NullTest2()
        {
            // Arrange

            A a = new A();
            a.B.C = null;
            LambdaCondition<A> condition = Condition.Lambda(a, x => x.B.Prop1 && x.B.C.Prop1, true);

            // Assert

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void ReplaceTest1()
        {
            // Arrange

            A a = new A();
            LambdaCondition<A> condition = Condition.Lambda(a, x => x.B.C.Prop1, false);
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
            LambdaCondition<A> condition = Condition.Lambda(a, x => x.B.Prop1 && x.B.C.Prop1, false);
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

            LambdaCondition<A> condition = Condition.Lambda(a, x => x.B.C.Prop1, false);

            // Act

            a.B = new B();

            // Assert

            Assert.AreEqual(false, oldB.HavePropertyChangeHandlers());
            Assert.AreEqual(false, oldB.C.HavePropertyChangeHandlers());
        }

        // Collection change tests -------------------------------------------

        [TestMethod]
        public void CollectionCountAddTest()
        {
            // Arrange

            A a = new A();
            LambdaCondition<A> condition = Condition.Lambda(a, x => x.Items.Count > 0, false);

            // Act & Assert

            Assert.AreEqual(false, condition.Value);

            a.Items.Add("item");

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void CollectionCountRemoveTest()
        {
            // Arrange

            A a = new A();
            a.Items.Add("item");
            LambdaCondition<A> condition = Condition.Lambda(a, x => x.Items.Count > 0, false);

            // Act & Assert

            Assert.AreEqual(true, condition.Value);

            a.Items.Clear();

            Assert.AreEqual(false, condition.Value);
        }

        [TestMethod]
        public void CollectionCountNotificationTest()
        {
            // Arrange

            A a = new A();
            LambdaCondition<A> condition = Condition.Lambda(a, x => x.Items.Count > 0, false);
            bool? notification = null;
            condition.PropertyChanged += (s, e) => { notification = condition.Value; };

            // Act

            a.Items.Add("item");

            // Assert

            Assert.AreEqual(true, condition.Value);
            Assert.AreEqual(true, notification);
        }

        [TestMethod]
        public void CollectionCountThresholdTest()
        {
            // Arrange

            A a = new A();
            LambdaCondition<A> condition = Condition.Lambda(a, x => x.Items.Count > 2, false);

            // Act & Assert

            a.Items.Add("1");
            Assert.AreEqual(false, condition.Value);

            a.Items.Add("2");
            Assert.AreEqual(false, condition.Value);

            a.Items.Add("3");
            Assert.AreEqual(true, condition.Value);

            a.Items.RemoveAt(0);
            Assert.AreEqual(false, condition.Value);
        }

        [TestMethod]
        public void NestedCollectionCountTest()
        {
            // Arrange

            A a = new A();
            LambdaCondition<A> condition = Condition.Lambda(a, x => x.B.Items.Count > 0, false);

            // Act & Assert

            Assert.AreEqual(false, condition.Value);

            a.B.Items.Add("item");

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void CollectionReplacementTest()
        {
            // Arrange

            A a = new A();
            a.Items.Add("item1");
            var oldItems = a.Items;
            LambdaCondition<A> condition = Condition.Lambda(a, x => x.Items.Count > 0, false);

            Assert.AreEqual(true, condition.Value);

            // Act - replace collection with a new empty one

            a.Items = new ObservableCollection<string>();

            // Assert - condition should now be false (new collection is empty)

            Assert.AreEqual(false, condition.Value);

            // Adding to old collection should NOT update the condition
            oldItems.Add("item2");
            Assert.AreEqual(false, condition.Value);

            // Adding to new collection SHOULD update the condition
            a.Items.Add("item3");
            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void CollectionNullIntermediateTest()
        {
            // Arrange

            A a = new A();
            a.B = null;
            LambdaCondition<A> condition = Condition.Lambda(a, x => x.B.Items.Count > 0, true);

            // Assert - defaultValue used because B is null

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void CollectionNullIntermediateRecoveryTest()
        {
            // Arrange

            A a = new A();
            a.B = null;
            LambdaCondition<A> condition = Condition.Lambda(a, x => x.B.Items.Count > 0, true);

            Assert.AreEqual(true, condition.Value);

            // Act - assign B, then add items

            a.B = new B();
            Assert.AreEqual(false, condition.Value);

            a.B.Items.Add("item");
            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void NestedCollectionReplacementViaIntermediateTest()
        {
            // Arrange

            A a = new A();
            a.B.Items.Add("item");
            var oldB = a.B;
            LambdaCondition<A> condition = Condition.Lambda(a, x => x.B.Items.Count > 0, false);

            Assert.AreEqual(true, condition.Value);

            // Act - replace B (new B has empty Items)

            a.B = new B();

            // Assert

            Assert.AreEqual(false, condition.Value);

            // Old B's collection changes should NOT trigger updates
            oldB.Items.Add("another");
            Assert.AreEqual(false, condition.Value);

            // New B's collection changes should trigger updates
            a.B.Items.Add("new item");
            Assert.AreEqual(true, condition.Value);
        }
    }
}
