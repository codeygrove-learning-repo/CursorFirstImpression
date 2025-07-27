namespace AspireLearning.Repository
{
    public class MongoDbSettings
    {
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
        public string? CatalogCollectionName { get; set; }
        public string? OrderCollectionName { get; set; }
    }
} 