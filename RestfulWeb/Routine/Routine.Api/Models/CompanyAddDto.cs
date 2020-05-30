using Routine.Api.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Models
{
    public class CompanyAddDto
    {
        [Display(Name="公司名称")]
        [Required(ErrorMessage ="{0} is needed")]//防止浏览器判断当用户输入name为空值导致报错500
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Introduction { get; set; }
        public ICollection<EmployeeAddDto> Employees { get; set; } = new List<EmployeeAddDto>();
    }
}
