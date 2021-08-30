using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RandomYTGenerátor.Models;

namespace RandomYTGenerátor.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var generator = await new RandomYtVideo().GetVideoAsync();
            return View(generator);
        }
    }
}