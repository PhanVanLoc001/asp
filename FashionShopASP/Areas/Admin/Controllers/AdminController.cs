using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FashionShopASP.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        // GET: HomeADController
        public ActionResult Index()
        {
            return View();
        }

        // GET: HomeADController/Details/5
        public ActionResult Details(int id)
        {
            return View(id);
        }

        // GET: HomeADController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeADController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeADController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeADController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeADController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeADController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
