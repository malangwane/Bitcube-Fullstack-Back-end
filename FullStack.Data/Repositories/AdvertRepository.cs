using FullStack.Data.DbContexts;
using FullStack.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FullStack.Data.Repositories
{
    public interface IAdvertRepository
    {
        IEnumerable<Advert> GetAllAdverts();
        Advert GetAdvertById(int advertId);
        List<Province> GetAllProvinces();
        List<City> GetAllCities();
        List<City> GetAllProvinceCities(int provinceId);
    }
    public class AdvertRepository : IAdvertRepository
    {
        private readonly FullStackDbContext _ctx;
        public AdvertRepository(FullStackDbContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<Advert> GetAllAdverts()
        {
            return _ctx.Adverts.Include(ad => ad.Province).Include(ad => ad.City).ToList();
        }

        public Advert GetAdvertById(int advertId)
        {
            return _ctx.Adverts.Include(ad => ad.Province).Include(ad => ad.City)
                .Where(adv => adv.Id == advertId).FirstOrDefault();
        }

        public List<Province> GetAllProvinces()
        {
            return _ctx.Provinces.ToList();
        }

        public List<City> GetAllCities()
        {
            return _ctx.Cities.ToList();
        }

        public List<City> GetAllProvinceCities(int provinceId)
        {
            return _ctx.Cities.Where(ad => ad.ProvinceId == provinceId).ToList();
        }
    }
}
