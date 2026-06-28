using exerciseAPI.Data;
using exerciseAPI.Models;
using exerciseAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace exerciseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly ApiResponse _response;
        private readonly ApplicationDbContext _dbContext;

        public OrderDetailsController(ApplicationDbContext dbContext)
        {
            _response = new();
            _dbContext = dbContext;
        }


        [HttpPut("{orderDetailsId:int}")]
        public ActionResult<ApiResponse> UpdateOrder(int orderDetailsId, [FromBody] OrderDetailsUpdateDTO orderDetailsUpdateDTO)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    if (orderDetailsId != orderDetailsUpdateDTO.OrderDetailId)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.ErrorMessages.Add("Invalid order Id");
                        return BadRequest(_response);
                    }
                    OrderDetail? orderDetailsFromDb = _dbContext.OrderDetails.FirstOrDefault(x => x.OrderDetailId == orderDetailsId);
                    if (orderDetailsFromDb == null)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.NotFound;
                        _response.ErrorMessages.Add("Order not found");
                        return NotFound(_response);

                    }

                    orderDetailsFromDb.Rating = orderDetailsUpdateDTO.Rating;














                    _dbContext.SaveChanges();

                    _response.StatusCode = HttpStatusCode.NoContent;
                    return Ok(_response);
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
                    return StatusCode((int)HttpStatusCode.InternalServerError, _response);

                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }
    }
}
