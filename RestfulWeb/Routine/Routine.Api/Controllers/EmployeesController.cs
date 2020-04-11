using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.Api.Entities;
using Routine.Api.Models;
using Routine.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Controllers
{
    [ApiController]
    [Route("api/companies/{companyId}/employees")]
    public class EmployeesController:ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ICompanyRepository companyRepository;

        public EmployeesController(IMapper mapper,ICompanyRepository companyRepository)
        {
            this.mapper = mapper??throw new ArgumentException(nameof(mapper));
            this.companyRepository = companyRepository??throw new ArgumentException(nameof(companyRepository)); ;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesForCompany(Guid companyId,[FromQuery(Name ="gender")]string genderDisplay,string q)
        {
            if (!await this.companyRepository.CompanyExitsAsync(companyId))
                return NotFound();
            var employees = await companyRepository.GetEmployeesAsync(companyId,genderDisplay, q);
            var dtos = this.mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return Ok(dtos);
        }
        [HttpGet("{employeeId}",Name =nameof(GetEmployeeForCompany))]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeForCompany(Guid companyId,Guid employeeId)
        {
            if (!await this.companyRepository.CompanyExitsAsync(companyId))
                return NotFound();
            var employee = await companyRepository.GetEmployeeAsync(companyId,employeeId);
            if (employee == null) return NotFound();
            var dto = this.mapper.Map<EmployeeDto>(employee);
            return Ok(dto);
        }
        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> CreateEmployeeForCompany([FromRoute]Guid companyId,[FromBody]EmployeeAddDto employeeAddDto)
        {
            if (!await this.companyRepository.CompanyExitsAsync(companyId))
            {
                return NotFound();
            }
            var entity = this.mapper.Map<Employee>(employeeAddDto);
            this.companyRepository.AddEmployee(companyId,entity);
            await this.companyRepository.SaveAsync();
            var returndto = this.mapper.Map<EmployeeDto>(entity);
            return CreatedAtRoute(nameof(GetEmployeeForCompany), new { companyId, employeeId = returndto.Id }, returndto);
        }
    }
}
