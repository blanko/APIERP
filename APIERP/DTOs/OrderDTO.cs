using APIERP.Entidades;

namespace APIERP.DTOs
{
    public class OrderDTO
    {

        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Username { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public decimal Total { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
