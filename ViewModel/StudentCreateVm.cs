using MyASP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyASP.ViewModel
{
    /*
         1model 验证等
          
     */
    public class StudentCreateVm
    {
        [Display(Name = "名")]
        [Required]
        public string firstname { get; set; }

        [Display(Name = "姓"), Required, MaxLength(10)]
        public string lastname { get; set; }

        [Display(Name = "出生日期")]
        public DateTime birthdate { get; set; }

        [Display(Name = "性别")]
        public Gender gender { get; set; }
    }
}
