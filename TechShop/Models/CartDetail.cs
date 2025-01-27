﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechShop.Models
{
    public class CartDetail
    {
        [Key] // Đặt Id làm khóa chính
        public int Id { get; set; }

        public int CartId { get; set; }

        [Required, Range(0, int.MaxValue, ErrorMessage = "Yêu cầu nhập số lượng sản phẩm")]
        public int quantity { get; set; }

        [Required]
        public int specId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal price { get; set; }

        public bool status { get; set; } = false;

        [ForeignKey("CartId")]
        public ShoppingCart cart { get; set; }

        [ForeignKey("specId")]
        public Specs Specs { get; set; }
    }
}
