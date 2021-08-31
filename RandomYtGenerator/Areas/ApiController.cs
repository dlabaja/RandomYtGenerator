using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RandomYTGenerátor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RandomYtGenerator.Areas
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        public async Task<string> Api()
        {
            return await new RandomYtVideo().GetJsonAsync();
        }
    }
}