using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.Api.Models;
using Routine.Api.Services;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
using Routine.Api.DtoParameters;
using Routine.Api.Entities;

namespace Routine.Api.Controllers
{
    [ApiController]//当modelstate验证出错时候，自动返回客户端400
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
        [HttpHead]//不返回body其他和httpget一致
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies(CompanyDtoParameters parameters)
        {
            var companies = await this.companyRepository.GetCompaniesAsync(parameters);
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
        [HttpGet("{companyid}",Name =nameof(GetCompany))]
        public async Task<ActionResult<CompanyDto>> GetCompany(Guid companyid)
        {
            if (!await this.companyRepository.CompanyExitsAsync(companyid))
                return NotFound();
            var company = await this.companyRepository.GetCompanyAsync(companyid);
            return Ok(this.mapper.Map<CompanyDto>(company));
        }
        [HttpPost]
        public async Task<ActionResult<CompanyDto>> CreateCompany(CompanyAddDto companyAddDto)
        {
            var entity = this.mapper.Map<Company>(companyAddDto);
            this.companyRepository.AddCompany(entity);
            await this.companyRepository.SaveAsync();

            var returnDto = this.mapper.Map<CompanyDto>(entity);
            return CreatedAtRoute(nameof(GetCompany),new { companyid= returnDto.Id }, returnDto);

        }
        [HttpOptions]
        public IActionResult GetCompanyOption()
        {
            Response.Headers.Add("Allow","GET,POST,OPTIONS");
            return Ok();
        }
    }
}
