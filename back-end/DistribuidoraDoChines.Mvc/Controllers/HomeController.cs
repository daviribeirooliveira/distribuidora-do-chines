using System.Diagnostics;
using DistribuidoraDoChines.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DistribuidoraDoChines.Mvc.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}