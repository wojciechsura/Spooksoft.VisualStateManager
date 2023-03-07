using System;
using System.Collections.Generic;
using System.Text;

namespace Spooksoft.VisualStateManager.Conditions
{
    public class FalseCondition : BaseCondition
    {
        [Obsolete("Please use Condition.False instead.")]
        public FalseCondition()
        {

        }

        public override bool Value => false;
    }
}
