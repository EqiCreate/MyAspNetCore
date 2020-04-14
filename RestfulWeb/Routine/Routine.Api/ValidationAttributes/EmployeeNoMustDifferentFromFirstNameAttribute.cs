using Routine.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace Routine.Api.ValidationAttributes
{
    public class EmployeeNoMustDifferentFromFirstNameAttribute:ValidationAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">特性加载clr属性有值，类无值</param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var adddtos = (EmployeeAddDto)validationContext.ObjectInstance;
            if (adddtos.EmployeeNo == adddtos.FirstName)
            {
                return new ValidationResult("员工编号不可以与员工名相同",new [] { nameof(EmployeeAddDto) });
            }
            return ValidationResult.Success;
        }
    }
}
