using System;
using System.Collections.Generic;

namespace BusinessObjects.ViewModels
{
    public class CartItemViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class CartViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<CartItemViewModel> Items { get; set; }
    }
} 