using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using northwindreactapi.Data;
using northwindreactapi.Models;
using northwindreactapi.Models.Dto;
using System.Net;

namespace northwindreactapi.Controllers
{
  //  [Route("api/[controller]")]
   // [ApiController]
    public class OrderHeaderController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderHeaderController(ApplicationDbContext dbContext)
        {
             _dbContext = dbContext;
        }
        //[HttpGet]
        //public IActionResult  GetOrders(string userId = "")
        //{
        //    IEnumerable<OrderHeader> orderHeaderList = _dbContext.OrderHeaders
        //        .Include(x => x.OrderDetails).ThenInclude(x => x.Item).OrderByDescending(x => x.OrderHeaderId);
        //    if (!string.IsNullOrEmpty(userId))
        //    {
        //        orderHeaderList = orderHeaderList.Where(x => x.IdentityUserId == userId);

        //    }
             
        //    return Ok(orderHeaderList);


        //}
        //[HttpGet("{orderId:int}")]
        //public IActionResult  GetOrder(int orderId)
        //{
        //    if (orderId == 0)
        //    {
               
        //        return BadRequest();
        //    }
        //    OrderHeader? orderHeader = _dbContext.OrderHeaders
        //        .Include(x => x.OrderDetails).ThenInclude(x => x.Item).FirstOrDefault(x => x.OrderHeaderId == orderId);
        //    if (orderHeader == null)
        //    {
                
        //        return NotFound( );
        //    }

            
        //    return Ok(orderHeader);


        //}




        //[HttpPost]
        //public IActionResult  CreateOrder([FromBody] OrderHeaderCreateDTO orderHeaderCreateDTO)
        //{

        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            OrderHeader orderHeader = new()
        //            {
                       
        //                OrderDate = DateTime.Now,
        //                OrderTotal = orderHeaderCreateDTO.OrderTotal,
                      
        //                TotalItem = orderHeaderCreateDTO.TotalItem,
        //                IdentityUserId = orderHeaderCreateDTO.IdentityUserId,
        //            };
        //            _dbContext.OrderHeaders.Add(orderHeader);
        //            _dbContext.SaveChanges();
        //            foreach (var orderDetailDTO in orderHeaderCreateDTO.OrderDetailsDTO)
        //            {
        //                OrderDetail orderDetail = new()
        //                {
        //                    OrderHeaderId = orderHeader.OrderHeaderId,
        //                  ItemId = orderDetailDTO.ItemId,
        //                    Quantity = orderDetailDTO.Quantity,
        //                    ItemName = orderDetailDTO.ItemName,
        //                    Price = orderDetailDTO.Price,

        //                };
        //                _dbContext.OrderDetails.Add(orderDetail);
        //            }
        //            _dbContext.SaveChanges();
                    
        //            return CreatedAtAction("GetOrder", new { orderId = orderHeader.OrderHeaderId } );
        //        }
        //        else
        //        {
                    
        //            return StatusCode((int)HttpStatusCode.InternalServerError );

        //        }
        //    }
        //    catch (Exception ex)
        //    {
                 
        //        return StatusCode((int)HttpStatusCode.InternalServerError );
        //    }
        //}
        //[HttpPut("{orderId:int}")]
        //public IActionResult  UpdateOrder(int orderId, [FromBody] OrderHeaderUpdateDTO orderHeaderUpdateDTO)
        //{

        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            if (orderId != orderHeaderUpdateDTO.OrderHeaderId)
        //            {
                      
        //                return BadRequest( );
        //            }
        //            OrderHeader? orderHeaderFromDb = _dbContext.OrderHeaders.FirstOrDefault(x => x.OrderHeaderId == orderId);
        //            if (orderHeaderFromDb == null)
        //            {
        //                  return NotFound( );

        //            }
                    














        //            _dbContext.SaveChanges();

        //             return Ok( );
        //        }
        //        else
        //        {
        //            return StatusCode((int)HttpStatusCode.InternalServerError );

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //         return StatusCode((int)HttpStatusCode.InternalServerError );
        //    }
        //}





    }
}
