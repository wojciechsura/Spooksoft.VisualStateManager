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
    public class AggregateConditionTests
    {
        [TestMethod]
        public void EmptyAllConditionTest()
        {
            // Arrange

            var c = new ObservableCollection<ConditionClass>();
            var condition = new AllCondition<ConditionClass>(c, x => x.SimpleCondition, true);

            // Assert

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void EmptyAnyConditionTest()
        {
            // Arrange

            var c = new ObservableCollection<ConditionClass>();
            var condition = new AnyCondition<ConditionClass>(c, x => x.SimpleCondition, true);

            // Assert

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void TrueAllConditionTest()
        {
            // Arrange

            var c = new ObservableCollection<ConditionClass>();
            var condition = new AllCondition<ConditionClass>(c, x => x.SimpleCondition, false);
            
            for (int i = 0; i < 10; i++)
            {
                var cls = new ConditionClass();
                cls.SimpleCondition.Value = true;
                c.Add(cls);
            }

            // Assert

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void TrueAnyConditionTest()
        {
            // Arrange

            var c = new ObservableCollection<ConditionClass>();
            var condition = new AnyCondition<ConditionClass>(c, x => x.SimpleCondition, false);

            for (int i = 0; i < 10; i++)
            {
                var cls = new ConditionClass();
                cls.SimpleCondition.Value = i == 4;
                c.Add(cls);
            }

            // Assert

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void FalseAllConditionTest()
        {
            // Arrange

            var c = new ObservableCollection<ConditionClass>();
            var condition = new AllCondition<ConditionClass>(c, x => x.SimpleCondition, false);

            for (int i = 0; i < 10; i++)
            {
                var cls = new ConditionClass();
                cls.SimpleCondition.Value = i != 5;
                c.Add(cls);
            }

            // Assert

            Assert.AreEqual(false, condition.Value);
        }

        [TestMethod]
        public void FalseAnyConditionTest()
        {
            // Arrange

            var c = new ObservableCollection<ConditionClass>();
            var condition = new AnyCondition<ConditionClass>(c, x => x.SimpleCondition, false);

            for (int i = 0; i < 10; i++)
            {
                var cls = new ConditionClass();
                cls.SimpleCondition.Value = false;
                c.Add(cls);
            }

            // Assert

            Assert.AreEqual(false, condition.Value);
        }

        [TestMethod]
        public void AllConditionValueChangeTest()
        {
            // Arrange

            var c = new ObservableCollection<ConditionClass>();
            var condition = new AllCondition<ConditionClass>(c, x => x.SimpleCondition, false);

            for (int i = 0; i < 10; i++)
            {
                var cls = new ConditionClass();
                cls.SimpleCondition.Value = i != 5;
                c.Add(cls);
            }

            // Act

            c[5].SimpleCondition.Value = true;

            // Assert

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void AnyConditionValueChangeTest()
        {
            // Arrange

            var c = new ObservableCollection<ConditionClass>();
            var condition = new AnyCondition<ConditionClass>(c, x => x.SimpleCondition, false);

            for (int i = 0; i < 10; i++)
            {
                var cls = new ConditionClass();
                cls.SimpleCondition.Value = false;
                c.Add(cls);
            }

            // Act

            c[5].SimpleCondition.Value = true;

            // Assert

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void AllConditionCollectionChangeTest()
        {
            // Arrange

            var c = new ObservableCollection<ConditionClass>();
            var condition = new AllCondition<ConditionClass>(c, x => x.SimpleCondition, false);

            for (int i = 0; i < 10; i++)
            {
                var cls = new ConditionClass();
                cls.SimpleCondition.Value = true;
                c.Add(cls);
            }

            // Act

            var cls1 = new ConditionClass();
            cls1.SimpleCondition.Value = false;
            c.Add(cls1);

            // Assert

            Assert.AreEqual(false, condition.Value);
        }

        [TestMethod]
        public void AnyConditionCollectionChangeTest()
        {
            // Arrange

            var c = new ObservableCollection<ConditionClass>();
            var condition = new AnyCondition<ConditionClass>(c, x => x.SimpleCondition, false);

            for (int i = 0; i < 10; i++)
            {
                var cls = new ConditionClass();
                cls.SimpleCondition.Value = i == 5;
                c.Add(cls);
            }

            // Act

            c.RemoveAt(5);

            // Assert

            Assert.AreEqual(false, condition.Value);
        }

        [TestMethod]
        public void AllConditionCollectionChangeAfterRemovalTest()
        {
            // Arrange

            var c = new ObservableCollection<ConditionClass>();
            var condition = new AllCondition<ConditionClass>(c, x => x.SimpleCondition, false);

            for (int i = 0; i < 10; i++)
            {
                var cls = new ConditionClass();
                cls.SimpleCondition.Value = true;
                c.Add(cls);
            }

            // Act

            var cls1 = c[0];
            c.RemoveAt(0);
            cls1.SimpleCondition.Value = false;

            // Assert

            Assert.AreEqual(true, condition.Value);
        }

        [TestMethod]
        public void AnyConditionCollectionChangeAfterRemovalTest()
        {
            // Arrange

            var c = new ObservableCollection<ConditionClass>();
            var condition = new AnyCondition<ConditionClass>(c, x => x.SimpleCondition, false);

            for (int i = 0; i < 10; i++)
            {
                var cls = new ConditionClass();
                cls.SimpleCondition.Value = false;
                c.Add(cls);
            }

            // Act

            var cls1 = c[0];
            c.RemoveAt(0);
            cls1.SimpleCondition.Value = true;

            // Assert

            Assert.AreEqual(false, condition.Value);
        }

        [TestMethod]
        public void DynamicAllCollectionTest()
        {
            // Arrange

            var c = new ObservableCollection<C>();
            for (int i = 0; i < 10; i++)
                c.Add(new C()
                {
                    Prop1 = true
                });

            var condition = new AllCondition<C>(c, item => new PropertyWatchCondition<C>(item, x => x.Prop1, false));

            // Assert

            Assert.AreEqual(condition.Value, true);
        }

        [TestMethod]
        public void DynamicAnyCollectionTest()
        {
            // Arrange

            var c = new ObservableCollection<C>();
            for (int i = 0; i < 10; i++)
                c.Add(new C()
                {
                    Prop1 = false
                });

            c[0].Prop1 = true;

            var condition = new AnyCondition<C>(c, item => new PropertyWatchCondition<C>(item, x => x.Prop1, false));

            // Assert

            Assert.AreEqual(condition.Value, true);
        }
    }
}
