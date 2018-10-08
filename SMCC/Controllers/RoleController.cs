using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SMCC.Models;
using Microsoft.AspNet.Identity;

namespace SMCC.Controllers
{
    public class RoleController : Controller
    {
        private Entities db = new Entities();

        // GET: Role
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {            
            return View(db.AspNetUsers.ToList());
        }


        // GET: AspNetRoles/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AspNetUser user = db.AspNetUsers.Find(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            IEnumerable<AspNetRole> roles = db.AspNetRoles.ToList().Except(db.AspNetUsers.SingleOrDefault(u => u.Id == id).AspNetRoles.ToList());
            ViewBag.Id = new SelectList(roles, "Id", "Name");
            TempData["UserId"] = id;

            return View();
        }

        // POST: AspNetRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AspNetRole role)
        {
            AspNetUser user = db.AspNetUsers.Find((string)TempData.Peek("UserId"));

            if (user != null)
            {
                user.AspNetRoles.Add(db.AspNetRoles.Find(role.Id));
                db.SaveChanges();
            }

            IEnumerable<AspNetRole> roles = db.AspNetRoles.ToList().Except(user.AspNetRoles.ToList());
            ViewBag.Id = new SelectList(roles, "Id", "Name");

            return View();
        }


        // GET: Articles/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Remove(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AspNetUser user = db.AspNetUsers.Find(id);

            if (user == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.Id = new SelectList(user.AspNetRoles, "Id", "Name");
            TempData["UserId"] = id;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Remove(AspNetRole role)
        {
            AspNetUser user = db.AspNetUsers.Find((string)TempData.Peek("UserId"));

            if (user != null)
            {
                user.AspNetRoles.Remove(db.AspNetRoles.Find(role.Id));
                db.SaveChanges();
            }
            
            ViewBag.Id = new SelectList(user.AspNetRoles, "Id", "Name");

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        // GET: Role/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            ViewBag.Roles = new SelectList(db.AspNetRoles, "Id", "Name");

            if (id == null)
            {
                ViewBag.Error = "User Id Missing";
            }

            AspNetUser user = db.AspNetUsers.Find(id);

            if (user == null)
            {
                ViewBag.Error = "User Id Not Matched.";
            }
            return View(user);
        }

        // POST: Role/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email")] AspNetUser user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }
    }
}
