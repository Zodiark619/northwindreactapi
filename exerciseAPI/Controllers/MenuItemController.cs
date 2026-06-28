using exerciseAPI.Data;
using exerciseAPI.Models;
using exerciseAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace exerciseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApiResponse _response;
        public MenuItemController(ApplicationDbContext dbContext,IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
            _response = new ApiResponse();
        }

        [HttpGet]
        public IActionResult GetMenuItems()
        {
            List<MenuItem> menuItems = _dbContext.MenuItems.ToList();
            List<OrderDetail> orderDetailsWithRatings = _dbContext.OrderDetails.Where(x => x.Rating != null).ToList();

            foreach (var menuItem in menuItems)
            {
                var ratings = orderDetailsWithRatings.Where(x => x.MenuItemId == menuItem.Id).Select(x => x.Rating.Value);
                double avgRating = ratings.Any() ? ratings.Average() : 0;
                menuItem.Rating = avgRating;
            }
            _response.Result = menuItems;
            _response.StatusCode=HttpStatusCode.OK;
            return Ok(_response);
        }
        [HttpGet("{id:int}",Name ="GetMenuItem")]
        public IActionResult GetMenuItem(int id)
        {
            if(id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            MenuItem? menuItem= _dbContext.MenuItems.FirstOrDefault(x => x.Id == id);

            List<OrderDetail> orderDetailsWithRating = _dbContext.OrderDetails
                .Where(x => x.Rating != null && x.MenuItemId == menuItem.Id).ToList();


            var ratings = orderDetailsWithRating.Select(x => x.Rating.Value);
            double avgRating = ratings.Any() ? ratings.Average() : 0;
            menuItem.Rating = avgRating;


            _response.Result = menuItem;
            _response.StatusCode=HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ApiResponse>> CreateMenuItem([FromForm] MenuItemCreateDTO menuItemCreateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                if (menuItemCreateDTO.File == null || menuItemCreateDTO.File.Length == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode=HttpStatusCode.BadRequest;
                    _response.ErrorMessages = ["File is required"];
                    return BadRequest(_response);
                }
                    var imagesPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    if(!Directory.Exists(imagesPath))
                    {
                        Directory.CreateDirectory(imagesPath);
                    }
                    var filePath = Path.Combine(imagesPath, menuItemCreateDTO.File.FileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    using(var filestream=new FileStream(filePath, FileMode.Create))
                    {
                        await menuItemCreateDTO.File.CopyToAsync(filestream);

                    }
                    MenuItem menuItem = new()
                    {
                        Description = menuItemCreateDTO.Description,
                        SpecialTag = menuItemCreateDTO.SpecialTag,
                        Category = menuItemCreateDTO.Category,
                        Price = menuItemCreateDTO.Price,
                        Name = menuItemCreateDTO.Name,
                        Image="images/"+menuItemCreateDTO.File.FileName,
                    };
                    _dbContext.MenuItems.Add(menuItem);
                    await _dbContext.SaveChangesAsync();

                    _response.Result = menuItemCreateDTO;
                    _response.StatusCode = HttpStatusCode.Created;
                    return CreatedAtAction("GetMenuItem", new {id=menuItem.Id},_response);

                }
                else
                {
                    _response.IsSuccess = false;
                }




            }
            catch (Exception ex)
            {
                _response.IsSuccess=false;
                _response.ErrorMessages = [ex.Message.ToString()];
            }
            return BadRequest(_response );
        }
        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ApiResponse>> UpdateMenuItem(int id,[FromForm] MenuItemUpdateDTO menuItemUpdateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                if (menuItemUpdateDTO  == null || menuItemUpdateDTO.Id!=id)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode=HttpStatusCode.BadRequest;
                      return BadRequest(_response);
                }
                    MenuItem? menuItemFromDb = await _dbContext.MenuItems.FirstOrDefaultAsync(x => x.Id == id);
                    if (menuItemFromDb == null)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.NotFound;
                        return NotFound(_response);

                    }
                    menuItemFromDb.Name = menuItemUpdateDTO.Name;
                    menuItemFromDb.Description = menuItemUpdateDTO.Description;
                    menuItemFromDb.Price = menuItemUpdateDTO.Price;
                    menuItemFromDb.Category = menuItemUpdateDTO.Category;
                    menuItemFromDb.SpecialTag = menuItemUpdateDTO.SpecialTag;

                    if (menuItemUpdateDTO.File != null && menuItemUpdateDTO.File.Length > 0)
                    {
                        var imagesPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        if (!Directory.Exists(imagesPath))
                        {
                            Directory.CreateDirectory(imagesPath);
                        }
                        var filePath = Path.Combine(imagesPath, menuItemUpdateDTO.File.FileName);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                        var filePath_OldFile = Path.Combine(_webHostEnvironment.WebRootPath, menuItemFromDb.Image);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    using(var filestream=new FileStream(filePath, FileMode.Create))
                    {
                        await menuItemUpdateDTO.File.CopyToAsync(filestream);

                    }
                        menuItemFromDb.Image = "images/" + menuItemUpdateDTO.File.FileName;

                    }
                     
                    _dbContext.MenuItems.Update(menuItemFromDb);
                    await _dbContext.SaveChangesAsync();

                      _response.StatusCode = HttpStatusCode.NoContent;
                    return Ok( _response);

                }
                else
                {
                    _response.IsSuccess = false;
                }




            }
            catch (Exception ex)
            {
                _response.IsSuccess=false;
                _response.ErrorMessages = [ex.Message.ToString()];
            }
            return BadRequest(_response );
        }
        [HttpDelete]
        //[Consumes("multipart/form-data")]
        public async Task<ActionResult<ApiResponse>> DeleteMenuItem(int id )
        {
            try
            {
                if (ModelState.IsValid)
                {

                if ( id==0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode=HttpStatusCode.BadRequest;
                      return BadRequest(_response);
                }
                    MenuItem? menuItemFromDb = await _dbContext.MenuItems.FirstOrDefaultAsync(x => x.Id == id);
                    if (menuItemFromDb == null)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.NotFound;
                        return NotFound(_response);

                    }
                   
                    
                        var filePath_OldFile = Path.Combine(_webHostEnvironment.WebRootPath, menuItemFromDb.Image);
                        if (System.IO.File.Exists(filePath_OldFile))
                        {
                            System.IO.File.Delete(filePath_OldFile);
                        }
                    
                     
                    _dbContext.MenuItems.Remove(menuItemFromDb);
                    await _dbContext.SaveChangesAsync();

                      _response.StatusCode = HttpStatusCode.NoContent;
                    return Ok( _response);

                }
                else
                {
                    _response.IsSuccess = false;
                }




            }
            catch (Exception ex)
            {
                _response.IsSuccess=false;
                _response.ErrorMessages = [ex.Message.ToString()];
            }
            return BadRequest(_response );
        }
    }
}
