using AutoMapper;
using Routine.Api.Entities;
using Routine.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Profiles
{
    public class CertProfile:Profile
    {
        public CertProfile()
        {
            CreateMap<IdentifyCert, CertAddDto>();
        }
    }
}
