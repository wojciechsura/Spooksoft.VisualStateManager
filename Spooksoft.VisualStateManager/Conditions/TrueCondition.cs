using System;
using System.Collections.Generic;
using System.Text;

namespace Spooksoft.VisualStateManager.Conditions
{
    public class TrueCondition : BaseCondition
    {
        [Obsolete("Please use Condition.True instead.")]
        public TrueCondition()
        {

        }

        public override bool Value => true;
    }
}
