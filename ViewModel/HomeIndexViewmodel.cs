using MyASP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyASP.ViewModel
{
    public class HomeIndexViewmodel
    {
        public IEnumerable<StudentViewModel> Students { get; set; }
    }
}
