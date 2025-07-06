namespace ProductsAPI.Models
{
    public class ProductResponse
    {
        public int? ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? StockAvailable { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? Success { get; set; }
        public string? Message { get; set; } 
    }
}
