using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Models
{
    public class CertAddDto
    {
        public int CertId { get; set; }
        public IFormFile File { get; set; }
    }
}
