using Microsoft.AspNetCore.Mvc;
using MyASP.Model;
using MyASP.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyASP.ViewComponents
{
    /*
     viewcomponent:迷你mvc
         */
    public class WelcomeStudentViewComponent:ViewComponent
    {
        private readonly ImodelEntity<Student> _imodelEntity;

        public WelcomeStudentViewComponent(ImodelEntity<Student>imodelEntity)
        {
            this._imodelEntity = imodelEntity;
        }

        public  IViewComponentResult Invoke()
        {
            var count =  this._imodelEntity.getall().Count().ToString();
            //TODO 不能直接return view（count），因为string类型的可能会被识别成char
            return View("Default", count);
        }
    }
}
