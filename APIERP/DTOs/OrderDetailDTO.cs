using System.ComponentModel.DataAnnotations.Schema;

namespace APIERP.DTOs
{
    public class OrderDetailDTO
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
