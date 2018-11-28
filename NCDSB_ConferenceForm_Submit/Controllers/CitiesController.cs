using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NCDSB_ConferenceForm_Submit.DAL;
using NCDSB_ConferenceForm_Submit.Models;

namespace NCDSB_ConferenceForm_Submit.Controllers
{
    [Authorize]
    public class CitiesController : Controller
    {
        private ConferenceFormEntities db = new ConferenceFormEntities();

        // GET: Cities
        public ActionResult Index()
        {
            return View(db.Cities.ToList());
        }

        // GET: Cities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            City city = db.Cities.Find(id);

            if (city == null)
            {
                return HttpNotFound();
            }

            return View(city);
        }

        // GET: Cities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] City city)
        {  
            try
            {
                if (ModelState.IsValid)
                {
                    db.Cities.Add(city);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
            }
            catch (DataException dex)
            {
                if (dex.InnerException.InnerException.Message.Contains("IX_"))
                {
                    ModelState.AddModelError("", "Unable to save changes. The city name must be unique.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
                }
            }

            return View(city);
        }

        // GET: Cities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            City city = db.Cities.Find(id);

            if (city == null)
            {
                return HttpNotFound();
            }

            return View(city);
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id, Byte[] rowVersion)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var cityToUpdate = db.Cities.Find(id);

            if (TryUpdateModel(cityToUpdate, "", new string[] { "ID", "Name" }))
            {
                try
                {

                    db.Entry(cityToUpdate).State = EntityState.Modified;
                    db.Entry(cityToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var theValues = (City)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();

                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("", "Unable to save changes. The city was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (City)databaseEntry.ToObject();

                        if (databaseValues.Name != theValues.Name)
                            ModelState.AddModelError("Name", "Current value: "
                                + databaseValues.Name);

                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                + "was modified by another user after you received your values. The "
                                + "edit operation was canceled and the current values in the database "
                                + "have been displayed. If you still want to save your version of this "
                                + "record, click the Save button again.");

                        cityToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (DataException dex)
                {
                    if (dex.InnerException.InnerException.Message.Contains("IX_"))
                    {
                        ModelState.AddModelError("", "Unable to save changes. The city name must be unique.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
                    }
                }
            }
            return View(cityToUpdate);
        }

        // GET: Cities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            City city = db.Cities.Find(id);

            if (city == null)
            {
                return HttpNotFound();
            }

            return View(city);
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            City city = db.Cities.Find(id);
            db.Cities.Remove(city);
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
