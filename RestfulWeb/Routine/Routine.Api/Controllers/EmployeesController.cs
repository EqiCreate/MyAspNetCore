using AutoMapper;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Routine.Api.DtoParameters;
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
    //[ResponseCache(CacheProfileName = "120sCacheProfile")]
    public class EmployeesController:ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ICompanyRepository companyRepository;

        public EmployeesController(IMapper mapper,ICompanyRepository companyRepository)
        {
            this.mapper = mapper??throw new ArgumentException(nameof(mapper));
            this.companyRepository = companyRepository??throw new ArgumentException(nameof(companyRepository)); ;
        }

        [HttpGet(Name =nameof(GetEmployeesForCompany))]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesForCompany(Guid companyId,[FromQuery]EmployeeDtoParameter parameter)
        {
            if (!await this.companyRepository.CompanyExitsAsync(companyId))
                return NotFound();
            var employees = await companyRepository.GetEmployeesAsync(companyId, parameter);
            var dtos = this.mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return Ok(dtos);
        }

        [HttpGet("{employeeId}",Name =nameof(GetEmployeeForCompany))]
        //[ResponseCache(Duration =60)]
        [HttpCacheExpiration(CacheLocation =CacheLocation.Public,MaxAge =60)][HttpCacheValidation(MustRevalidate =false)]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeForCompany(Guid companyId,Guid employeeId)
        {
            if (!await this.companyRepository.CompanyExitsAsync(companyId))
                return NotFound();
            var employee = await companyRepository.GetEmployeeAsync(companyId,employeeId);
            if (employee == null) return NotFound();
            var dto = this.mapper.Map<EmployeeDto>(employee);
            return Ok(dto);
        }

        [HttpPost(Name =nameof(CreateEmployeeForCompany))]
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

        [HttpPut("{employeeId}")]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid employeeId, EmployeeUpdateDto employeeUpdateDto)
        {
            if (!await this.companyRepository.CompanyExitsAsync(companyId))
            {
                return NotFound();
            }
            var employeeentity = await companyRepository.GetEmployeeAsync(companyId, employeeId);
            if (employeeentity == null)
            {
                var employeeAddEntity = this.mapper.Map<Employee>(employeeUpdateDto);
                employeeAddEntity.Id = employeeId; 
                this.companyRepository.AddEmployee(companyId, employeeAddEntity);
                await this.companyRepository.SaveAsync();
                var returndto = this.mapper.Map<EmployeeDto>(employeeAddEntity);
                return CreatedAtRoute(nameof(GetEmployeeForCompany), new { companyId, employeeId = returndto.Id }, returndto);

            }
            this.mapper.Map(employeeUpdateDto, employeeentity);
            this.companyRepository.UpdateEmployee(employeeentity);
            await this.companyRepository.SaveAsync();
            return NoContent();//返回204
        }

        [HttpPatch("{employeeId}")]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(
            Guid companyId, 
            Guid employeeId,
            JsonPatchDocument<EmployeeUpdateDto> jsonPatchDocument)
        {
            if (!await this.companyRepository.CompanyExitsAsync(companyId))
            {
                return NotFound();
            }
            var employeeentity = await companyRepository.GetEmployeeAsync(companyId, employeeId);
            if (employeeentity == null)
            {
                //return NotFound();
                var employeeDto = new EmployeeUpdateDto();
                jsonPatchDocument.ApplyTo(employeeDto,ModelState);
                if (!TryValidateModel(employeeDto)) { return ValidationProblem(ModelState); }
                else
                {
                    var employeeToAdd= this.mapper.Map<Employee>(employeeDto);
                    employeeToAdd.Id = employeeId;
                    employeeToAdd.CompanyId = companyId;
                    this.companyRepository.UpdateEmployee(employeeToAdd);
                    await this.companyRepository.SaveAsync();
                    var dtoToReturn = this.mapper.Map<EmployeeDto>(employeeToAdd);
                    return CreatedAtRoute(nameof(GetEmployeeForCompany), new { companyId, employeeId = dtoToReturn.Id }, dtoToReturn);
                }
            }
            var dtoToPatch = this.mapper.Map<EmployeeUpdateDto>(employeeentity);
            jsonPatchDocument.ApplyTo(dtoToPatch,ModelState);
            if (!TryValidateModel(dtoToPatch))  { return ValidationProblem(ModelState);};

            this.mapper.Map(dtoToPatch, employeeentity);
            this.companyRepository.UpdateEmployee(employeeentity);
            await this.companyRepository.SaveAsync();
            return NoContent();//返回204
        }

        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid employeeId)
        {
            if (!await this.companyRepository.CompanyExitsAsync(companyId))
            {
                return NotFound();
            }
            var employeeentity = await companyRepository.GetEmployeeAsync(companyId, employeeId);
            if (employeeentity == null) return NotFound();
            this.companyRepository.DeleteEmployee(employeeentity);
            await this.companyRepository.SaveAsync();
            return NoContent();
        }

        /// <summary>
        /// 更改validproblem方法从startup中抽取注入的option 转换为actionresult
        /// </summary>
        /// <param name="modelStateDictionary"></param>
        /// <returns></returns>
        public override ActionResult ValidationProblem(ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
           return  (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
            //return base.ValidationProblem(modelStateDictionary);
        }

    }
}
