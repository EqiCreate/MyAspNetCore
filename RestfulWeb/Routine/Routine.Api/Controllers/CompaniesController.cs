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
using Routine.Api.Helpers;
using System.Text.Json;
using System.Text.Encodings.Web;

namespace Routine.Api.Controllers
{
    [ApiController]//当modelstate验证出错时候，自动返回客户端400
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository companyRepository;
        private readonly IMapper mapper;
        private readonly IPropertyMappingService propertyMappingService;

        public CompaniesController(ICompanyRepository companyRepository,IMapper mapper, IPropertyMappingService propertyMappingService)
        {
            this.companyRepository = companyRepository??throw new ArgumentException(nameof(companyRepository));
            this.mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            this.propertyMappingService = propertyMappingService ?? throw new ArgumentException(nameof(propertyMappingService));
        }
        [HttpGet(Name =nameof(GetCompanies))]
        [HttpHead]//不返回body其他和httpget一致
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies([FromQuery]CompanyDtoParameters parameters)
        {
            if (!this.propertyMappingService.ValidMappingExitFor<CompanyDto, Company>(parameters.OrderBy)) return BadRequest();

            var companies = await this.companyRepository.GetCompaniesAsync(parameters);
            #region 分页增加自描述信息
            var previousPagelink = companies.HasPrevious ? this.CreateCompaniesResourceUri(parameters,ResourceUriType.PreviousPage):null;
            var nextPagelink = companies.HasNext ? this.CreateCompaniesResourceUri(parameters,ResourceUriType.NextPage):null;
            var paginationMetedata = new 
            {
                totalCount=companies.TotalCount,
                pageSize=companies.PageSize,
                currentPage=companies.CurrentPage,
                totalPages = companies.TotalPages,
                previousPagelink,
                nextPagelink
            };
            this.Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetedata, new JsonSerializerOptions() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }));
            #endregion
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

        [HttpDelete("{companyid}")]
        public async Task<IActionResult> DeleteCompany(Guid companyId)
        {
            var companyEntity = await this.companyRepository.GetCompanyAsync(companyId);
            if (companyEntity == null) return NotFound();
            else 
            {
                await this.companyRepository.GetEmployeesAsync(companyId,null);//很奇怪，需要从内存加载出来才可以删除级联
                this.companyRepository.DeleteCompany(companyEntity);
                await this.companyRepository.SaveAsync();
                return NoContent();
            }
        }

        private string CreateCompaniesResourceUri(CompanyDtoParameters Parameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetCompanies),new { orderBy=Parameters.OrderBy, pageNumber=Parameters.PageNumber-1,pageSize=Parameters.PageSize, companyName=Parameters.CompanyName, searchTerm=Parameters.SearchTerm });
                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetCompanies), new { orderBy = Parameters.OrderBy, pageNumber = Parameters.PageNumber + 1, pageSize = Parameters.PageSize, companyName = Parameters.CompanyName, searchTerm = Parameters.SearchTerm });
                default:
                    return Url.Link(nameof(GetCompanies), new { orderBy = Parameters.OrderBy, pageNumber = Parameters.PageNumber , pageSize = Parameters.PageSize, companyName = Parameters.CompanyName, searchTerm = Parameters.SearchTerm });
            }
        }
    }
}
