using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/login/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("{email}")]
        public ActionResult GetUserByEmail([FromRoute] string email)
        {
            UserDto user = userService.GetUserByEmail(email);
            return Ok(user);
        }

    }
}
