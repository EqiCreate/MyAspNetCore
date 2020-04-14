using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.Api.Entities;
using Routine.Api.Helpers;
using Routine.Api.Models;
using Routine.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Controllers
{
    [ApiController]
    [Route("api/companiescollections")]
    public class CompaniesCollectionController:ControllerBase
    {
        private ICompanyRepository companyRepository;
        private IMapper mapper;

        public CompaniesCollectionController(ICompanyRepository companyRepository, IMapper mapper)
        {
            this.companyRepository = companyRepository ?? throw new ArgumentException(nameof(companyRepository));
            this.mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }

        [HttpGet("({ids})",Name = nameof(GetCompaniesCollections))]
        public async Task<IActionResult> GetCompaniesCollections([FromRoute][ModelBinder(BinderType =typeof(ArrayModelBinder))]IEnumerable<Guid>ids)
        {
            if (ids == null) return BadRequest();
            var entities = await this.companyRepository.GetCompaniesAsync(ids);
            if (ids.Count() != entities.Count()) return NotFound();
            var dtoreturns = this.mapper.Map<IEnumerable<CompanyDto>>(entities);
            return Ok(dtoreturns);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> CreateCompaniesCollection(IEnumerable<CompanyAddDto> companyAddDtos)
        {
            var entities = this.mapper.Map<IEnumerable<Company>>(companyAddDtos);
            foreach (var item in entities)
            {
                this.companyRepository.AddCompany(item);
            }
            await this.companyRepository.SaveAsync();
            var dtosreturn = this.mapper.Map<IEnumerable<CompanyDto>>(entities);
            var idsstirng = string.Join(",",entities.Select(x=>x.Id));
            return CreatedAtRoute(nameof(GetCompaniesCollections), new { ids= idsstirng }, dtosreturn);
        }
    }
}
