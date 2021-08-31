using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RandomYTGenerátor.Models;
using System.Diagnostics;
using RandomYtGenerator.Models;

namespace RandomYTGenerátor.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var generator = await new RandomYtVideo().GetVideoAsync();
            ViewBag.videoId = generator.videoId;

            if (generator.subCount == 0)
                ViewBag.subCount = "0 / not found";
            else
                ViewBag.subCount = generator.subCount;

            ViewBag.viewCount = generator.viewCount;

            ViewBag.published = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(generator.published);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}