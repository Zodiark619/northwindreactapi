using System.ComponentModel.DataAnnotations;

namespace northwindreactapi.Models
{
    public class Item
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }=string.Empty;
        public decimal Price {  get; set; }  
    }
}
