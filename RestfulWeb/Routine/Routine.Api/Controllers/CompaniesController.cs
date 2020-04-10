using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.Api.Models;
using Routine.Api.Services;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Routine.Api.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository companyRepository;
        private readonly IMapper mapper;

        public CompaniesController(ICompanyRepository companyRepository,IMapper mapper)
        {
            this.companyRepository = companyRepository??throw new ArgumentException(nameof(companyRepository));
            this.mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }
       //[HttpGet]
       // public async Task<IActionResult> GetCompanies() 
       // {
       //     var companies = await this.companyRepository.GetCompaniesAsync();
       //     var companiesDto = new List<CompanyDto>();
       //     foreach (var company in companies)
       //     {
       //         companiesDto.Add(new CompanyDto()
       //         { 
       //             Id=company.Id,
       //             Name=company.Name
       //         });
       //     }
       //     return Ok(companiesDto);
       // }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies()
        {
            var companies = await this.companyRepository.GetCompaniesAsync();
            // var companiesDto = new List<CompanyDto>();
            //foreach (var company in companies)
            //{
            //    companiesDto.Add(new CompanyDto()
            //    {
            //        Id = company.Id,
            //        CompanyName = company.Name
            //    });
            //}
            var companiesDto = this.mapper.Map<IEnumerable<CompanyDto>>(companies);
            return Ok(companiesDto);
        }
        [HttpGet("{companyid}")]
        public async Task<ActionResult<CompanyDto>> GetCompany(Guid companyid)
        {
            if (!await this.companyRepository.CompanyExitsAsync(companyid))
                return NotFound();
            var company = await this.companyRepository.GetCompanyAsync(companyid);
            return Ok(this.mapper.Map<CompanyDto>(company));
        }
    }
}
