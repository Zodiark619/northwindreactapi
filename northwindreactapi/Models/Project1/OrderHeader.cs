using System.ComponentModel.DataAnnotations;

namespace northwindreactapi.Models.Project1
{
    public class OrderHeader
    {
        [Key]
        public int OrderHeaderId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public string IdentityUserId { get; set; } = string.Empty;

        public decimal OrderTotal { get; set; }

        public int TotalItem { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
            = new List<OrderDetail>();
    }
}
