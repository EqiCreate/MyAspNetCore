using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyASP
{
    interface Interfacetest
    {
        void Configure(string app);
    }

    public class intertest : Interfacetest
    {
        public void Configure(string app)
        {
            throw new NotImplementedException();
        }
    }
}
