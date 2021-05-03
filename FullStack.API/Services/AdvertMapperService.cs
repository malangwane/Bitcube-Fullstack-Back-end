using FullStack.Data.Entities;
using FullStack.ViewModels.Adverts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FullStack.API.Services
{
    public interface IAdvertMapper
    {
        AdvertViewModel ViewMapper(Advert entity);
        Advert EntityMapper(AdvertCreateUpdateModel model);
        ProvinceViewModel ProvinceMapper(Province entity);
        CityViewModel CityMapper(City entity);

    }
    public class AdvertMapper : IAdvertMapper
    {
        public AdvertViewModel ViewMapper(Advert entity)
        {
            return new AdvertViewModel()
            {
                Id = entity.Id,
                Header = entity.Header,
                Description = entity.Description,
                Province = entity.Province.Name,
                ProvinceId = entity.ProvinceId,
                City = entity.City.Name,
                CityId = entity.CityId,
                Price = entity.Price,
                Date = entity.Date.ToString("dd-MM-yyyy"),
                State = entity.State,
                Featured = entity.Featured,
                UserId = entity.UserId
            };
        }

        public Advert EntityMapper(AdvertCreateUpdateModel model)
        {
            return new Advert()
            {
                Header = model.Header,
                Description = model.Description,
                ProvinceId = model.ProvinceId,
                CityId = model.CityId,
                Price = model.Price,
                Date = DateTime.ParseExact(model.Date, "dd-MM-yyyy", CultureInfo.InvariantCulture),
                State = model.State,
                Featured = model.Featured
            };
        }

        public ProvinceViewModel ProvinceMapper(Province entity)
        {
            return new ProvinceViewModel()
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public CityViewModel CityMapper(City entity)
        {
            return new CityViewModel()
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}
