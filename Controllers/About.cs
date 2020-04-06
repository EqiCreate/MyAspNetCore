using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyASP.Controllers
{
    [Route("[controller]")]
    public class About : Controller
    {
        [Route("")]
        public string www()
        {
            return "abv=out";
        }
    }
}
