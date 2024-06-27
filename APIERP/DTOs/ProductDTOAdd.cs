using APIERP.Entidades;

namespace APIERP.DTOs
{
    public class ProductDTOAdd
    {
        public string SkuNumber { get; set; } = null!;
        public int CategoryId { get; set; }
        public int RecommendationId { get; set; } = 0;
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal SalePrice { get; set; }
        public string? ProductArtUrl { get; set; }
        public string Description { get; set; } = null!;
        public string ProductDetails { get; set; } = null!;
        public int Inventory { get; set; }
        public int LeadTime { get; set; }
    }
}
