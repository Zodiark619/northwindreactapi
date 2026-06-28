using System.ComponentModel.DataAnnotations;

namespace northwindreactapi.Models.Dto
{
    public class OrderHeaderUpdateDTO
    {
        [Required]

        public int OrderHeaderId { get; set; }

      
    }
}
