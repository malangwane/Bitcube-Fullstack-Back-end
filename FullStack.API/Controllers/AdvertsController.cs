using FullStack.API.Services;
using FullStack.ViewModels.Adverts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullStack.API.Controllers
{
    [Route("adverts")]
    [ApiController]
    public class AdvertsController : ControllerBase
    {
        private readonly IAdvertService _adService;
        public AdvertsController(IAdvertService adService)
        {
            _adService = adService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AdvertViewModel>> GetAllAdverts()
        {
            var adverts = _adService.GetAllAdverts();
            return Ok(adverts);
        }

        [HttpGet("{advertId}")]
        public ActionResult<AdvertViewModel> GetAdvertById(int advertId)
        {
            var adverts = _adService.GetAdvertById(advertId);
            return Ok(adverts);
        }

        [HttpGet("provinces")]
        public ActionResult<IEnumerable<ProvinceViewModel>> GetAllProvinces()
        {
            var provinces = _adService.GetAllProvinces();
            return Ok(provinces);
        }

        [HttpGet("cities")]
        public ActionResult<IEnumerable<CityViewModel>> GetAllCities()
        {
            var cities = _adService.GetAllCities();
            return Ok(cities);
        }

        [HttpGet("provinces/{provinceId}/cities")]
        public ActionResult<IEnumerable<ProvinceViewModel>> GetAllProvinceCities(int provinceId)
        {
            var cities = _adService.GetAllProvinceCities(provinceId);
            return Ok(cities);
        }

        [HttpPost("featured/search")]
        public ActionResult<IEnumerable<AdvertViewModel>> SearchFeaturedAdverts(AdvertSearchModel model)
        {
            var adverts = _adService.SearchFeaturedAdverts(model);
            return Ok(adverts);
        }

        [HttpPost("notfeatured/search")]
        public ActionResult<IEnumerable<AdvertViewModel>> SearchNonFeaturedAdverts(AdvertSearchModel model)
        {
            var adverts = _adService.SearchNonFeaturedAdverts(model);
            return Ok(adverts);
        }

        [HttpGet("featured")]
        public ActionResult<IEnumerable<AdvertViewModel>> GetAllFeaturedAdverts()
        {
            var adverts = _adService.GetAllFeaturedAdverts();
            return Ok(adverts);
        }

        [HttpGet("notfeatured")]
        public ActionResult<IEnumerable<AdvertViewModel>> GetAllNonFeaturedAdverts()
        {
            var adverts = _adService.GetAllNonFeaturedAdverts();
            return Ok(adverts);
        }
    }
}
