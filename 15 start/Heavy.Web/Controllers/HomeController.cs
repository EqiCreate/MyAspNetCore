using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Heavy.Web.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace Heavy.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IHttpContextAccessor httpContextAccessor;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            this.logger = logger;
            this.httpContextAccessor = httpContextAccessor??throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        [ResponseCache(CacheProfileName = "Default")]
        public IActionResult Index()
        {
           var testcookie= Request.Cookies;
            string cookie = Request.Cookies["Key"];
            Response.Cookies.Append("asdf1", "testcookie");
            this.httpContextAccessor.HttpContext.Response.Cookies.Append("asdf","testcookie",new CookieOptions() { Expires=DateTime.Now.AddSeconds(10)});
            this.logger.LogInformation(1000, "===开始记录测试===");
            var cookiereadkey = this.httpContextAccessor.HttpContext.Request.Cookies["asdf"];
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
