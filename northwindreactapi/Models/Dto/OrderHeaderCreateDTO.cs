namespace northwindreactapi.Models.Dto
{
    public class OrderHeaderCreateDTO
    {
       
        public string IdentityUserId { get; set; } = string.Empty;


        public decimal OrderTotal { get; set; }
        public int TotalItem { get; set; }

        public List<OrderDetailsCreateDTO> OrderDetailsDTO { get; set; } = new();
    }
}
