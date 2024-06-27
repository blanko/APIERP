namespace APIERP.DTOs
{
    public class OrderDetailDTOAdd
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
