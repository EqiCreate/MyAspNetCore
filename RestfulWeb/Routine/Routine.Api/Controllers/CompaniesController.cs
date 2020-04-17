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
using Microsoft.Net.Http.Headers;
using Routine.Api.ActionConstraints;

namespace Routine.Api.Controllers
{
    [ApiController]//当modelstate验证出错时候，自动返回客户端400
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository companyRepository;
        private readonly IMapper mapper;
        private readonly IPropertyMappingService propertyMappingService;
        private readonly IPropertyCheckServices propertyCheckServices;

        public CompaniesController(ICompanyRepository companyRepository,IMapper mapper, IPropertyMappingService propertyMappingService, IPropertyCheckServices propertyCheckServices)
        {
            this.companyRepository = companyRepository??throw new ArgumentException(nameof(companyRepository));
            this.mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            this.propertyMappingService = propertyMappingService ?? throw new ArgumentException(nameof(propertyMappingService));
            this.propertyCheckServices = propertyCheckServices ?? throw new ArgumentException(nameof(propertyCheckServices));
        }
        [HttpGet(Name =nameof(GetCompanies))]
        [HttpHead]//不返回body其他和httpget一致
        //public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies([FromQuery]CompanyDtoParameters parameters)
        public async Task<IActionResult> GetCompanies([FromQuery]CompanyDtoParameters parameters)
        {
            if (!this.propertyMappingService.ValidMappingExitFor<CompanyDto, Company>(parameters.OrderBy)) return BadRequest();
            if (!this.propertyCheckServices.TypeHasProPerty<CompanyDto>(parameters.Fields)) return BadRequest();
            
            var companies = await this.companyRepository.GetCompaniesAsync(parameters);
            #region 分页增加自描述信息
            var paginationMetedata = new 
            {
                totalCount=companies.TotalCount,
                pageSize=companies.PageSize,
                currentPage=companies.CurrentPage,
                totalPages = companies.TotalPages,
            };
            this.Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetedata, new JsonSerializerOptions() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }));
            #endregion
            var companiesDto = this.mapper.Map<IEnumerable<CompanyDto>>(companies);
            var sharpOriginData= companiesDto.ShapeData(parameters.Fields);
            var links = this.CreateLinksForCompany(parameters, companies.HasPrevious, companies.HasNext);
            var shapedCompaniesWithLinks = sharpOriginData.Select(c=> {
                var companyDict = c as IDictionary<string, object>;
                var companylink = this.CreateLinkForCompany((Guid)companyDict["Id"], parameters.Fields);
                companyDict.Add("links",companylink);
                return companyDict;
            });
            return Ok(new { value= shapedCompaniesWithLinks , links });
        }

        [HttpGet("{companyid}",Name =nameof(GetCompany))]
        [Produces("application/json", "application/vnd.company.hateoas+json", 
            "application/vnd.company.company.hateoas+json", 
            "application/vnd.company.company.friendly+json", 
            "application/vnd.company.company.friendly.hateoas+json", 
            "application/vnd.company.company.full+json", 
            "application/vnd.company.company.full.hateoas+json")]
        public async Task<IActionResult> GetCompany(Guid companyid,string fields,[FromHeader(Name ="Accept")]string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parsedMediaType)) 
            {
                return BadRequest();
            }
           
            if (!this.propertyCheckServices.TypeHasProPerty<CompanyDto>(fields)) return BadRequest();

            if (!await this.companyRepository.CompanyExitsAsync(companyid))
                return NotFound();
            var company = await this.companyRepository.GetCompanyAsync(companyid);
            //var linkedDict = this.mapper.Map<CompanyDto>(company).ShapeData(fields);

            bool IsIncludedLinks = parsedMediaType.SubTypeWithoutSuffix.EndsWith("hateoas",StringComparison.InvariantCultureIgnoreCase);
            IEnumerable<LinkDto> incluedLinks = new List<LinkDto>();
            if (IsIncludedLinks) 
            {
                incluedLinks = this.CreateLinkForCompany(companyid, fields);
            }
            var primaryMediaType = IsIncludedLinks ? parsedMediaType.SubTypeWithoutSuffix.Substring(0,parsedMediaType.SubTypeWithoutSuffix.Length-8):parsedMediaType.SubTypeWithoutSuffix;
            if (primaryMediaType == "vnd.company.company.full")
            { 
                var full= this.mapper.Map<CompanyFullDto>(company).ShapeData(fields);
                if (IsIncludedLinks) full.TryAdd("links", incluedLinks);
                return Ok(full);
            }
            var friendly= this.mapper.Map<CompanyDto>(company).ShapeData(fields);
            if (IsIncludedLinks) friendly.TryAdd("links", incluedLinks);
            return Ok(friendly);
        }

        [HttpPost(Name = nameof(CreateCompanyWithBankrupDto))]
        [RequestHeaderMatchMediaType(requestHeaderToMatch: "Content-Type",
          mediaType: "application/vnd.company.companyforcreationwithbankrupttime+json")]
        [Consumes("application/vnd.company.companyforcreationwithbankrupttime+json")]
        public async Task<ActionResult<CompanyDto>> CreateCompanyWithBankrupDto(CompanyAddWithBankrupDto companyAddDto)
        {
            var entity = this.mapper.Map<Company>(companyAddDto);
            this.companyRepository.AddCompany(entity);
            await this.companyRepository.SaveAsync();
            var returnDto = this.mapper.Map<CompanyDto>(entity);
            var links = this.CreateLinkForCompany(returnDto.Id, null);
            var linkedDict = returnDto.ShapeData(null);
            linkedDict.TryAdd("links", links);
            return CreatedAtRoute(nameof(GetCompany), new { companyid = linkedDict.Select(x => x.Key == "Id") }, linkedDict);
        }

        [HttpPost(Name =nameof(CreateCompany))]
        [RequestHeaderMatchMediaType(requestHeaderToMatch:"Content-Type",
            "application/json",
            "application/vnd.company.companyforcreation+json")]
        [Consumes("application/json","application/vnd.company.companyforcreation+json")]
        public async Task<ActionResult<CompanyDto>> CreateCompany(CompanyAddDto companyAddDto)
        {
            var entity = this.mapper.Map<Company>(companyAddDto);
            this.companyRepository.AddCompany(entity);
            await this.companyRepository.SaveAsync();
            var returnDto = this.mapper.Map<CompanyDto>(entity);
            var links = this.CreateLinkForCompany(returnDto.Id,null);
            var linkedDict = returnDto.ShapeData(null);
            linkedDict.TryAdd("links", links);
            return CreatedAtRoute(nameof(GetCompany),new { companyid= linkedDict.Select(x=>x.Key=="Id") }, linkedDict);
        }

      

        [HttpOptions]
        public IActionResult GetCompanyOption()
        {
            Response.Headers.Add("Allow","GET,POST,OPTIONS");
            return Ok();
        }

        [HttpDelete("{companyid}",Name =nameof(DeleteCompany))]
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

        private string CreateCompaniesResourceUri(CompanyDtoParameters Parameters, ResourceUriType type=ResourceUriType.CurrentPage)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetCompanies),new { fields=Parameters.Fields, orderBy=Parameters.OrderBy, pageNumber=Parameters.PageNumber-1,pageSize=Parameters.PageSize, companyName=Parameters.CompanyName, searchTerm=Parameters.SearchTerm });
                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetCompanies), new { fields = Parameters.Fields, orderBy = Parameters.OrderBy, pageNumber = Parameters.PageNumber + 1, pageSize = Parameters.PageSize, companyName = Parameters.CompanyName, searchTerm = Parameters.SearchTerm });
                case ResourceUriType.CurrentPage:
                    return Url.Link(nameof(GetCompanies), new { fields = Parameters.Fields, orderBy = Parameters.OrderBy, pageNumber = Parameters.PageNumber , pageSize = Parameters.PageSize, companyName = Parameters.CompanyName, searchTerm = Parameters.SearchTerm });
                default:
                    throw new ArgumentNullException(nameof(type));
            }
        }

        private IEnumerable<LinkDto> CreateLinkForCompany(Guid companyId,string fields) 
        {
            var links = new List<LinkDto>();
            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(new LinkDto(Url.Link(nameof(GetCompany), new { companyId }), "self", "GET"));
            }
            else
            { 
                links.Add(new LinkDto(Url.Link(nameof(GetCompany), new { companyId,fields }), "self", "GET"));
            }
            links.Add(new LinkDto(Url.Link(nameof(DeleteCompany), new { companyId }), "delete_company", "DELETE"));
            links.Add(new LinkDto(Url.Link(nameof(EmployeesController.CreateEmployeeForCompany), new { companyId }), "create_employee_for_company", "POST"));
            links.Add(new LinkDto(Url.Link(nameof(EmployeesController.GetEmployeesForCompany), new { companyId }), "employees", "GET"));
            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForCompany(CompanyDtoParameters parameters,bool hasprevious,bool hasnext)
        {
            var links = new List<LinkDto>();
            links.Add(new LinkDto(this.CreateCompaniesResourceUri(parameters,ResourceUriType.CurrentPage),"self","GET"));
            if(hasprevious)
                links.Add(new LinkDto(this.CreateCompaniesResourceUri(parameters, ResourceUriType.PreviousPage), "previous_page", "GET"));
            if(hasnext)
                links.Add(new LinkDto(this.CreateCompaniesResourceUri(parameters, ResourceUriType.NextPage), "next_page", "GET"));
            return links;
        }
    }
}
