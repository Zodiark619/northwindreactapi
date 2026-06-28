using System.ComponentModel.DataAnnotations;

namespace northwindreactapi.Models.Project1
{
    public class OrderDetailDto
    {
        
        
      public int OrderDetailId {  get; set; }
        

        public string ItemName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal LineTotal { get; set; }
    }
}
