﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spooksoft.VisualStateManager.Conditions
{
    public class CompositeCondition : BaseCondition
    {
        // Private fields ------------------------------------------------------

        private readonly CompositionKind compositionKind;
        private readonly List<BaseCondition> conditions;
        private bool value;

        // Private methods -----------------------------------------------------

        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BaseCondition.Value))
                ReevaluateValue();
        }

        private bool InternalEvalValue()
        {
            if (conditions.Count == 0)
                return false;
            else
            {
                switch (compositionKind)
                {
                    case CompositionKind.And:
                        {
                            bool result = true;

                            int i = 0;
                            while (i < conditions.Count && result)
                                result &= conditions[i++].Value;

                            return result;
                        }
                    case CompositionKind.Or:
                        {
                            bool result = false;

                            int i = 0;
                            while (i < conditions.Count && !result)
                                result |= conditions[i++].Value;

                            return result;
                        }
                    default:
                        throw new InvalidEnumArgumentException("Unsupported composition kind!");
                }
            }
        }

        private void ReevaluateValue()
        {
            bool newValue = InternalEvalValue();

            if (newValue != value)
            {
                value = newValue;
                OnValueChanged();
            }
        }

        // Public types --------------------------------------------------------

        public enum CompositionKind
        {
            And,
            Or
        }

        // Public methods ------------------------------------------------------

        [Obsolete("Please use Condition.Composite instead.")]
        public CompositeCondition(CompositionKind kind = CompositionKind.And)
        {
            compositionKind = kind;
            conditions = new List<BaseCondition>();
            value = false;
        }

        [Obsolete("Please use Condition.Composite instead.")]
        public CompositeCondition(CompositionKind kind,
            params BaseCondition[] compositeConditions)
        {
            compositionKind = kind;
            conditions = new List<BaseCondition>();
            value = false;

            foreach (BaseCondition condition in compositeConditions)
            {
                Add(condition);
            }
        }

        public void Add(BaseCondition condition)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");

            if (!conditions.Contains(condition))
            {
                condition.PropertyChanged += HandlePropertyChanged;
                conditions.Add(condition);

                ReevaluateValue();
            }
        }

        public void Remove(BaseCondition condition)
        {
            if (conditions.Contains(condition))
            {
                condition.PropertyChanged -= HandlePropertyChanged;
                conditions.Remove(condition);

                ReevaluateValue();
            }
        }

        public override bool Value => value;
    }
}
