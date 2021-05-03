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
    public interface IAdvertService
    {
        IEnumerable<AdvertViewModel> GetAllAdverts();
        AdvertViewModel GetAdvertById(int advertId);
        IEnumerable<ProvinceViewModel> GetAllProvinces();
        IEnumerable<CityViewModel> GetAllCities();
        IEnumerable<CityViewModel> GetAllProvinceCities(int provinceId);
        IEnumerable<AdvertViewModel> SearchFeaturedAdverts(AdvertSearchModel model);
        IEnumerable<AdvertViewModel> SearchNonFeaturedAdverts(AdvertSearchModel model);
        IEnumerable<AdvertViewModel> GetAllFeaturedAdverts();
        IEnumerable<AdvertViewModel> GetAllNonFeaturedAdverts();
    }

    public class AdvertService : IAdvertService
    {
        private readonly IAdvertRepository _repo;
        private readonly IAdvertValidator _validator;
        private readonly IAdvertMapper _mapper;

        public AdvertService(IAdvertRepository repo, IAdvertValidator validator, IAdvertMapper mapper)
        {
            _repo = repo;
            _validator = validator;
            _mapper = mapper;
        }

        public IEnumerable<AdvertViewModel> GetAllAdverts()
        {
            var entityList = _repo.GetAllAdverts();
            return entityList.Select(ad => _mapper.ViewMapper(ad)).Where(ad => ad.State != "Deleted");
        }

        public AdvertViewModel GetAdvertById(int advertId)
        {
            var entity = _repo.GetAdvertById(advertId);
            
            if (entity == null || entity.State == "Deleted")
                throw new NotFoundApiException("Advert does not exist");

            return _mapper.ViewMapper(entity);
        }

        public IEnumerable<ProvinceViewModel> GetAllProvinces()
        {
            var entityList = _repo.GetAllProvinces();
            return entityList.Select(province => _mapper.ProvinceMapper(province));
        }

        public IEnumerable<CityViewModel> GetAllCities()
        {
            var entityList = _repo.GetAllCities();
            return entityList.Select(city => _mapper.CityMapper(city));
        }

        public IEnumerable<CityViewModel> GetAllProvinceCities(int provinceId)
        {
            var entityList = _repo.GetAllProvinceCities(provinceId);
            return entityList.Select(city => _mapper.CityMapper(city));
        }

        public IEnumerable<AdvertViewModel> SearchFeaturedAdverts(AdvertSearchModel model)
        {
        
            var entityList = _repo.GetAllAdverts();
            entityList = entityList.Where(ad => ad.State == "Live" && ad.Featured == true);

            var returnList = FilterSearch(entityList, model);

            return returnList.Select(advert => _mapper.ViewMapper(advert));
        }

        public IEnumerable<AdvertViewModel> SearchNonFeaturedAdverts(AdvertSearchModel model)
        {

            var entityList = _repo.GetAllAdverts();
            entityList = entityList.Where(ad => ad.State == "Live" && ad.Featured == false);

            var returnList = FilterSearch(entityList, model);

            return returnList.Select(advert => _mapper.ViewMapper(advert));
        }

        public IEnumerable<AdvertViewModel> GetAllFeaturedAdverts()
        {
            var entityList = _repo.GetAllAdverts();
            entityList = entityList.Where(ad => ad.Featured == true);
            entityList = entityList.OrderByDescending(ad => ad.Date);
            return entityList.Select(advert => _mapper.ViewMapper(advert));
        }

        public IEnumerable<AdvertViewModel> GetAllNonFeaturedAdverts()
        {
            var entityList = _repo.GetAllAdverts();
            entityList = entityList.Where(ad => ad.Featured == false);
            entityList = entityList.OrderByDescending(ad => ad.Date);
            return entityList.Select(advert => _mapper.ViewMapper(advert));
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