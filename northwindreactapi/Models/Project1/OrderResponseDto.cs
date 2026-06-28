namespace northwindreactapi.Models.Project1
{
    public class OrderResponseDto
    {
         
            public int OrderHeaderId { get; set; }
            public decimal OrderTotal { get; set; }
            public int TotalItem { get; set; }


         

            public List<OrderDetailDto> OrderDetails { get; set; } = [];
        
    }
}
