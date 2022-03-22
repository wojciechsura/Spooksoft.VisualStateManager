using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spooksoft.VisualStateManager.Conditions
{
    public class SimpleCondition : BaseSimpleCondition
    {
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
