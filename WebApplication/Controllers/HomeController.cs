using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using DataLayer;
using DataBase;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using WebApplication.Models;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Net;

namespace WebApplication.Controllers
{
    
    public class HomeController : Controller
    {
        [Authorize]
        [HandleError]
        
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View("HomePage");
        }

        public ActionResult Login()
        {
            return View("Login");

            
        }
        [Authorize]
        [HandleError]

        public ActionResult HomePage()
        {
            return View("HomePage");
        }

        [HandleError]
        public ActionResult Logout()
        {
           
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
            return View("Login");
        }
        public ActionResult ErrorLog()
        {
            return View("ErrorLog");
        }
        public ActionResult UserList()
        {
            UserDataHandler userdata = new UserDataHandler();
            ICollection<usermodel> data = userdata.GetAllUsers();
             return View(data);
        }
        public ActionResult app_list()
        {
            AppDataHandler appdata = new AppDataHandler();
            ICollection<application> data = appdata.GetAllApps();
            return View(data);
            
        }
        //public ActionResult AppSearch(string user)
        //{
        //    AppDataHandler appdata = new AppDataHandler();
        //    ICollection<appmodel> data = appdata.GetAppByUser(user);

        //    return View(data);
        //}
        [HttpPost]
       
        [HandleError]
        public ActionResult Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
                var authManager = HttpContext.GetOwinContext().Authentication;
                bsadashiDataBaseEntities db = new bsadashiDataBaseEntities();
                Models.user user = userManager.Find(login.UserName, login.Password);
                if (user.LockoutEnabled == true)
                {
                    ModelState.AddModelError("", "User is not active");
                    return View(login);
                }
                string active_inactive = db.users.Find(login.UserName).active_inactive.Trim();
                if (active_inactive == "inactive")
                {
                    ModelState.AddModelError("", "User is not active");
                    return View(login);
                }
                if (user != null)
                {
                    var ident = userManager.CreateIdentity(user,DefaultAuthenticationTypes.ApplicationCookie);
                    authManager.SignIn(
                        new AuthenticationProperties { IsPersistent = false }, ident);
                    db.users.Find(login.UserName).last_login = DateTime.Now.ToString();
                    db.SaveChanges();
                    return Redirect(login.ReturnUrl ?? Url.Action("Index", "Home"));
                }
            }
            ModelState.AddModelError("", "Invalid username or password");
            return View(login);
        }

        public ActionResult Register()
        {
            return View("Register");


        }

        [HttpPost]
        
        [HandleError]
        public ActionResult Register(DataBase.user user)
        {
            if (ModelState.IsValid)
            {
                var context = new MyDbContext();
                var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
                var authManager = HttpContext.GetOwinContext().Authentication;
                var roleManager =new RoleManager<AppRole>(new RoleStore<AppRole>(context));
                 bsadashiDataBaseEntities db = new bsadashiDataBaseEntities();
        var appUserIdentity = new Models.user() { UserName = user.username,Email=user.email,EmailConfirmed=true };
              

                var result = userManager.Create(appUserIdentity, user.password);
                if (result.Succeeded)
                {
                    if (!roleManager.RoleExists("User"))
                    {
                        var role = new AppRole("User");
                        roleManager.Create(role);
                        userManager.AddToRole(appUserIdentity.Id, "User");
                    }
                    userManager.AddToRole(appUserIdentity.Id, "User");

                    user.last_login = (DateTime.Now).ToString();
                    user.is_admin = "NO";
                    user.active_inactive = "active";
                    user.sign_up_date = (DateTime.Now).ToString();
                    db.users.Add(user);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException dbEx)
                    {
                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                Trace.TraceInformation("Property: {0} Error: {1}",
                                                        validationError.PropertyName,
                                                        validationError.ErrorMessage);
                            }
                        }
                    }


                    return Redirect(user.ReturnUrl ?? Url.Action("Login", "home"));

                }
                
            }
            ModelState.AddModelError("", "Invalid entries");
            return View(user);
        }

    }

}
