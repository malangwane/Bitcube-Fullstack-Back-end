using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using FullStack.API.Helpers;
using FullStack.ViewModels;
using FullStack.ViewModels.Users;
using FullStack.Data.Entities;
using FullStack.API.Exceptions;
using FullStack.Data.Repositories;
using FullStack.API.Helpers.Exceptions;

namespace FullStack.API.Services
{
    public interface IUserService
    {
        AuthenticationResponseModel AuthenticateUser(AuthenticationRequestModel model);
        void CheckUserPassword(int userId, UserPasswordCheckModel model);
        List<UserViewModel> GetAllUsers();
        UserViewModel GetUserById(int id);
        UserViewModel CreateUser(UserCreateUpdateModel model);
        UserViewModel UpdateUser(int userId, UserCreateUpdateModel model);

    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IUserValidator _userValidator;
        public readonly IUserMapper _userMapper;
        private readonly AppSettings _appSettings;

        public UserService(IUserRepository repo,
                            IUserValidator userValidator,
                            IUserMapper userMapper,
                            IOptions<AppSettings> appSettings)
        {
            _repo = repo;
            _userValidator = userValidator;
            _userMapper = userMapper;
            _appSettings = appSettings.Value;
        }

        public AuthenticationResponseModel AuthenticateUser(AuthenticationRequestModel model)
        {
            // validation
            var results = _userValidator.Validate(model).ToArray();
            if (results.Length > 0)
                throw new ValidationApiException(results);

            // get the user from the repository / database 
            var entity = _repo.GetUsers().SingleOrDefault(user => user.Email == model.Email && user.Password == model.Password);

            // throw unathorized exception if user doesn't exist
            if (entity == null)
            {
                throw new UnauthorizedApiException("Username or password is incorrect");
            }

            if (entity.Locked)
            {
                throw new UnauthorizedApiException("Account is locked");
            }

            // authentication successful so generate jwt token
            var token = GenerateJwtToken(entity.Id);

            //return the UserAuthenticateResponseModel to the controller
            return _userMapper.AuthenticateMapper(entity, token);
        }

        public void CheckUserPassword(int userId, UserPasswordCheckModel model)
        {
            // validation
            var results = _userValidator.Validate(model.Password).ToArray();
            if (results.Length > 0)
                throw new ValidationApiException(results);

            User entity = _repo.GetUser(userId);
            if (entity == null)
                throw new NotFoundApiException("User does not exist");

            if (entity.Password != model.Password)
                throw new CheckPasswordApiException("Old password is incorrect");
        }


        public List<UserViewModel> GetAllUsers()
        {
            var entityList = _repo.GetUsers();
            return entityList.Select(user => _userMapper.ViewMapper(user)).ToList();
        }

        public UserViewModel GetUserById(int id)
        {
            var entity = _repo.GetUser(id);
            if (entity == null)
            {
                throw new NotFoundApiException("User does not exist");
            }

            return _userMapper.ViewMapper(entity);
        }

        public UserViewModel CreateUser(UserCreateUpdateModel model)
        {
            // validation
            var results = _userValidator.Validate(model).ToArray();
            if (results.Length > 0)
                throw new ValidationApiException(results);

            if (_repo.GetUsers().Any(user => user.Email == model.Email))
            {
                throw new DuplicateUserApiException("Email address already in use");
            }

            User entity = _userMapper.EntityMapper(model);
            entity = _repo.CreateUser(entity);
            return _userMapper.ViewMapper(entity);
        }

        public UserViewModel UpdateUser(int userId, UserCreateUpdateModel model)
        {
            User entity = _repo.GetUser(userId);

            if (entity == null)
                throw new NotFoundApiException("User does not exist");

            if (model.Password == null)
                model.Password = entity.Password;

            // validation
            var results = _userValidator.Validate(model).ToArray();
            if (results.Length > 0)
                throw new ValidationApiException(results);

            entity = _userMapper.EntityMapper(model);
            entity.Id = userId;
            entity = _repo.UpdateUser(entity);
            return _userMapper.ViewMapper(entity);
        }


        // generate token that is valid for 7 days
        private string GenerateJwtToken(int userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", userId.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
