using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spooksoft.VisualStateManager.Conditions
{
    public class SimpleCondition : BaseSimpleCondition
    {
        [Obsolete("Please use Condition.Simple instead.")]
        public SimpleCondition(bool newValue = false) 
            : base(newValue)
        {

        }

        public new bool Value
        {
            get => base.Value;
            set => SetValue(value);
        }
    }
}
