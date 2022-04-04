using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FashionShopASP.Data;
using FashionShopASP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FashionShopASP.Controllers
{
    public class ProductsController : Controller
    {
        private readonly FashionShopAdmin _context;

        public ProductsController(FashionShopAdmin context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.Keys.Contains("Username"))
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
            }
            var shopContext = _context.Product.Include(p => p.ProductType);
            return View(await shopContext.ToListAsync());
        }
        public IActionResult Search(string keyword = "", int min = 0, int max = int.MaxValue)
        {
            if (keyword == null)
            {
                keyword = "";
            }

            var productList = _context.Product.Where(prod => prod.Name.Contains(keyword) || prod.SKU.Contains(keyword) || prod.ProductType.Name.Contains(keyword)).Where(prod => prod.Price >= min && prod.Price <= max).ToList();
            return View(productList);
        }
    }
}
