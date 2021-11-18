using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Spooksoft.VisualStateManager.Test.Classes
{
    public class BaseClass : INotifyPropertyChanged
    {
        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        protected void InternalSet<T>(ref T field, string property, T value, bool force = false)
        {
            if (!Equals(field, value) || force)
            {
                field = value;
                OnPropertyChanged(property);
            }
        }

        protected void Set<T>(ref T field, T value, [CallerMemberName] string property = null, bool force = false)
        {
            if (!Equals(field, value) || force)
            {
                field = value;
                OnPropertyChanged(property);
            }
        }

        protected void Set<T>(ref T field, T value, Action changeHandler, [CallerMemberName] string property = null, bool force = false)
        {
            if (!Equals(field, value) || force)
            {
                field = value;
                OnPropertyChanged(property);
                changeHandler?.Invoke();
            }
        }

        protected void Set<T>(ref T field, T value, Action changeHandler, Action beforeChangeHandler, [CallerMemberName] string property = null, bool force = false)
        {
            if (!Equals(field, value) || force)
            {
                beforeChangeHandler?.Invoke();
                field = value;
                OnPropertyChanged(property);
                changeHandler?.Invoke();
            }
        }

        public bool HavePropertyChangeHandlers()
        {
            return PropertyChanged?.GetInvocationList().Any() ?? false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
