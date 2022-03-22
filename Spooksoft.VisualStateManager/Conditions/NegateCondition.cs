using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spooksoft.VisualStateManager.Conditions
{
    public class NegateCondition : BaseCondition
    {
        // Private fields ----------------------------------------------------

        private readonly BaseCondition condition;

        // Private methods ---------------------------------------------------

        private void HandleInnerConditionValueChanged(object sender, PropertyChangedEventArgs e)
        { 
            if (e.PropertyName == nameof(BaseCondition.Value))
                OnValueChanged();
        }

        // Public methods ----------------------------------------------------

        public NegateCondition(BaseCondition newCondition)
        {
            condition = newCondition ?? throw new ArgumentNullException("newCondition");
            condition.PropertyChanged += HandleInnerConditionValueChanged;
        }

        public override bool Value => !condition.Value;
    }
}
