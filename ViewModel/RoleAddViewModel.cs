using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MyASP.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace MyASP.ViewModel
{
    public class RoleAddViewModel
    {
        [Required]
        [Display(Name = "角色名称")]
        //[Remote(nameof(RoleController.CheckRoleExist), "Role", ErrorMessage = "角色已存在")]
        public string RoleName { get; set; }
    }
}
