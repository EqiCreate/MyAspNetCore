using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.Api.Entities;
using Routine.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class IdentityFileController:ControllerBase
    {
        private readonly IMapper mapper;
        public IdentityFileController(IMapper mapper)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet("{id}", Name = nameof(GetCertById))]
        public async Task<IActionResult> GetCertById(int id)
        {
            await Task.Delay(1);
            return Ok(id);
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await Task.Delay(10);

            return Ok("ASDF");
        }
       
        [HttpPost]
        public  IActionResult UploadCert([FromForm]CertAddDto certAddDto)
        {
            try
            {
                var filefullPath = Path.Combine(Directory.GetCurrentDirectory(), "copy" + certAddDto.File.FileName);
              // var fs = new FileStream(filefullPath, FileMode.Create);
                using (var fs = new FileStream(filefullPath, FileMode.Create))
                {
                    certAddDto.File.CopyTo(fs);
                    fs.Flush();
                }

                //await Task.Run(() => {

                //});

                return Ok();
                //return CreatedAtRoute(nameof(GetCertById), new { id = certAddDto.CertId });
            }
            catch(Exception ex) { 
                throw;
            }
        }
    }
}
