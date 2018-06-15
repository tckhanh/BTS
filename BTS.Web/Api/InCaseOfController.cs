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
    [RoutePrefix("api/inCaseOf")]
    public class InCaseOfController : ApiControllerBase
    {
        #region Initialize

        private IInCaseOfService _inCaseOfService;

        public InCaseOfController(IErrorService errorService, IInCaseOfService inCaseOfService) : base(errorService)
        {
            this._inCaseOfService = inCaseOfService;
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
                var listInCaseOf = _inCaseOfService.getAll(keyword);

                totalRow = listInCaseOf.Count();

                var query = listInCaseOf.OrderBy(x => x.ID).Skip(page * pageSize).Take(pageSize);

                var listInCaseOfVm = Mapper.Map<List<InCaseOfViewModel>>(query);

                var paginationSet = new PaginationSet<InCaseOfViewModel>
                {
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize),
                    Items = listInCaseOfVm
                };

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, paginationSet);

                return response;
            });
        }

        [Route("getbyid/{id}")]
        [HttpGet]
        //[Authorize(Roles = "*")]
        public HttpResponseMessage GetByID(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var dbInCaseOf = _inCaseOfService.getByID(id);

                var dbInCaseOfVm = Mapper.Map<InCaseOf, InCaseOfViewModel>(dbInCaseOf);

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, dbInCaseOfVm);

                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        //[Authorize(Roles = "*")]
        public HttpResponseMessage Post(HttpRequestMessage request, InCaseOfViewModel inCaseOfVm)
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
                    InCaseOf newInCaseOf = new InCaseOf();
                    newInCaseOf.UpdateInCaseOf(inCaseOfVm);
                    newInCaseOf = _inCaseOfService.Add(newInCaseOf);
                    _inCaseOfService.Save();

                    var responseData = Mapper.Map<InCaseOf, InCaseOfViewModel>(newInCaseOf);

                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        //[Authorize(Roles = "*")]
        public HttpResponseMessage Put(HttpRequestMessage request, InCaseOfViewModel inCaseOfVm)
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
                    var dbInCaseOf = _inCaseOfService.getByID(inCaseOfVm.Id);
                    dbInCaseOf.UpdateInCaseOf(inCaseOfVm);

                    _inCaseOfService.Update(dbInCaseOf);
                    _inCaseOfService.Save();

                    var responData = Mapper.Map<InCaseOf, InCaseOfViewModel>(dbInCaseOf);
                    response = request.CreateResponse(HttpStatusCode.OK, responData);
                }
                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        //[Authorize(Roles = "*")]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var dbInCaseOf = _inCaseOfService.Delete(id);
                _inCaseOfService.Save();

                var responData = Mapper.Map<InCaseOf, InCaseOfViewModel>(dbInCaseOf);
                response = request.CreateResponse(HttpStatusCode.OK, responData);

                return response;
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        //[Authorize(Roles = "*")]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedInCaseOfs)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                var listInCaseOfs = new JavaScriptSerializer().Deserialize<List<string>>(checkedInCaseOfs);
                foreach (var item in listInCaseOfs)
                {
                    _inCaseOfService.Delete(item);
                }
                _inCaseOfService.Save();

                response = request.CreateResponse(HttpStatusCode.OK, listInCaseOfs.Count);

                return response;
            });
        }
    }
}