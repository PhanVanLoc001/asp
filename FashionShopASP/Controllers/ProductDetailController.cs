using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FashionShopASP.Models;
using FashionShopASP.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace FashionShopASP.Controllers
{
    public class ProductDetailController : Controller
    {
        private readonly FashionShopAdmin _context;

        public ProductDetailController(FashionShopAdmin context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? id)
        {
            if (HttpContext.Session.Keys.Contains("Username"))
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
            }
            var product = await _context.Product
                .Include(i => i.ProductType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}
