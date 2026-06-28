using Azure.Core;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using northwindreactapi.Data;
using northwindreactapi.Models.Project1;

namespace northwindreactapi.Controllers.Project1
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet("{orderHeaderId:int}")]
        public async Task<IActionResult> GetOrder(   int orderHeaderId)
        {
            var order = await _dbContext.OrderHeaders
        .Select(x => new OrderResponseDto
        {
            OrderHeaderId = x.OrderHeaderId,
            OrderTotal = x.OrderTotal,
            TotalItem = x.TotalItem,
            OrderDetails=x.OrderDetails.Select(d=>  new OrderDetailDto
            {
                OrderDetailId = d.OrderDetailId,
                ItemName = d.ItemName,
                Price = d.Price,
                Quantity = d.Quantity,
                LineTotal = d.LineTotal
            }).ToList()
        })
        .FirstOrDefaultAsync(x => x.OrderHeaderId == orderHeaderId);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }
        [HttpGet ]
        public async Task<IActionResult> GetOrders(   )
        {
            var orders = await _dbContext.OrderHeaders
       .Select(x => new OrderResponseDto
       {
           OrderHeaderId = x.OrderHeaderId,
           OrderTotal = x.OrderTotal,
           TotalItem = x.TotalItem,
           OrderDetails= x.OrderDetails.Select(d => new OrderDetailDto
           {
               OrderDetailId = d.OrderDetailId,
               ItemName = d.ItemName,
               Price = d.Price,
               Quantity = d.Quantity,
               LineTotal = d.LineTotal
           }).ToList()
       })
       .ToListAsync();

            return Ok(orders);
        }
        [HttpPost]
        public async Task< IActionResult> CreateOrder(CreateOrderDto request)
        {
            var orderHeader = new OrderHeader();
            var orderDetailDto = new List<OrderDetailDto>();

            foreach (var dto in request.Items)
            {
                var item = await _dbContext.Items.FindAsync(dto.ItemId);

                if (item == null)
                {
                    return BadRequest($"Item {dto.ItemId} not found");
                }

                var orderDetail = new OrderDetail
                {
                    ItemId = item.ItemId,
                    ItemName = item.Name,
                    Price = item.Price,
                    Quantity = dto.Quantity,
                    LineTotal = item.Price * dto.Quantity
                };

                orderHeader.OrderDetails.Add(orderDetail);
                orderDetailDto.Add(new OrderDetailDto
                {
                    OrderDetailId= orderDetail.OrderDetailId,
                    ItemName = item.Name,
                    Price = item.Price,
                    Quantity = dto.Quantity,
                    LineTotal = item.Price * dto.Quantity
                });
            }

            orderHeader.TotalItem =
                orderHeader.OrderDetails.Sum(x => x.Quantity);

            orderHeader.OrderTotal =
                orderHeader.OrderDetails.Sum(x => x.LineTotal);

            _dbContext.OrderHeaders.Add(orderHeader);

            await _dbContext.SaveChangesAsync();
            var orderHeaderDto = new OrderResponseDto
            {
                OrderHeaderId=orderHeader.OrderHeaderId,
                OrderTotal=orderHeader.OrderTotal,
                TotalItem=orderHeader.TotalItem,
                OrderDetails= orderDetailDto
            };
            return Ok(orderHeaderDto);
        }
    }
}
