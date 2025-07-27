using System;
using System.Collections.Generic;

namespace AspireLearning.Contracts.Models
{
    public class Order
    {
        public string? Id { get; set; }
        public DateTime OrderDate { get; set; }
        public bool Delivered { get; set; }
        public List<OrderItem>? OrderItems { get; set; }
    }
} 