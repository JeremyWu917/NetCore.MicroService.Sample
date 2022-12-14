using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebMVC.Helper;
using WebMVC.Models;

namespace WebMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServiceHelper _serviceHelper;


        public HomeController(ILogger<HomeController> logger, IServiceHelper serviceHelper)
        {
            _logger = logger;
            _serviceHelper = serviceHelper;
        }

        public async Task<IActionResult> Index()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            ViewBag.OrderData = await _serviceHelper.GetOrder(accessToken);
            ViewBag.ProductData = await _serviceHelper.GetProduct(accessToken);
            return View();
        }

        //public async IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}