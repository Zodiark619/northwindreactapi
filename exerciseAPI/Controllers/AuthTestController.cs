using exerciseAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace exerciseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthTestController : ControllerBase
    {

        [HttpGet]
        [Authorize]
        public ActionResult<string> Index()
        {
            return "you are authored user";
        }
        [HttpGet("{id:int}")]
        [Authorize(SD.Role_Admin)]
        public ActionResult<string> Index(int id)
        {
            return "you are authored user , with reole admin";
        }
    }
}
