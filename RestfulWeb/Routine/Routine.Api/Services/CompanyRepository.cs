using Microsoft.EntityFrameworkCore;
using Routine.Api.Data;
using Routine.Api.DtoParameters;
using Routine.Api.Entities;
using Routine.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Services
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly RoutineDbContext routineDbContext;

        public CompanyRepository(RoutineDbContext routineDbContext)
        {
            this.routineDbContext = routineDbContext??throw new Exception(nameof(routineDbContext));
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

        public async Task<PagedList<Company>> GetCompaniesAsync(CompanyDtoParameters parameters)
        {
            if (parameters == null) throw new ArgumentException(nameof(parameters));
             var queryExpression=this.routineDbContext.Companies as IQueryable<Company>;
            // queryExpression = queryExpression.Skip(parameters.PageSize * (parameters.PageNumber - 1)).Take(parameters.PageSize);
            //return await queryExpression.ToListAsync();
            return await PagedList<Company>.CreateAsync(queryExpression,parameters.PageNumber,parameters.PageSize);
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

        public  async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyid,string genderDisplay,string q)
        {
            if (companyid == Guid.Empty) throw new ArgumentException(nameof(companyid));
            if (string.IsNullOrWhiteSpace(genderDisplay) && string.IsNullOrWhiteSpace(q))
            { 
                return await this.routineDbContext.Employees.Where(x => x.CompanyId == companyid).OrderBy(x => x.EmployeeNo).ToListAsync();
            }
            var items = this.routineDbContext.Employees.Where(x=>x.CompanyId==companyid);
            var gender = Enum.Parse<Gender>(genderDisplay.Trim());
            if (!string.IsNullOrWhiteSpace(genderDisplay))
            {
                items = items.Where(x=>x.gender==gender);
            }
            if (!string.IsNullOrWhiteSpace(q))
            { 
            
            }
          
            return await items.Where(x => x.CompanyId == companyid && x.gender==gender).OrderBy(x => x.EmployeeNo).ToListAsync();
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
