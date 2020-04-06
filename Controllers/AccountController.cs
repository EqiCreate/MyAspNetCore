using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyASP.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyASP.Controllers
{
    /*
     1 涉及到SignInManager和UserManager的使用
     2  涉及到注入以及startup的UseAuthentication等
     3 涉及到多个数据库的迁移dbcontext
     4 js的特殊写法如href="javascript代码......"
     5 task 的使用等
         */
    public class AccountController:Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public async  Task< IActionResult> Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var user = await _userManager.FindByNameAsync(viewModel.Username);
            if (user != null)
            {
                var res = await _signInManager.PasswordSignInAsync(user,viewModel.Password,false,false);
                if (res.Succeeded)
                {
                    return RedirectToAction("Index", "Home");

                }
            }
            ModelState.AddModelError(string.Empty,"密码或用户名错误");
            return View(viewModel);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                var user = new IdentityUser()
                {
                    UserName=viewModel.Username,
                };
                var res = await _userManager.CreateAsync(user,viewModel.Password);
                if (res.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

            }
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
