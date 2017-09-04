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
    [RoutePrefix("api/Operator")]
    public class OperatorController : ApiControllerBase
    {
        IOperatorService _operatorService;

        public OperatorController(IErrorService errorService, IOperatorService operatorService) : base(errorService)
        {
            this._operatorService = operatorService;
        }


        [Route("getall")]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            int totalRow = 10;

            return CreateHttpResponse(request, () =>
            {
                var listOperator = _operatorService.getAll(out totalRow);

                var listOperatorVm = Mapper.Map<List<OperatorViewModel>>(listOperator);

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, listOperatorVm);

                return response;
            });
        }

        [Route("add")]
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

                    response = request.CreateResponse(HttpStatusCode.Created, newOperator);

                }
                return response;
            });
        }

        [Route("update")]
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
                    var Operator = _operatorService.getByID(operatorVm.ID);
                    Operator.UpdateOperator(operatorVm);

                    _operatorService.Update(Operator);
                    _operatorService.Save();
                    response = request.CreateResponse(HttpStatusCode.OK);

                }
                return response;
            });
        }

        public HttpResponseMessage Delete(HttpRequestMessage request, string id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    _operatorService.Delete(id);
                    _operatorService.Save();
                    response = request.CreateResponse(HttpStatusCode.OK);

                }
                return response;
            });
        }
    }
}
