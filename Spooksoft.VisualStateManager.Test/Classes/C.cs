using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spooksoft.VisualStateManager.Test.Classes
{
    public class C : BaseClass
    {
        private bool field1;
        private bool field2;

        public bool Prop1
        {
            get => field1;
            set => Set(ref field1, value);
        }

        public bool Prop2
        {
            get => field2;
            set => Set(ref field2, value);
        }
    }
}
