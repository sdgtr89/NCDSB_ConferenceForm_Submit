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
    public class AddressesController : Controller
    {
        private ConferenceFormEntities db = new ConferenceFormEntities();

        // GET: Addresses
        public ActionResult Index()
        {
            var addresses = db.Addresses.AsNoTracking()
                .Include(a => a.City);

            return View(addresses.ToList());
        }

        // GET: Addresses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Address address = db.Addresses.Find(id);

            if (address == null)
            {
                return HttpNotFound();
            }

            return View(address);
        }

        // GET: Addresses/Create
        public ActionResult Create()
        {
            PopulateCityDropDownList();
            return View();
        }

        // POST: Addresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,SiteName,StreetAddress,CityID,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] Address address)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Addresses.Add(address);
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
                    ModelState.AddModelError("", "Unable to save changes. Remember, you cannot have sites with the same name.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }

            PopulateCityDropDownList();
            return View(address);
        }

        // GET: Addresses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Address address = db.Addresses.Find(id);

            if (address == null)
            {
                return HttpNotFound();
            }

            PopulateCityDropDownList(address);

            return View(address);
        }

        // POST: Addresses/Edit/5
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

            var addressToUpdate = db.Addresses.Find(id);

            if (TryUpdateModel(addressToUpdate, "",
                   new string[] { "ID", "SiteName", "StreetAddress", "CityID" }))
            {
                try
                {
                    db.Entry(addressToUpdate).State = EntityState.Modified;
                    db.Entry(addressToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)// Added for concurrency
                {
                    var entry = ex.Entries.Single();
                    var theValues = (Address)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("", "Unable to save changes. The Address was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Address)databaseEntry.ToObject();
                        if (databaseValues.SiteName != theValues.SiteName)
                            ModelState.AddModelError("SiteName", "Current value: "
                                + databaseValues.SiteName);
                        if (databaseValues.StreetAddress != theValues.StreetAddress)
                            ModelState.AddModelError("StreetAddress", "Current value: "
                                + databaseValues.StreetAddress);
                        if (databaseValues.CityID != theValues.CityID)
                            ModelState.AddModelError("CityID", "Current value: "
                                + db.Cities.Find(databaseValues.CityID).Name);
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                + "was modified by another user after you received your values. The "
                                + "edit operation was canceled and the current values in the database "
                                + "have been displayed. If you still want to save your version of this record, click "
                                + "the Save button again.");
                        addressToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
                }
                catch (DataException dex)
                {
                    if (dex.InnerException.InnerException.Message.Contains("IX_"))
                    {
                        ModelState.AddModelError("", "Unable to save changes. Remember, you cannot have sites with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                    }
                }
            }
            PopulateCityDropDownList(addressToUpdate);
            return View(addressToUpdate);
        }

        // GET: Addresses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Address address = db.Addresses.Find(id);

            if (address == null)
            {
                return HttpNotFound();
            }

            return View(address);
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Address address = db.Addresses.Find(id);
            db.Addresses.Remove(address);
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

        protected void PopulateCityDropDownList(Address address = null)
        {
            ViewBag.CityID = new SelectList(db.Cities
                .OrderBy(c=> c.Name), 
                "ID", "Name", address?.CityID);
        }
    }
}
