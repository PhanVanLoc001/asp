﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FashionShopASP.Areas.Admin.Models
{
    public class CartAD
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        // Navigation reference property cho khóa ngoại đến Account
        [DisplayName("Khách hàng")]
        public AccountAD Account { get; set; }

        public int ProductId { get; set; }

        // Navigation reference property cho khóa ngoại đến Product
        [DisplayName("Sản phẩm")]
        public ProductAD Product { get; set; }

        [Required(ErrorMessage = "{0} không được bỏ trống")]
        [DefaultValue(1)]
        [DisplayName("Số Lượng")]
        public int Quantity { get; set; } = 1;
    }
}
