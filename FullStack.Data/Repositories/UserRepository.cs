using FullStack.Data.DbContexts;
using FullStack.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FullStack.Data.Repositories
{
    public interface IUserRepository
    {
        List<User> GetUsers();
        User GetUser(int id);
        User CreateUser(User user);
        User UpdateUser(User user);

    }
    public class UserRepository : IUserRepository
    {
        private readonly FullStackDbContext _ctx;
        public UserRepository(FullStackDbContext ctx)
        {
            _ctx = ctx;
        }

        public List<User> GetUsers()
        {
            return _ctx.Users.ToList();
        }

        public User GetUser(int id)
        {
            return _ctx.Users.FirstOrDefault(user => user.Id == id);
        }

        public User CreateUser(User user)
        {
            _ctx.Users.Add(user);
            _ctx.SaveChanges();

            return user;
        }

        public User UpdateUser(User user)
        {
            _ctx.Users.Update(user);
            _ctx.SaveChanges();

            return user;
        }
      
      

      
    }
}
