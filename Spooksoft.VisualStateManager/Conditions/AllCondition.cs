using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;

namespace Spooksoft.VisualStateManager.Conditions
{
    public class AllCondition<T> : BaseAggregateCondition<T>
        where T : class
    {
        private static bool EvalValue(IEnumerable<BaseCondition> conditions)
        {
            bool newValue = true;

            foreach (var condition in conditions)
            {
                if (!condition.Value)
                {
                    newValue = false;
                    break;
                }
            }

            return newValue;
        }

        [Obsolete("Please use Condition.All instead.")]
        public AllCondition(ObservableCollection<T> collection, Func<T, BaseCondition> conditionExtractor, bool valueIfEmpty = false)
            : base(collection, 
                  conditionExtractor, 
                  EvalValue,
                  valueIfEmpty)
        {
            
        }
    }
}
