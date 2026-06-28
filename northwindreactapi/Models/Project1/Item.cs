using System.ComponentModel.DataAnnotations;

namespace northwindreactapi.Models.Project1
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }
        [Required]
        public string Name { get; set; }=string.Empty;
        public decimal Price {  get; set; }  
    }
}
