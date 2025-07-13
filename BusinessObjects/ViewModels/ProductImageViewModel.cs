using System;

namespace BusinessObjects.ViewModels
{
    public class ProductImageViewModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public bool IsMain { get; set; }
        public bool IsActive { get; set; }
    }
} 