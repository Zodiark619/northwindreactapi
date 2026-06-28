using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using northwindreactapi.Data;
using northwindreactapi.Models;
using northwindreactapi.Models.Dto;
using System.Net;

namespace northwindreactapi.Controllers
{
  //  [Route("api/[controller]")]
   // [ApiController]
    public class OrderDetailsController : ControllerBase
    {
         private readonly ApplicationDbContext _dbContext;

        public OrderDetailsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        //[HttpPut("{orderDetailsId:int}")]
        //public IActionResult UpdateOrder(int orderDetailsId, [FromBody] OrderDetailsUpdateDTO orderDetailsUpdateDTO)
        //{

        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            if (orderDetailsId != orderDetailsUpdateDTO.OrderDetailId)
        //            {
        //                  return BadRequest( );
        //            }
        //            OrderDetail? orderDetailsFromDb = _dbContext.OrderDetails.FirstOrDefault(x => x.OrderDetailId == orderDetailsId);
        //            if (orderDetailsFromDb == null)
        //            {
        //                 return NotFound( );

        //            }

                   












        //            _dbContext.SaveChanges();

        //             return Ok( );
        //        }
        //        else
        //        {
        //              return StatusCode((int)HttpStatusCode.InternalServerError );

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //         return StatusCode((int)HttpStatusCode.InternalServerError );
        //    }
        //}
    }
}
