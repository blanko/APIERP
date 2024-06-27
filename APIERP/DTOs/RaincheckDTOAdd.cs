namespace APIERP.DTOs
{
    public class RaincheckDTOAdd
    {
        public string Name { get; set; } = null!;
        public int Count { get; set; }
        public double SalePrice { get; set; }
        public int StoreId { get; set; }
        public int ProductId { get; set; }
    }
}
