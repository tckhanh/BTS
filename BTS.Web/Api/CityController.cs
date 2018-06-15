using AutoMapper;
using BTS.Model.Models;
using BTS.Service;
using BTS.Web.Infrastructure.Core;
using BTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BTS.Web.Infrastructure.Extensions;
using System.Web.Script.Serialization;

namespace BTS.Web.Api
{
    [RoutePrefix("api/city")]
    public class CityController : ApiControllerBase
    {
        #region Initialize

        private ICityService _cityService;

        public CityController(IErrorService errorService, ICityService cityService) : base(errorService)
        {
            this._cityService = cityService;
        }

        #endregion Initialize

        [Route("getall")]
        [HttpGet]
        //[Authorize(Roles = "*")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword = "", int page = 0, int pageSize = 20)
        {
            int totalRow = 0;

            return CreateHttpResponse(request, () =>
            {
                var listCity = _cityService.getAll(keyword);

                totalRow = listCity.Count();

                var query = listCity.OrderBy(x => x.ID).Skip(page * pageSize).Take(pageSize);

                var listCityVm = Mapper.Map<List<CityViewModel>>(query);

                var paginationSet = new PaginationSet<CityViewModel>
                {
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize),
                    Items = listCityVm
                };

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, paginationSet);

                return response;
            });
        }

        [Route("getbyid/{id}")]
        [HttpGet]
        //[Authorize(Roles = "*")]
        public HttpResponseMessage GetByID(HttpRequestMessage request, string id)
        {
            return CreateHttpResponse(request, () =>
            {
                var dbCity = _cityService.getByID(id);

                var dbCityVm = Mapper.Map<City, CityViewModel>(dbCity);

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, dbCityVm);

                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        //[Authorize(Roles = "*")]
        public HttpResponseMessage Post(HttpRequestMessage request, CityViewModel cityVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    City newCity = new City();
                    newCity.UpdateCity(cityVm);
                    newCity = _cityService.Add(newCity);
                    _cityService.Save();

                    var responseData = Mapper.Map<City, CityViewModel>(newCity);

                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        //[Authorize(Roles = "*")]
        public HttpResponseMessage Put(HttpRequestMessage request, CityViewModel cityVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var dbCity = _cityService.getByID(cityVm.Id);
                    dbCity.UpdateCity(cityVm);

                    _cityService.Update(dbCity);
                    _cityService.Save();

                    var responData = Mapper.Map<City, CityViewModel>(dbCity);
                    response = request.CreateResponse(HttpStatusCode.OK, responData);
                }
                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        //[Authorize(Roles = "*")]
        public HttpResponseMessage Delete(HttpRequestMessage request, string id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var dbCity = _cityService.Delete(id);
                _cityService.Save();

                var responData = Mapper.Map<City, CityViewModel>(dbCity);
                response = request.CreateResponse(HttpStatusCode.OK, responData);

                return response;
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        //[Authorize(Roles = "*")]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedCitys)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                var listCities = new JavaScriptSerializer().Deserialize<List<string>>(checkedCitys);
                foreach (var item in listCities)
                {
                    _cityService.Delete(item);
                }
                _cityService.Save();

                response = request.CreateResponse(HttpStatusCode.OK, listCities.Count);

                return response;
            });
        }
    }
}