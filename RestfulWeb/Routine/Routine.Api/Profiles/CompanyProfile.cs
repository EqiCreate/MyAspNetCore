using AutoMapper;
using Routine.Api.Entities;
using Routine.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Profiles
{
    public class CompanyProfile:Profile
    {
        public CompanyProfile()
        {
            CreateMap<Company, CompanyDto>()
                .ForMember(
                    d=>d.CompanyName,
                    opt=>opt.MapFrom(src=>src.Name)
                );

            CreateMap<CompanyAddDto, Company>();
            CreateMap<Company, CompanyFullDto>();
            CreateMap<CompanyAddWithBankrupDto, Company>();
        }
    }
}
