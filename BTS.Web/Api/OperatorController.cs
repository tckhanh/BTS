using AutoMapper;
using BTS.Model.Models;
using BTS.Service;
using BTS.Web.Infastructure.Core;
using BTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BTS.Web.Infastructure.Extensions;

namespace BTS.Web.Api
{
    [RoutePrefix("api/operator")]
    public class OperatorController : ApiControllerBase
    {
        #region Initialize
        IOperatorService _operatorService;

        public OperatorController(IErrorService errorService, IOperatorService operatorService) : base(errorService)
        {
            this._operatorService = operatorService;
        }

        #endregion

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword = "", int page = 0, int pageSize = 20)
        {
            int totalRow = 0;

            return CreateHttpResponse(request, () =>
            {

                var listOperator = _operatorService.getAll(keyword);

                totalRow = listOperator.Count();

                var query = listOperator.OrderBy(x => x.ID).Skip(page * pageSize).Take(pageSize);

                var listOperatorVm = Mapper.Map<List<OperatorViewModel>>(query);

                var paginationSet = new PaginationSet<OperatorViewModel>
                {
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize),
                    items = listOperatorVm
                };

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, paginationSet);

                return response;
            });
        }


        [Route("getbyid/{id}")]
        [HttpGet]
        public HttpResponseMessage GetByID(HttpRequestMessage request, string id)
        {

            return CreateHttpResponse(request, () =>
            {

                var dbOperator = _operatorService.getByID(id);

                var dbOperatorVm = Mapper.Map<Operator, OperatorViewModel>(dbOperator);

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, dbOperatorVm);

                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Post(HttpRequestMessage request, OperatorViewModel operatorVm)
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
                    Operator newOperator = new Operator();
                    newOperator.UpdateOperator(operatorVm);
                    newOperator = _operatorService.Add(newOperator);
                    _operatorService.Save();

                    var responseData = Mapper.Map<Operator, OperatorViewModel>(newOperator);

                    response = request.CreateResponse(HttpStatusCode.Created, responseData);

                }
                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Put(HttpRequestMessage request, OperatorViewModel operatorVm)
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
                    var dbOperator = _operatorService.getByID(operatorVm.ID);
                    dbOperator.UpdateOperator(operatorVm);

                    _operatorService.Update(dbOperator);
                    _operatorService.Save();

                    var responData = Mapper.Map<Operator, OperatorViewModel>(dbOperator);
                    response = request.CreateResponse(HttpStatusCode.OK, responData);

                }
                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, string id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var dbOperator = _operatorService.Delete(id);
                _operatorService.Save();

                var responData = Mapper.Map<Operator, OperatorViewModel>(dbOperator);
                response = request.CreateResponse(HttpStatusCode.OK, responData);

                return response;
            });
        }
    }
}
