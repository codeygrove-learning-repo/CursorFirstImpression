using System;

namespace AspireLearning.Contracts.Models
{
    public class CatalogItem
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? CatalogType { get; set; }
        public decimal Price { get; set; }
        public int AvailableStock { get; set; }
        public int AvailableForSell { get; set; }
    }
} 