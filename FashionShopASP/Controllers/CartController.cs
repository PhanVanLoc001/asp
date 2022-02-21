using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FashionShopASP.Data;
using FashionShopASP.Models;
using Microsoft.AspNetCore.Http;

namespace FashionShopASP.Controllers
{
    public class CartController : Controller
    {
        private readonly FashionShopAdmin _context;

        public CartController(FashionShopAdmin context)
        {
            _context = context;
        }
        public IActionResult Carts()
        {
            Invoice invoice = new Invoice();
            string username = HttpContext.Session.GetString("Username");
            if (!checkStock(username))
            {
                ViewBag.ErrorMessage = "Có Sản Phẩm Hết Hàng!";
                ViewBag.Account = _context.Account.Where(c => c.Username == username).FirstOrDefault();
                ViewBag.CartsTotal = _context.Cart.Include(c => c.Product).Include(c => c.Account)
                                .Where(c => c.Account.Username == username)
                                .Sum(c => c.Quantity * c.Product.Price);
            }
            Account acc = _context.Account.FirstOrDefault(c => c.Username == username);
            //Hoa don
            DateTime now = DateTime.Now;
            invoice.Code = now.ToString("yyMMddhhmmss");
            invoice.AccountId = _context.Account.FirstOrDefault(a => a.Username == username).Id;
            invoice.IssueDate = now;
            invoice.ShippingPhone = acc.Phone;
            invoice.ShippingAddress = acc.Address;
            invoice.Total = _context.Cart.Include(c => c.Account).Include(c => c.Product)
                          .Where(c => c.Account.Username == username)
                          .Sum(c => c.Quantity * c.Product.Price);
            _context.Add(invoice);
            _context.SaveChanges();
            //Chi Tiet Hoa Don
            List<Cart> carts = _context.Cart.Include(c => c.Product).Include(c => c.Account)
                             .Where(c => c.Account.Username == username).ToList();
            foreach (Cart item in carts)
            {
                InvoiceDetail invoiceDetail = new InvoiceDetail();
                invoiceDetail.InvoiceId = invoice.Id;
                invoiceDetail.ProductId = item.ProductId;
                invoiceDetail.Quantity = item.Quantity;
                invoiceDetail.UnitPrice = item.Product.Price;
                _context.Add(invoiceDetail);
            }
            _context.SaveChanges();
            foreach (Cart c in carts)
            {
                c.Product.Stock -= c.Quantity;
                _context.Cart.Remove(c);
            }
            _context.SaveChanges();
            return RedirectToAction("Carts", "Cart");
        }
        public IActionResult clearCart()
        {
            string username = HttpContext.Session.GetString("Username");
            List<Cart> carts = _context.Cart.Include(c => c.Product).Include(c => c.Account)
                             .Where(c => c.Account.Username == username).ToList();
            foreach (Cart c in carts)
            {
                _context.Cart.Remove(c);
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Carts));
        }
        public IActionResult Tang(int id)
        {
            string username = HttpContext.Session.GetString("Username");
            int ids = _context.Account.FirstOrDefault(c => c.Username == username).Id;
            Cart cart = _context.Cart.FirstOrDefault(c => c.Id == id && c.AccountId == ids);
            cart.Quantity += 1;
            _context.SaveChanges();
            return RedirectToAction("Carts", "Cart");
        }
        public IActionResult Giam(int id)
        {
            string username = HttpContext.Session.GetString("Username");
            int ids = _context.Account.FirstOrDefault(c => c.Username == username).Id;
            Cart cart = _context.Cart.FirstOrDefault(c => c.Id == id && c.AccountId == ids);
            if (cart.Quantity == 1)
            {
                cart.Quantity = 1;
            }
            else
            {
                cart.Quantity -= 1;
            }
            _context.SaveChanges();
            return RedirectToAction("Carts", "Cart");
        }
        // GET: Carts
        public async Task<IActionResult> Index()
        {
            var shopContext = _context.Cart.Include(c => c.Account).Include(c => c.Product);
            return View(await shopContext.ToListAsync());
        }
        public async Task<IActionResult> CartUser()
        {
            string username = HttpContext.Session.GetString("Username");
            ViewBag.CartsTotal = _context.Cart.Include(c => c.Product).Include(c => c.Account)
                                .Where(c => c.Account.Username == username)
                                .Sum(c => c.Quantity * c.Product.Price);
            var id = HttpContext.Session.GetInt32("id");
            if (HttpContext.Session.Keys.Contains("Username"))
            {
                ViewBag.UserName = username;
            }
            if (HttpContext.Session.Keys.Contains("id"))
            {
                ViewBag.id = HttpContext.Session.GetInt32("id");
            }

            var shopContext = _context.Cart.Include(c => c.Account).Include(c => c.Product).Where(i => i.AccountId == id);
            return View(await shopContext.ToListAsync());
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .Include(c => c.Account)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        //thêm giỏ hàng
        public IActionResult Add(int id)
        {
            return Add(id, 1);
        }
        [HttpPost]
        public IActionResult Add(int ProductId, int Quantity)
        {
            string username = HttpContext.Session.GetString("Username");
            int id = _context.Account.FirstOrDefault(c => c.Username == username).Id;
            Cart cart = _context.Cart.FirstOrDefault(c => c.AccountId == id && c.ProductId == ProductId);
            if (cart == null)
            {
                cart = new Cart();
                cart.AccountId = id;
                cart.ProductId = ProductId;
                cart.Quantity = Quantity;
                _context.Add(cart);

            }
            else
            {
                cart.Quantity += Quantity;

            }
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        private bool checkStock(string username)
        {
            List<Cart> cart = _context.Cart.Include(c => c.Product).Include(c => c.Account).Where(c => c.Account.Username == username).ToList();
            foreach (Cart c in cart)
            {
                if (c.Product.Stock < c.Quantity)
                {
                    return false;
                }
            }
            return true;
        }
        //<<<end thanh toán
        // GET: Carts/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Account, "id", "Username");
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccountId,ProductId,Quantity")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "id", "Username", cart.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", cart.ProductId);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "id", "Username", cart.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", cart.ProductId);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountId,ProductId,Quantity")] Cart cart)
        {
            if (id != cart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Carts));
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "id", "Username", cart.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", cart.ProductId);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> DeleteUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .Include(c => c.Account)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserConfirmed(int id)
        {
            var cart = await _context.Cart.FindAsync(id);
            _context.Cart.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Carts));
        }




        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .Include(c => c.Account)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }





        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.Cart.FindAsync(id);
            _context.Cart.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        private bool CartExists(int id)
        {
            return _context.Cart.Any(e => e.Id == id);
        }
    }
}
