using AutoMapper;
using BTS.Model.Models;
using BTS.Service;
using BTS.Web.Infrastructure.Extensions;
using BTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace BTS.Web.Controllers
{
    public class InCaseOfController : BaseController
    {
        private IInCaseOfService _inCaseOfService;

        public InCaseOfController(IInCaseOfService inCaseOfService, IErrorService errorService) : base(errorService)
        {
            _inCaseOfService = inCaseOfService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult LoadData()
        {
            var model = _inCaseOfService.getAll().ToList();
            var data = Mapper.Map<List<InCaseOfViewModel>>(model);
            int totalRow = data.Count();
            return Json(new
            {
                data = data,
                total = totalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDetail(int id)
        {
            var item = Mapper.Map<InCaseOfViewModel>(_inCaseOfService.getByID(id));
            return Json(new
            {
                data = item,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveData(string strSenderObj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            InCaseOfViewModel itemVm = serializer.Deserialize<InCaseOfViewModel>(strSenderObj);
            InCaseOf item = new InCaseOf();
            item.UpdateInCaseOf(itemVm);

            bool status = false;
            string message = string.Empty;
            //add new employee if id = 0
            if (item.ID == 0)
            {
                item.CreatedDate = DateTime.Now;
                item.CreatedBy = User.Identity.Name;

                _inCaseOfService.Add(item);
                try
                {
                    _inCaseOfService.Save();
                    status = true;
                }
                catch (Exception ex)
                {
                    status = false;
                    message = ex.Message;
                }
            }
            else
            {
                //update existing DB
                //save db
                var entity = _inCaseOfService.getByID(item.ID);
                entity.Code = item.Code;
                entity.Name = item.Name;

                entity.UpdatedDate = DateTime.Now;
                entity.UpdatedBy = User.Identity.Name;

                try
                {
                    _inCaseOfService.Update(entity);
                    _inCaseOfService.Save();
                    status = true;
                }
                catch (Exception ex)
                {
                    status = false;
                    message = ex.Message;
                }
            }

            return Json(new
            {
                status = status,
                message = message
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Delete(int id)
        {
            _inCaseOfService.Delete(id);
            try
            {
                _inCaseOfService.Save();
                return Json(new
                {
                    status = true
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    message = ex.Message
                });
            }
        }
    }
}