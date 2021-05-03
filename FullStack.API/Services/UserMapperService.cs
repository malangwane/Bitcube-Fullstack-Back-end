using FullStack.Data.Entities;
using FullStack.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullStack.API.Services
{
    public interface IUserMapper
    {
        User EntityMapper(UserCreateUpdateModel model);
        UserViewModel ViewMapper(User entity);
        UserAuthenticateResponseModel AuthenticateMapper(User entity, string token);
    }
    public class UserMapper: IUserMapper
    {
        public User EntityMapper(UserCreateUpdateModel model)
        {
            return new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Password = model.Password,
                AdminRole = model.AdminRole,
                Locked = model.Locked
            };
        }

        public UserViewModel ViewMapper(User entity)
        {
            return new UserViewModel()
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                AdminRole = entity.AdminRole,
                Locked = entity.Locked
            };
        }

        public UserAuthenticateResponseModel AuthenticateMapper(User entity, string token)
        {
            return new UserAuthenticateResponseModel()
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                AdminRole = entity.AdminRole,
                Locked = entity.Locked,
                Token = token
            };
        }
    }
}


