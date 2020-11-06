using BotDetect.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BTS.Common;
using BTS.Web.App_Start;
using BTS.Web.Models;
using BTS.Service;
using BTS.Data.ApplicationModels;
using BTS.Web.Infrastructure.Extensions;
using SKGL;
using BTS.Web.Infrastructure.Core;
using AutoMapper;
using BTS.Model.Models;
using static BTS.Web.Infrastructure.Helpers.SessionStateHelpers;
using BTS.Web.Infrastructure.Helpers;

namespace BTS.Web.Areas.Controllers
{
    public class AccountController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ILicenceService _licenceService;
        private IConfigService _configService;

        public AccountController(IErrorService errorService, ILicenceService licenceService, IConfigService configService) : base(errorService)
        {
            _licenceService = licenceService;
            _configService = configService;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        // GET: Account
        public ActionResult Login(string returnUrl)
        {
            LoginViewModel model = new LoginViewModel();
            model.ReturnUrl = returnUrl;
            if (checkLicence.isValid() == true)
                return View(model);
            else
                return View("NoLicence");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.Find<ApplicationUser, string>(model.UserName, model.Password);

                if (user != null)
                {
                    if (user.Locked == false)
                    {
                        IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                        authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                        ClaimsIdentity identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

                        //identity.AddClaim(new Claim("FullName", user.FullName));
                        //identity.AddClaim(new Claim("Email", user.Email));
                        if (_configService.getByCode(CommonConstants.EnableCityIDsScope) != null)
                        {
                            identity.AddClaim(new Claim(CommonConstants.EnableCityIDsScope, _configService.getByCode(CommonConstants.EnableCityIDsScope)?.ValueString));
                        }
                        else
                        {
                            identity.AddClaim(new Claim(CommonConstants.EnableCityIDsScope, "False"));
                        }
                            
                        identity.AddClaim(new Claim("CityIDsScope", user.CityIDsScope ?? ""));
                        identity.AddClaim(new Claim("ImagePath", user.ImagePath ?? ""));

                        // Session["CityIDsScope"] = user.CityIDsScope ?? "";
                        Session["ImagePath"] = user.ImagePath ?? "";                        

                        AuthenticationProperties props = new AuthenticationProperties();
                        props.IsPersistent = model.RememberMe;
                        authenticationManager.SignIn(props, identity);

                        AuditHelpers.Log(0, identity.Name);
                        loadLicense();

                        if (Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Tài khoản đã bị khóa, vui lòng liên hệ nhà quản trị hệ thống.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult SignOut()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);

                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CaptchaValidation("CaptchaCode", "registerCaptcha", "Mã xác nhận không đúng")]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userByEmail = await UserManager.FindByEmailAsync(model.Email);
                if (userByEmail != null)
                {
                    ModelState.AddModelError("email", "Email đã tồn tại");
                    return View(model);
                }
                var userByUserName = await UserManager.FindByNameAsync(model.UserName);
                if (userByUserName != null)
                {
                    ModelState.AddModelError("email", "Tài khoản đã tồn tại");
                    return View(model);
                }
                var user = new ApplicationUser()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    EmailConfirmed = true,
                    BirthDay = DateTime.Now,
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address
                };

                await UserManager.CreateAsync(user, model.Password);

                var adminUser = await UserManager.FindByEmailAsync(model.Email);
                if (adminUser != null)
                    await UserManager.AddToRolesAsync(adminUser.Id, new string[] { "User" });

                string content = System.IO.File.ReadAllText(Server.MapPath("/Assets/client/template/newuser.html"));
                content = content.Replace("{{UserName}}", adminUser.FullName);
                content = content.Replace("{{Link}}", ConfigHelper.GetByKey("CurrentLink") + "dang-nhap.html");

                MailHelper.SendMail(adminUser.Email, "Đăng ký thành công", content);

                ViewData["SuccessMsg"] = "Đăng ký thành công";
            }

            return View();
        }


        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion Helpers

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ViewAll()
        {
            return View(GetAll());
        }

        [AllowAnonymous]
        private IEnumerable<LicenceViewModel> GetAll()
        {
            List<Licence> model = _licenceService.getAll().ToList();
            List<LicenceViewModel> viewModel = new List<LicenceViewModel>();
            foreach (var item in model)
            {
                viewModel.Add(checkLicence.GetLicenceInfo(item));
            }
            return viewModel;
        }

        [AllowAnonymous]
        public ActionResult Add()
        {
            LicenceViewModel ItemVm = new LicenceViewModel();
            return View(ItemVm);
        }

        [AllowAnonymous]
        public ActionResult Detail(string id = "0")
        {
            LicenceViewModel ItemVm = new LicenceViewModel();
            Licence DbItem = _licenceService.getByID(id);
            if (DbItem != null)
            {
                ItemVm = Mapper.Map<LicenceViewModel>(DbItem);
            }
            return View(ItemVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Add(LicenceViewModel Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Licence newItem = new Licence();
                    newItem.UpdateLicence(Item);

                    newItem.CreatedBy = User.Identity.Name;
                    newItem.CreatedDate = DateTime.Now;

                    _licenceService.Add(newItem);
                    _licenceService.Save();
                    return Json(new { resetUrl = Url.Action("Add", "Account"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<LicenceViewModel>>(GetAll())), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { resetUrl = Url.Action("Add", "Account"), status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "Account"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}