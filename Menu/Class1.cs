using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;

namespace Menu
{
    internal class Class1: Window1
    { 
        public Class1()
        {
            this.InitializeComponent();
            Enemy_Spawn(1);
        }
    }
}
