namespace AspireLearning.Contracts.Models
{
    public class OrderItem
    {
        public string? CatalogItemId { get; set; }
        public int OrderedQuantity { get; set; }
        public int SuppliedQuantity { get; set; }
    }
} 