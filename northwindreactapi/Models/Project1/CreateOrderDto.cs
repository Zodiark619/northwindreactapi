namespace northwindreactapi.Models.Project1
{
    public class CreateOrderDto
    {
        public List<CreateOrderItemDto> Items { get; set; } = [];
    }

    public class CreateOrderItemDto
    {
        public int ItemId { get; set; }

        public int Quantity { get; set; }
    }
}
