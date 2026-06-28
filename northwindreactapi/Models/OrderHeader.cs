using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace northwindreactapi.Models
{
    public class OrderHeader
    {
        [Key]
        public int OrderHeaderId { get; set; }
     
        public DateTime OrderDate { get; set; }

        public string IdentityUserId { get; set; } = string.Empty;
        [ForeignKey("IdentityUserId")]
        public IdentityUser? IdentityUser { get; set; }

        public decimal OrderTotal { get; set; } 
        public int TotalItem { get; set; }

        public List<OrderDetail> OrderDetails { get; set; } = new();
    }
}
