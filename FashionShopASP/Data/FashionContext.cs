using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FashionShopASP.Models;

namespace FashionShopASP.Data
{
    public class FashionShopAdmin : DbContext
    {
        public FashionShopAdmin(DbContextOptions<FashionShopAdmin> options)
             : base(options)
        {
        }

        public DbSet<FashionShopASP.Models.Account> Account { get; set; }

        public DbSet<FashionShopASP.Models.Cart> Cart { get; set; }

        public DbSet<FashionShopASP.Models.Invoice> Invoice { get; set; }

        public DbSet<FashionShopASP.Models.InvoiceDetail> InvoiceDetail { get; set; }

        public DbSet<FashionShopASP.Models.Product> Product { get; set; }

        public DbSet<FashionShopASP.Models.ProductType> ProductType { get; set; }

    }
}