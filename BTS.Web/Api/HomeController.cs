using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BTS.Service;
using BTS.Web.Infrastructure.Core;

namespace BTS.Web.Api
{
    [RoutePrefix("api/home")]
    [Authorize]
    public class HomeController : BaseController
    {
        private IErrorService _errorService;

        public HomeController(IErrorService errorService) : base(errorService)
        {
            this._errorService = errorService;
        }

        [HttpGet]
        [Route("testMethod")]
        public string TestMethod()
        {
            return "Hello, TEDU Member. ";
        }
    }
}