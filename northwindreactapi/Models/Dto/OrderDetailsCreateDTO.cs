using System.ComponentModel.DataAnnotations;

namespace northwindreactapi.Models.Dto
{
    public class OrderDetailsCreateDTO
    {
        [Required]
        public int ItemId { get; set; }

        [Required]
        public int Quantity { get; set; }
        [Required]
        public string ItemName { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
    }
}
