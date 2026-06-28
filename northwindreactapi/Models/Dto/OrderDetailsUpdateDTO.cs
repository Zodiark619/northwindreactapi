using System.ComponentModel.DataAnnotations;

namespace northwindreactapi.Models.Dto
{
    public class OrderDetailsUpdateDTO
    {
        [Required]

        public int OrderDetailId { get; set; }
    }
}
