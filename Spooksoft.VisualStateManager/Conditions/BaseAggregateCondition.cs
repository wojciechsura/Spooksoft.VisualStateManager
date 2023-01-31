using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace Spooksoft.VisualStateManager.Conditions
{
    public class BaseAggregateCondition<T> : BaseCollectionCondition<T>
        where T : class
    {
        // Private fields -----------------------------------------------------

        private readonly Func<T, BaseCondition> conditionExtractor;
        private readonly Dictionary<T, BaseCondition> conditions;
        private readonly Func<IEnumerable<BaseCondition>, bool> evalMethod;
        private readonly bool valueIfEmpty;
        private bool value;

        // Private methods ----------------------------------------------------

        private void HandleConditionValueChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BaseCondition.Value))
                Update();
        }

        private void DoRemoveAllConditions()
        {
            foreach (var condition in conditions.Values)
            {
                condition.PropertyChanged -= HandleConditionValueChanged;
            }
            conditions.Clear();
        }

        private void DoBuildConditions()
        {
            foreach (var item in collection)
            {
                DoAddCondition(item);
            }
        }

        private void DoAddCondition(T item)
        {
            var condition = conditionExtractor(item);
            if (condition == null)
                throw new InvalidOperationException("No condition was supplied for collection item!");
            condition.PropertyChanged += HandleConditionValueChanged;

            conditions[item] = condition;
        }

        private void DoRemoveCondition(T item)
        {
            if (conditions.TryGetValue(item, out BaseCondition condition))
                condition.PropertyChanged -= HandleConditionValueChanged;

            conditions.Remove(item);
        }

        // Protected methods --------------------------------------------------

        protected void Update()
        {
            bool newValue = conditions.Any() ? evalMethod(conditions.Values) : valueIfEmpty;

            if (newValue != value)
            {
                value = newValue;
                OnValueChanged();
            }
        }

        protected override void PartialAllItemsRemoved()
        {
            DoRemoveAllConditions();
            DoBuildConditions();
        }

        protected override void PartialItemAdded(T item)
        {
            DoAddCondition(item);
        }

        protected override void PartialItemRemoved(T item)
        {
            DoRemoveCondition(item);
        }

        protected override void AfterCollectionChanged()
        {
            Update();
        }

        // Public methods -----------------------------------------------------

        public BaseAggregateCondition(ObservableCollection<T> collection, 
            Func<T, BaseCondition> conditionExtractor,
            Func<IEnumerable<BaseCondition>, bool> evalMethod,
            bool valueIfEmpty = false)
            : base(collection)
        {
            this.conditionExtractor = conditionExtractor ?? throw new ArgumentNullException(nameof(conditionExtractor));
            this.evalMethod = evalMethod ?? throw new ArgumentNullException(nameof(evalMethod));
            this.valueIfEmpty = valueIfEmpty;
            this.value = false;

            conditions = new Dictionary<T, BaseCondition>();
            DoBuildConditions();
            Update();
        }

        public override sealed bool Value => value;
    }
}
