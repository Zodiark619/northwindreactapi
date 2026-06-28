using exerciseAPI.Data;
using exerciseAPI.Models;
using exerciseAPI.Models.Dto;
using exerciseAPI.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace exerciseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderHeaderController : ControllerBase
    {
        private readonly ApiResponse _response;
        private readonly ApplicationDbContext _dbContext;

        public OrderHeaderController(ApplicationDbContext dbContext)
        {
            _response = new();
            _dbContext = dbContext;
        }


        [HttpGet]
        public ActionResult<ApiResponse> GetOrders(string userId = "")
        {
            IEnumerable<OrderHeader> orderHeaderList = _dbContext.OrderHeaders
                .Include(x => x.OrderDetails).ThenInclude(x => x.MenuItem).OrderByDescending(x => x.OrderHeaderId);
            if (!string.IsNullOrEmpty(userId))
            {
                orderHeaderList = orderHeaderList.Where(x => x.ApplicationUserId == userId);

            }
            _response.Result = orderHeaderList;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);


        }
        [HttpGet("{orderId:int}")]
        public ActionResult<ApiResponse> GetOrder(int orderId)
        {
            if (orderId == 0)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Invalid order Id");
                return BadRequest(_response);
            }
            OrderHeader? orderHeader = _dbContext.OrderHeaders
                .Include(x => x.OrderDetails).ThenInclude(x => x.MenuItem).FirstOrDefault(x => x.OrderHeaderId == orderId);
            if (orderHeader == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("order not found");
                return NotFound(_response);
            }

            _response.Result = orderHeader;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);


        }


        [HttpPost]
        public ActionResult<ApiResponse> CreateOrder([FromBody] OrderHeaderCreateDTO orderHeaderCreateDTO)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    OrderHeader orderHeader = new()
                    {
                        PickUpEmail = orderHeaderCreateDTO.PickUpEmail,
                        PickUpName = orderHeaderCreateDTO.PickUpName,
                        PickUpPhoneNumber = orderHeaderCreateDTO.PickUpPhoneNumber,
                        OrderDate = DateTime.Now,
                        OrderTotal = orderHeaderCreateDTO.OrderTotal,
                        Status = SD.status_confirmed,
                        TotalItem = orderHeaderCreateDTO.TotalItem,
                        ApplicationUserId = orderHeaderCreateDTO.ApplicationUserId,
                    };
                    _dbContext.OrderHeaders.Add(orderHeader);
                    _dbContext.SaveChanges();
                    foreach (var orderDetailDTO in orderHeaderCreateDTO.OrderDetailsDTO)
                    {
                        OrderDetail orderDetail = new()
                        {
                            OrderHeaderId = orderHeader.OrderHeaderId,
                            MenuItemId = orderDetailDTO.MenuItemId,
                            Quantity = orderDetailDTO.Quantity,
                            ItemName = orderDetailDTO.ItemName,
                            Price = orderDetailDTO.Price,

                        };
                        _dbContext.OrderDetails.Add(orderDetail);
                    }
                    _dbContext.SaveChanges();
                    _response.Result = orderHeader;
                    orderHeader.OrderDetails = [];
                    _response.StatusCode = HttpStatusCode.Created;
                    return CreatedAtAction("GetOrder", new { orderId = orderHeader.OrderHeaderId }, _response);
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

        [HttpPut("{orderId:int}")]
        public ActionResult<ApiResponse> UpdateOrder(int orderId, [FromBody] OrderHeaderUpdateDTO orderHeaderUpdateDTO)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    if (orderId != orderHeaderUpdateDTO.OrderHeaderId)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.ErrorMessages.Add("Invalid order Id");
                        return BadRequest(_response);
                    }
                    OrderHeader? orderHeaderFromDb = _dbContext.OrderHeaders.FirstOrDefault(x => x.OrderHeaderId == orderId);
                    if (orderHeaderFromDb == null)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.NotFound;
                        _response.ErrorMessages.Add("Order not found");
                        return NotFound(_response);

                    }
                    if (!string.IsNullOrEmpty(orderHeaderFromDb.Status))
                    {
                        if (orderHeaderFromDb.Status.Equals(SD.status_confirmed, StringComparison.InvariantCultureIgnoreCase)
                            && orderHeaderUpdateDTO.Status.Equals(SD.status_readyForPickUp, StringComparison.InvariantCultureIgnoreCase))
                        {
                            orderHeaderFromDb.Status = SD.status_readyForPickUp;
                        }
                        if (orderHeaderFromDb.Status.Equals(SD.status_readyForPickUp, StringComparison.InvariantCultureIgnoreCase)
                            && orderHeaderUpdateDTO.Status.Equals(SD.status_Completed, StringComparison.InvariantCultureIgnoreCase))
                        {
                            orderHeaderFromDb.Status = SD.status_Completed;
                        }
                        if (orderHeaderFromDb.Status.Equals(SD.status_Cancelled, StringComparison.InvariantCultureIgnoreCase)
                       )

                        {
                            orderHeaderFromDb.Status = SD.status_Cancelled;
                        }
                    }














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
