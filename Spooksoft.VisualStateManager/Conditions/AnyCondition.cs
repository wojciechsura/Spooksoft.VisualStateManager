using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;

namespace Spooksoft.VisualStateManager.Conditions
{
    public class AnyCondition<T> : BaseAggregateCondition<T>
        where T : class
    {
        private static bool EvalValue(IEnumerable<BaseCondition> conditions)
        {
            bool newValue = false;

            foreach (var condition in conditions)
            {
                if (condition.Value)
                {
                    newValue = true;
                    break;
                }
            }

            return newValue;
        }

        public AnyCondition(ObservableCollection<T> collection, Func<T, BaseCondition> conditionExtractor, bool valueIfEmpty = false)
            : base(collection, 
                  conditionExtractor, 
                  EvalValue,
                  valueIfEmpty)
        {

        }
    }
}
