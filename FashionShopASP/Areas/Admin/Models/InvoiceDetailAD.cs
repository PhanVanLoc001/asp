using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FashionShopASP.Areas.Admin.Models
{
    public class InvoiceDetailAD
    {
        public int Id { get; set; }

        public int InvoiceId { get; set; }

        // Navigation reference property cho khóa ngoại đến Invoice
        [DisplayName("Hóa đơn")]
        public InvoiceAD Invoice { get; set; }

        public int ProductId { get; set; }

        // Navigation reference property cho khóa ngoại đến Product
        [DisplayName("Sản phẩm")]
        public ProductAD Product { get; set; }

        [DisplayName("Số lượng")]
        [DefaultValue(1)]
        public int Quantity { get; set; } = 1;

        [DisplayName("Đơn giá")]
        [DefaultValue(0)]
        public int UnitPrice { get; set; } = 0;
    }
}
