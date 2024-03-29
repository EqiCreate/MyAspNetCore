﻿using System;
namespace Routine.Api.Models
{
    public class CompanyFullDto
    {
        public Guid Id { get; set; }
        public string Country { get; set; }
        public string Industry { get; set; }
        public string Product { get; set; }
        public string Introduction { get; set; }
        public string Name { get; set; }
        public DateTime? BankrupTime { get; set; }
    }
}
