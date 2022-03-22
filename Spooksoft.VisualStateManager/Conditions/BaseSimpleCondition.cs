using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spooksoft.VisualStateManager.Conditions
{
    public class BaseSimpleCondition : BaseCondition
    {
        // Private fields ----------------------------------------------------

        protected internal bool value;

        // Private methods ---------------------------------------------------

        protected internal void SetValue(bool value)
        {
            if (value == this.value)
                return;

            this.value = value;
            OnValueChanged();
        }

        // Public methods ----------------------------------------------------

        public BaseSimpleCondition(bool newValue = false)
        {
            value = newValue;
        }

        // Public properties -------------------------------------------------

        public override sealed bool Value => value;
    }
}
