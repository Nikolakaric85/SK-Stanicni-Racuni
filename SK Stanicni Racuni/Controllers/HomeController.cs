using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SK_Stanicni_Racuni.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SK_Stanicni_Racuni.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            bool val1 = HttpContext.User.Identity.IsAuthenticated; //  da je ulogovan
            var UserId = HttpContext.User.Identity.Name; // daje UserId

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult GetPdf()
        {
            var stream = new FileStream(@"wwwroot\pdf\Stanicni racun uputstvo.pdf", FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
