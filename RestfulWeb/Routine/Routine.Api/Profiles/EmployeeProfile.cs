using AutoMapper;
using Routine.Api.Entities;
using Routine.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Profiles
{
    public class EmployeeProfile:Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(d=>d.Name,opt=>opt.MapFrom(src=>src.FirstName+src.LastName))
                .ForMember(d=>d.GenderDisplay,opt=>opt.MapFrom(src=>src.gender.ToString()))
                .ForMember(d => d.Age, opt => opt.MapFrom(src => DateTime.Now.Year-src.DateofBirth.Year+1))
                ;
        }
    }
}
