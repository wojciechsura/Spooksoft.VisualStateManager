using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spooksoft.VisualStateManager.Conditions.Expressions
{
    internal interface IChainedExpressionHandler<TInput>
        where TInput : class, INotifyPropertyChanged
    {
        void NotifySourceChanged(TInput newSource);
    }
}
