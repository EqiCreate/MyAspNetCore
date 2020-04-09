using Microsoft.EntityFrameworkCore;
using Routine.Api.Data;
using Routine.Api.Entities;
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

        public async Task<IEnumerable<Company>> GetCompaniesAsync()
        {
            return await this.routineDbContext.Companies.ToListAsync();
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

        public  async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyid)
        {
            if (companyid == Guid.Empty) throw new ArgumentException(nameof(companyid));
            return await this.routineDbContext.Employees.Where(x => x.CompanyId == companyid).OrderBy(x => x.EmployeeNo).ToListAsync();
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
