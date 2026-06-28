using System.ComponentModel.DataAnnotations;

namespace northwindreactapi.Models.Project1
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }

        public int OrderHeaderId { get; set; }
        public OrderHeader? OrderHeader { get; set; }

        public int ItemId { get; set; }

        public string ItemName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal LineTotal { get; set; }
    }
}
