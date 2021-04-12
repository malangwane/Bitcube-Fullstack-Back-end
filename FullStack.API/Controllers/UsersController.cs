using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FullStack.API.Services;
using FullStack.ViewModels;
using FullStack.ViewModels.Users;

namespace FullStack.API.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public ActionResult<UserViewModel> RegisterUser(UserCreateUpdateModel model)
        {

            var user = _userService.CreateUser(model);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPost("authenticate")]
        public ActionResult<AuthenticationResponseModel> AuthenticateUser(AuthenticationRequestModel model)
        {
            var response = _userService.AuthenticateUser(model);
            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<UserViewModel>> GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }

        [Authorize]
        [HttpPost("{userId}/password")]
        public IActionResult CheckUserPassword(int userId, UserPasswordCheckModel model)
        {
            _userService.CheckUserPassword(userId, model);
            return NoContent();
        }

        [HttpGet("{id}", Name = "GetUserById")]
        public ActionResult<UserViewModel> GetUserById(int id)
        {
            var user = _userService.GetUserById(id);
            return Ok(user);
        }
    }
}
