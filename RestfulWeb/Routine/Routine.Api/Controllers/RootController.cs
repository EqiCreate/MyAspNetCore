﻿using Microsoft.AspNetCore.Mvc;
using Routine.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Controllers
{
    [ApiController()]
    [Route("api")]
    public class RootController:ControllerBase
    {
        [HttpGet(Name =nameof(GetRoot))]
        public IActionResult GetRoot()
        {
            var links = new List<LinkDto>();
            links.Add(new LinkDto(Url.Link(nameof(GetRoot),new { }),"self","GET"));
            links.Add(new LinkDto(Url.Link(nameof(CompaniesController.GetCompanies), new { }), "companies", "GET"));
            links.Add(new LinkDto(Url.Link(nameof(CompaniesController.CreateCompany), new { }), "create_companies", "POST"));

            return Ok(links);
        }
    }
}
