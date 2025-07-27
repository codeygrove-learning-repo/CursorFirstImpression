using System;
using System.Collections.Generic;

namespace AspireLearning.WebPortal.Models
{
    public class OrderViewModel
    {
        public string? Id { get; set; }
        public DateTime OrderDate { get; set; }
        public bool Delivered { get; set; }
        public List<OrderItemViewModel>? OrderItems { get; set; }
    }
} 