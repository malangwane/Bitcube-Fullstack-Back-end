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

        List<Advert> GetAllUserAdverts(int userId);
        Advert GetUserAdvertById(int userId, int advertId);
        Advert CreateUserAdvertById(Advert advert);
        void UpdateUserAdvertById(Advert advert);
        void AddUserFavourite(FavouriteJoin favourite);
        void RemoveUserFavourite(FavouriteJoin favourite);
    }
    public class UserRepository: IUserRepository
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
            return _ctx.Users
                .Include(user => user.FavouriteJoins)
                    .ThenInclude(fav => fav.Advert)
                        .ThenInclude(ad => ad.Province)
                .Include(user => user.FavouriteJoins)
                    .ThenInclude(fav => fav.Advert)
                        .ThenInclude(ad => ad.City)
                .FirstOrDefault(user => user.Id == id);
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
        public List<Advert> GetAllUserAdverts(int userId)
        {
            return _ctx.Adverts.Include(ad => ad.Province).Include(ad => ad.City).Where(ad => ad.UserId == userId).ToList();
        }

        public Advert GetUserAdvertById(int userId, int advertId)
        {
            return _ctx.Adverts.Include(ad => ad.Province).Include(ad => ad.City)
                .Where(adv => adv.UserId == userId && adv.Id == advertId).FirstOrDefault();
        }

        public Advert CreateUserAdvertById(Advert advert)
        {
            _ctx.Adverts.Add(advert);
            _ctx.Entry(advert).Reference(ad => ad.Province).Load();
            _ctx.Entry(advert).Reference(ad => ad.City).Load();
            _ctx.SaveChanges();
            return advert;
        }

        public void UpdateUserAdvertById(Advert advert)
        {
            _ctx.Adverts.Update(advert);
            _ctx.SaveChanges();
        }

        public void AddUserFavourite(FavouriteJoin favourite)
        {
            _ctx.Add(favourite);
            _ctx.SaveChanges();
        }

        public void RemoveUserFavourite(FavouriteJoin favourite)
        {
            _ctx.Remove(favourite);
            _ctx.SaveChanges();
        }
    }
}
