using System;

namespace Routine.Api.Models
{
    public class CompanyAddWithBankrupDto:CompanyAddDto
    {
        public DateTime? BankrupTime { get; set; }
    }
}
