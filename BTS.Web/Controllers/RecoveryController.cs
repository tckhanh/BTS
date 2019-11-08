using BTS.Common;
using BTS.Data;
using BTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BTS.Web.Controllers
{
    public class RecoveryController : Controller
    {
        // GET: Recovery
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (model.UserName == CommonConstants.Recovery_Name && model.Password == CommonConstants.Recovery_Password)
                {
                    BTSDbContext DbContext = new BTSDbContext();

                    InitDatabase.SetupRecoveryRolesGroups(DbContext);
                    InitDatabase.GrantDefaultRolesForRecoveryGroup(DbContext);
                    InitDatabase.CreateRecoveryUser(DbContext);

                    if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }

            return View(model);
        }
    }
}