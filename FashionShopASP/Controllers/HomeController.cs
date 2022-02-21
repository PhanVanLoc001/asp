using FashionShopASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FashionShopASP.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FashionShopASP.Controllers
{
    public class HomeController : Controller
    {
        private readonly FashionShopAdmin _context;

        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger, FashionShopAdmin context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            if (HttpContext.Session.Keys.Contains("Username"))
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
            }
            var lstProduct = _context.Product;
            return View(await lstProduct.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        } 
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Account _account)
        {
           
            return View("Index");
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Account _account)
        {

            return View("Index");
        }

    }
}

