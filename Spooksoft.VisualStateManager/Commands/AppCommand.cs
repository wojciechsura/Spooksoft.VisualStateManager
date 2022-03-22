using Spooksoft.VisualStateManager.Conditions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Spooksoft.VisualStateManager.Commands
{
    public class AppCommand : ICommand
    {
        private readonly BaseCondition condition;
        private readonly Action<object> action;

        private void HandleConditionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BaseCondition.Value))
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public AppCommand(Action<object> action, BaseCondition condition = null)
        {
            this.action = action;
            this.condition = condition;
            if (condition != null)
                condition.PropertyChanged += HandleConditionPropertyChanged;
        }

        public bool CanExecute(object parameter)
        {
            return condition?.Value ?? true;
        }

        public void Execute(object parameter)
        {
            action(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}
