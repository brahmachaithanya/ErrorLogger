using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
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


namespace WebApplication.Controllers
{
    public class usersController : Controller
    {
        private bsadashiDataBaseEntities db = new bsadashiDataBaseEntities();
        [Authorize]
        
        [HandleError]
        // GET: users
        public ActionResult Index()
        {
            return View(db.users.ToList());
        }
        [Authorize]
        [HandleError]
        // GET: users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataBase.user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: users/Create
        public ActionResult Create()
        {
            return View();
        }
        [Authorize]
        // POST: users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "username,password,is_admin,finstname,lastname,email,sign_up_date,last_login,active_inactive")] DataBase.user user)
        {
            if (ModelState.IsValid)
            {
                db.users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        [Authorize]
        [HandleError]
        public  ActionResult Disable(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataBase.user urs = db.users.Find(id);

            if (urs == null)
            {
                return HttpNotFound();
            }
            if (urs.active_inactive.Trim() == "inactive")
            {
                urs.active_inactive = "active";
            }
            else
            {
                urs.active_inactive = "inactive";
            }
            db.SaveChanges();
            var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            var user = userManager.FindByNameAsync(urs.username);
            string userId = user.Id.ToString();
            userManager.SetLockoutEnabledAsync(userId, true);
            userManager.SetLockoutEndDateAsync(userId, DateTime.Today.AddYears(10));




            return RedirectToAction("Index", "users");
        }

        // GET: users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataBase.user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "username,password,is_admin,finstname,lastname,email,sign_up_date,last_login,active_inactive")] DataBase.user user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataBase.user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DataBase.user user = db.users.Find(id);
            db.users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
