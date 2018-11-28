using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NCDSB_ConferenceForm_Submit.DAL;
using NCDSB_ConferenceForm_Submit.Models;

namespace NCDSB_ConferenceForm_Submit.Controllers
{
    [Authorize]
    public class StaffMembersController : Controller
    {
        private ConferenceFormEntities db = new ConferenceFormEntities();

        // GET: StaffMembers
        public ActionResult Index()
        {
            return RedirectToAction("Details");
        }

        // GET: StaffMembers/Details/5
        public ActionResult Details()
        {            
            StaffMember staffMember = db.Staff
                .Where(s => s.Email == User.Identity.Name).SingleOrDefault();

            if (staffMember == null)
            {
                return RedirectToAction("Create");
            }

            return View(staffMember);
        }

        // GET: StaffMembers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StaffMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,MiddleName,LastName,Position,Email")] StaffMember staffMember)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Staff.Add(staffMember);
                    db.SaveChanges();
                    return RedirectToAction("Details");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(staffMember);
        }

        // GET: StaffMembers/Edit/5
        public ActionResult Edit()
        {
            StaffMember staffMember = db.Staff
               .Where(s => s.Email == User.Identity.Name).SingleOrDefault();

            if (staffMember == null)
            {
                return RedirectToAction("Create");
            }

            return View(staffMember);
        }

        // POST: StaffMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost()
        {
            StaffMember staffMember = db.Staff
                .Where(c => c.Email == User.Identity.Name).SingleOrDefault();

            if (TryUpdateModel(staffMember, "",
               new string[] { "FirstName", "MiddleName", "LastName", "Position" }))
            {
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Details");
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }

            return View(staffMember);
        }

        // GET: StaffMembers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffMember staffMember = db.Staff.Find(id);
            if (staffMember == null)
            {
                return HttpNotFound();
            }
            return View(staffMember);
        }

        // POST: StaffMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StaffMember staffMember = db.Staff.Find(id);
            db.Staff.Remove(staffMember);
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
