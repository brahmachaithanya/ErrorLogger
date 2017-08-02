using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using WebApplication.Models;


namespace WebApplication.App_Start
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(() => new MyDbContext());
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);
            //app.CreatePerOwinContext<RoleManager<AppRole>>((options, context) =>
            //    new RoleManager<AppRole>(
            //        new RoleStore<AppRole>(context.Get<MyDbContext>())));

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Home/Login"),
            });
            InitializeUsersAndRoleAsync();

        }
        public Boolean InitializeUsersAndRoleAsync()
        {
            var context = new MyDbContext();
            //var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            var userManager = new AppUserManager(new UserStore<user>(context));
            var roleManager = new RoleManager<AppRole>(new RoleStore<AppRole>(context));
            var appUserIdentity = new user() { UserName = "brahma" };

            var result = userManager.Create(appUserIdentity, "password");
            if (result.Succeeded)
            {
                if (!roleManager.RoleExists("role"))
                {
                    var role = new AppRole("Admin");
                    roleManager.Create(role);
                    userManager.AddToRole(appUserIdentity.Id, "Admin");
                }
                return true;
            }
            return false;
        }


    }
}
