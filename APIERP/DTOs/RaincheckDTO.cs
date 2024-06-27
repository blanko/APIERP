namespace APIERP.DTOs
{
    public class RaincheckDTO
    {
        public int RaincheckId { get; set; }
        public string Name { get; set; } = null!;
        public int Count { get; set; }
        public double SalePrice { get; set; }
        public int StoreId { get; set; }
        public int ProductId { get; set; }
    }
}
