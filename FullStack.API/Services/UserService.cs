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
using FullStack.ViewModels.Adverts;

namespace FullStack.API.Services
{
    public interface IUserService
    {
        UserAuthenticateResponseModel AuthenticateUser(UserAuthenticateRequestModel model);
        void CheckUserPassword(int userId, UserPasswordCheckModel model);
        List<UserViewModel> GetAllUsers();
        UserViewModel GetUserById(int id);
        UserViewModel CreateUser(UserCreateUpdateModel model);
        UserViewModel UpdateUser(int userId, UserCreateUpdateModel model);

        List<AdvertViewModel> GetAllUserAdverts(int userId);
        List<AdvertViewModel> SearchUserAdverts(int userId, AdvertSearchModel model);
        AdvertViewModel GetUserAdvertById(int userId, int advertId);
        AdvertViewModel CreateUserAdvertById(int userId, AdvertCreateUpdateModel model);
        void UpdateUserAdvertById(int userId, int advertId, AdvertCreateUpdateModel model);

        List<AdvertViewModel> GetUserFavourites(int userId);
        void AddRemoveUserFavourite(UserFavouriteModel model);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IAdvertRepository _advertRepo;
        private readonly IUserValidator _userValidator;
        public readonly IAdvertValidator _advertValidator;
        public readonly IUserMapper _userMapper;
        public readonly IAdvertMapper _advertMapper;
        private readonly AppSettings _appSettings;

        public UserService( IUserRepository repo,
                            IAdvertRepository advertRepo,
                            IUserValidator userValidator,
                            IAdvertValidator advertValidator,
                            IUserMapper userMapper,
                            IAdvertMapper advertMapper,
                            IOptions<AppSettings> appSettings)
        {
            _repo = repo;
            _advertRepo = advertRepo;
            _userValidator = userValidator;
            _advertValidator = advertValidator;
            _userMapper = userMapper;
            _advertMapper = advertMapper;
            _appSettings = appSettings.Value;
        }

        public UserAuthenticateResponseModel AuthenticateUser(UserAuthenticateRequestModel model)
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
            if ( entity == null)
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

        public List<AdvertViewModel> GetAllUserAdverts(int userId)
        {
            var entityList = _repo.GetAllUserAdverts(userId);
            entityList = entityList.Where(ad => ad.State != "Deleted").OrderByDescending(ad => ad.Date).ToList();
            return entityList.Select(advert => _advertMapper.ViewMapper(advert)).ToList();
        }

        public List<AdvertViewModel> SearchUserAdverts(int userId, AdvertSearchModel model)
        {
            var entityList = _repo.GetAllUserAdverts(userId);
            entityList = entityList.Where(ad => ad.State != "Deleted").ToList();

            var returnList = FilterSearch(entityList, model);

            return returnList.Select(advert => _advertMapper.ViewMapper(advert)).ToList();
        }

        public AdvertViewModel GetUserAdvertById(int userId, int advertId)
        {
            if (_repo.GetUser(userId) == null)
                throw new NotFoundApiException("User does not exist");

            var entity = _repo.GetUserAdvertById(userId, advertId);

            if (entity == null || entity.State == "Deleted")
                throw new NotFoundApiException("Advert does not exist");

            return _advertMapper.ViewMapper(entity);
        }

        public AdvertViewModel CreateUserAdvertById(int userId, AdvertCreateUpdateModel model)
        {
            // validation
            var results = _advertValidator.Validate(model).ToArray();
            if (results.Length > 0)
                throw new ValidationApiException(results);

            if (_repo.GetUser(userId) == null)
                throw new NotFoundApiException("User does not exist");

            var entity = _advertMapper.EntityMapper(model);
            entity.UserId = userId;
            entity = _repo.CreateUserAdvertById(entity);
            return _advertMapper.ViewMapper(entity);
        }

        public void UpdateUserAdvertById(int userId, int advertId, AdvertCreateUpdateModel model)
        {
            // validation
            var results = _advertValidator.Validate(model).ToArray();
            if (results.Length > 0)
                throw new ValidationApiException(results);

            if (_repo.GetUser(userId) == null)
                throw new NotFoundApiException("User does not exist");

            if (_repo.GetUserAdvertById(userId, advertId) == null)
                throw new NotFoundApiException("Advert does not exist");

            var entity = _advertMapper.EntityMapper(model);
            entity.UserId = userId;
            entity.Id = advertId;
            _repo.UpdateUserAdvertById(entity);
        }

        public List<AdvertViewModel> GetUserFavourites(int userId)
        {
            var userEntity = _repo.GetUser(userId);

            if (userEntity == null)
                throw new NotFoundApiException("User does not exist");

            var advertList = userEntity.FavouriteJoins.Select(join => join.Advert).ToList();
            advertList.Where(ad => ad.State != "Deleted").ToList();
            return advertList.Select(advert => _advertMapper.ViewMapper(advert)).ToList();
        }

        public void AddRemoveUserFavourite(UserFavouriteModel model)
        {
            var advertEntity = _advertRepo.GetAdvertById(model.AdvertId);
            if (advertEntity == null || advertEntity.State == "Deleted")
                throw new NotFoundApiException("Advert does not exist");

            var userEntity = _repo.GetUser(model.UserId);

            if (userEntity == null)
                throw new NotFoundApiException("User does not exist");

            var joinEntity = new FavouriteJoin();
            joinEntity.UserId = model.UserId;
            joinEntity.AdvertId = model.AdvertId;

            if (userEntity.FavouriteJoins.FirstOrDefault(join => join.AdvertId == model.AdvertId) == null)
            {
                _repo.AddUserFavourite(joinEntity);
            } 
            else
            {
                _repo.RemoveUserFavourite(joinEntity);
            }
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

        private IEnumerable<Advert> FilterSearch(IEnumerable<Advert> adverts, AdvertSearchModel model)
        {
            IEnumerable<Advert> filteredList = new List<Advert>();

            if (model.Keywords != null)
            {
                foreach (string keyword in model.Keywords)
                {
                    filteredList = filteredList.Concat(adverts.Where(ad => ad.Header.ToLower().Contains(keyword.ToLower()) ||
                                                        ad.Description.ToLower().Contains(keyword.ToLower())));
                }
                // remove duplicates
                filteredList = filteredList.Distinct();
            }
            else
            {
                filteredList = adverts;
            }

            if (model.ProvinceId != 0)
                filteredList = filteredList.Where(ad => ad.ProvinceId == model.ProvinceId);


            if (model.CityId != 0)
                filteredList = filteredList.Where(ad => ad.CityId == model.CityId);

            filteredList = filteredList.Where(ad => ad.Price >= model.MinPrice);

            if (model.MaxPrice != 0)
                filteredList = filteredList.Where(ad => ad.Price <= model.MaxPrice);

            switch (model.OrderBy)
            {
                case 1:
                    {
                        filteredList = filteredList.OrderByDescending(ad => ad.Date);
                        break;
                    }
                case 2:
                    {
                        filteredList = filteredList.OrderBy(ad => ad.Price);
                        break;
                    }
                default:
                    {
                        filteredList = filteredList.OrderByDescending(ad => ad.Price);
                        break;
                    }

            }

            return filteredList;
        }
    }
}
