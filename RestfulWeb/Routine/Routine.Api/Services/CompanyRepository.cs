using Microsoft.EntityFrameworkCore;
using Routine.Api.Data;
using Routine.Api.DtoParameters;
using Routine.Api.Entities;
using Routine.Api.Helpers;
using Routine.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Services
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly RoutineDbContext routineDbContext;
        private readonly IPropertyMappingService propertyMappingService;

        public CompanyRepository(RoutineDbContext routineDbContext, IPropertyMappingService propertyMappingService)
        {
            this.routineDbContext = routineDbContext??throw new ArgumentNullException(nameof(routineDbContext));
            this.propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
        }
        public void AddCompany(Company company)
        {
            if (company == null) throw new ArgumentException(nameof(company));
            company.Id = Guid.NewGuid();
            if(company.Employees!=null)
            foreach (var employee in company.Employees)
            { 
                employee.Id= Guid.NewGuid();
            }
            this.routineDbContext.Companies.Add(company);
        }

        public void AddEmployee(Guid companyid, Employee employee)
        {
            if (companyid == Guid.Empty) throw new ArgumentException(nameof(companyid));
            if (employee == null) throw new ArgumentException(nameof(employee));
            employee.CompanyId = companyid;
            this.routineDbContext.Employees.Add(employee);
        }

        public async Task<bool> CompanyExitsAsync(Guid companyid)
        {
            if (companyid == Guid.Empty)
            {
                throw new ArgumentException(nameof(companyid));
            }
            return await this.routineDbContext.Companies.AnyAsync(x=>x.Id==companyid);
        }

        public void DeleteCompany(Company company)
        {
            if (company == null) throw new ArgumentException(nameof(company));
            this.routineDbContext.Companies.Remove(company);
        }

        public void DeleteEmployee(Employee employee)
        {
            if (employee == null) throw new ArgumentException(nameof(employee));
            this.routineDbContext.Employees.Remove(employee);
        }

        public async Task<PagedList<Company>> GetCompaniesAsync(CompanyDtoParameters parameter)
         {
            if (parameter == null) throw new ArgumentException(nameof(parameter));
             var queryExpression=this.routineDbContext.Companies as IQueryable<Company>;
            // queryExpression = queryExpression.Skip(parameters.PageSize * (parameters.PageNumber - 1)).Take(parameters.PageSize);
            //return await queryExpression.ToListAsync();
            if (!string.IsNullOrWhiteSpace(parameter.CompanyName))
            {
                parameter.CompanyName = parameter.CompanyName.Trim();
                queryExpression = queryExpression.Where(x=>x.Name== parameter.CompanyName);
            }
            if (!string.IsNullOrWhiteSpace(parameter.SearchTerm))
            {
                parameter.SearchTerm = parameter.SearchTerm.Trim();
                queryExpression = queryExpression.Where(x => x.Name.Contains(parameter.SearchTerm) || x.Introduction.Contains(parameter.SearchTerm));
            }

            var mappingDict = this.propertyMappingService.GetPropertyMapping<CompanyDto, Company>();
            queryExpression = queryExpression.ApplySource(parameter.OrderBy, mappingDict);
            return await PagedList<Company>.CreateAsync(queryExpression, parameter.PageNumber, parameter.PageSize);
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync(IEnumerable<Guid> companyids)
        {
            if (companyids == null)
            {
                throw new ArgumentException(nameof(companyids));
            }
            return await this.routineDbContext.Companies.Where(x => companyids.Contains(x.Id)).OrderBy(x=>x.Name).ToListAsync();
        }

        public async Task<Company> GetCompanyAsync(Guid companyid)
        {
            if (companyid == Guid.Empty)
            {
                throw new ArgumentException(nameof(companyid));
            }
            return await this.routineDbContext.Companies.FirstOrDefaultAsync(x => x.Id == companyid);
        }

        public async Task<Employee> GetEmployeeAsync(Guid companyid, Guid employeeid)
        {
            if (companyid == Guid.Empty) throw new ArgumentException(nameof(companyid));
            if (employeeid == Guid.Empty) throw new ArgumentException(nameof(employeeid));
            return await this.routineDbContext.Employees.Where(x => x.CompanyId == companyid && x.Id == employeeid).FirstOrDefaultAsync();
        }

        public  async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyid,EmployeeDtoParameter parameter)
        {
            if (companyid == Guid.Empty) throw new ArgumentException(nameof(companyid));
            //if (string.IsNullOrWhiteSpace(parameter.Gender) && string.IsNullOrWhiteSpace(parameter.Q))
            //{
            //    return await this.routineDbContext.Employees.Where(x => x.CompanyId == companyid).OrderBy(x => x.EmployeeNo).ToListAsync();
            //}
            var items = this.routineDbContext.Employees.Where(x=>x.CompanyId==companyid);
            //var gender = Enum.Parse<Gender>(parameter.Gender.Trim());
            if (!string.IsNullOrWhiteSpace(parameter.Gender))
            {
                items = items.Where(x=>x.gender== Enum.Parse<Gender>(parameter.Gender.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(parameter.Q))
            {
                parameter.Q = parameter.Q.Trim();
                items = items.Where(x=>x.EmployeeNo.Contains(parameter.Q)
                    || x.FirstName.Contains(parameter.Q)
                    || x.LastName.Contains(parameter.Q)
                );
            }
            //if (!string.IsNullOrWhiteSpace(parameter.OrderBy))
            //{
            //    if (parameter.OrderBy.ToLowerInvariant() == "name")
            //    {
            //        items = items.OrderBy(x => x.FirstName).ThenBy(x => x.LastName);
            //    }
            //    else
            //    {
            //        items = items.OrderBy(x => parameter.OrderBy);
            //    }
            //}
            var mappingDict = this.propertyMappingService.GetPropertyMapping<EmployeeDto, Employee>();
            items=items.ApplySource(parameter.OrderBy, mappingDict);
            return await items.ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return await this.routineDbContext.SaveChangesAsync() >= 0;
        }
        //类库自带更新
        public void UpdateCompany(Company company)
        {
            
        }
        //类库自带更新
        public void UpdateEmployee(Employee employee)
        {
        }
    }
}
