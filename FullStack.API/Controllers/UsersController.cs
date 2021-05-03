using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FullStack.API.Services;
using FullStack.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using FullStack.API.Exceptions;
using FullStack.ViewModels.Adverts;

namespace FullStack.API.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
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
        public ActionResult<UserAuthenticateResponseModel> AuthenticateUser(UserAuthenticateRequestModel model)
        {
            var response = _userService.AuthenticateUser(model);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("{userId}/password")]
        public IActionResult CheckUserPassword(int userId, UserPasswordCheckModel model)
        {
            _userService.CheckUserPassword(userId, model);
            return NoContent();
        }

        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<UserViewModel>> GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}", Name ="GetUserById")]
        public ActionResult<UserViewModel> GetUserById(int id)
        {
            var user = _userService.GetUserById(id);
            return Ok(user);
        }

        [Authorize]
        [HttpPut("{userId}")]
        public ActionResult<UserViewModel> UpdateUser(int userId, UserCreateUpdateModel model)
        {
            var user = _userService.UpdateUser(userId, model);
            return Ok(user);
        }

        [Authorize]
        [HttpGet("{userId}/adverts")]
        public ActionResult<IEnumerable<AdvertViewModel>> GetAllUserAdverts(int userId)
        {
            var adverts = _userService.GetAllUserAdverts(userId);
            return Ok(adverts);
        }

        [HttpPost("{userId}/adverts/search")]
        public ActionResult<IEnumerable<AdvertViewModel>> SearchUserAdverts(int userId, AdvertSearchModel model)
        {
            var adverts = _userService.SearchUserAdverts(userId, model);
            return Ok(adverts);
        }

        [Authorize]
        [HttpGet("{userId}/adverts/{advertId}", Name = "GetUserAdvertById")]
        public ActionResult<AdvertViewModel> GetUserAdvertById(int userId, int advertId)
        {
            var advert = _userService.GetUserAdvertById(userId, advertId);
            return Ok(advert);
        }

        [Authorize]
        [HttpPost("{userId}/adverts")]
        public ActionResult<AdvertViewModel> CreateUserAdvertById(int userId, AdvertCreateUpdateModel model)
        {
            var advert = _userService.CreateUserAdvertById(userId, model);
            return CreatedAtAction(nameof(GetUserAdvertById), new { userId = userId, advertId = advert.Id }, advert);
        }

        [Authorize]
        [HttpPut("{userId}/adverts/{advertId}")]
        public IActionResult UpdateUserAdvertById(int userId, int advertId, AdvertCreateUpdateModel model)
        {
            _userService.UpdateUserAdvertById(userId, advertId, model);
            return NoContent();
        }


        [Authorize]
        [HttpGet("favourites/{userId}")]
        public ActionResult<AdvertViewModel> GetUserFavourites(int userId)
        {
            var adverts = _userService.GetUserFavourites(userId);
            return Ok(adverts);
        }

        [Authorize]
        [HttpPut("favourites")]
        public IActionResult AddRemoveUserFavourite(UserFavouriteModel model)
        {
            _userService.AddRemoveUserFavourite(model);
            return NoContent();
        }
    }
}
