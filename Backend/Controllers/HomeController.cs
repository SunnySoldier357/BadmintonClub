using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Backend.ViewModels;

namespace Backend.Controllers
{
    public class HomeController : Controller
    {
        // Public Methods
        public IActionResult Index()
        {
            return Redirect("https://shsbadmintonclub.weebly.com/");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel 
            { 
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}