﻿using Routine.Api.DtoParameters;
using Routine.Api.Entities;
using Routine.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Services
{
    public interface ICompanyRepository
    {
        Task<PagedList<Company>> GetCompaniesAsync(CompanyDtoParameters parameters);
        Task<Company> GetCompanyAsync(Guid companyid);
        Task<IEnumerable<Company>> GetCompaniesAsync(IEnumerable<Guid>companyids);
        void AddCompany(Company company);
        void UpdateCompany(Company company);
        void DeleteCompany(Company company);
        Task<bool> CompanyExitsAsync(Guid companyid);
        Task<IEnumerable< Employee>> GetEmployeesAsync(Guid companyid, EmployeeDtoParameter parameters);
        Task<Employee> GetEmployeeAsync(Guid companyid,Guid employeeid);
        void AddEmployee(Guid companyid, Employee employee);
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(Employee employee);
        Task<bool> SaveAsync();
    }
}
