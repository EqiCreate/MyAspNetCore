using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyASP.Model;
using MyASP.Service;
using MyASP.ViewModel;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

    /*redirection 重定向 防止刷新导致增加多余数据*/
namespace MyASP.Controllers
{
    public class HomeController : Controller
    {
        private ImodelEntity<Student> _resposity;
        public IActionResult Index()
        {
            var res = _resposity.getall();
            var vms = res.Select(x=>new StudentViewModel()
            {
                Name=x.firstname+x.lastname,
                Age=DateTime.Now.Subtract(x.birthdate).Days/365
            });
            var vm = new HomeIndexViewmodel()
            {
                Students=vms
            };
            return View(vm);
        }
        public HomeController(ImodelEntity<Student> resposity)
        {
            _resposity = resposity;
        }
        public IActionResult Detail(int id=1)
        {
            var student = _resposity.getbyid(id);
            return View(student);
        }

        //authorize 授权才能进行访问
        //[HttpGet,Authorize]
        [HttpGet][Authorize]
        public IActionResult Create(int id = 0)
        {
            return View();
        }

        /*
         * 1 针对post防止跨站token伪造 => [ValidateAntiForgeryToken]
         */
         //[Authorize]
        //[HttpPost,ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(StudentCreateVm s)
        {
            if (ModelState.IsValid)
            {
                var student = new Student()
                {
                    birthdate = s.birthdate,
                    firstname = s.firstname,
                    lastname = s.lastname,
                    gender = s.gender
                };
                var newmodel = _resposity?.Add(student);
                return RedirectToAction(nameof(Detail), new { id = newmodel.id });
            }
            else {
                return View();//返回到create 视图
            }
          
        }
    }
}
