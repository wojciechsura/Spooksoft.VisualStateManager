using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spooksoft.VisualStateManager.Conditions
{
    public abstract class BaseCondition : INotifyPropertyChanged
    {
        protected void OnValueChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        }

        public static BaseCondition operator &(BaseCondition first, BaseCondition second) =>
            new CompositeCondition(CompositeCondition.CompositionKind.And, first, second);

        public static BaseCondition operator |(BaseCondition first, BaseCondition second) =>
            new CompositeCondition(CompositeCondition.CompositionKind.Or, first, second);

        public static BaseCondition operator !(BaseCondition condition) =>
            new NegateCondition(condition);

        public abstract bool Value
        {
            get;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
