using Routine.Api.Entities;
using Routine.Api.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Routine.Api.Models
{
    [EmployeeNoMustDifferentFromFirstName]
    public abstract class EmployeeAddOrUpdateDto : IValidatableObject
    {
        [Required(ErrorMessage = "{0}必填项")]
        [Display(Name = "员工号")]
        [StringLength(10, MinimumLength = 10)]
        public string EmployeeNo { get; set; }

        [Required(ErrorMessage = "{0}必填项")]
        [Display(Name = "名")]
        public string FirstName { get; set; }
        [Display(Name = "姓")]
        public string LastName { get; set; }
        [Display(Name = "性别")]
        public Gender Gender { get; set; }
        [Display(Name = "生日")]
        public DateTime DateofBirth { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FirstName == LastName)
            {
                yield return new ValidationResult("姓名不可相等", new[] { nameof(FirstName), nameof(LastName) });
            }
        }
    }
}
