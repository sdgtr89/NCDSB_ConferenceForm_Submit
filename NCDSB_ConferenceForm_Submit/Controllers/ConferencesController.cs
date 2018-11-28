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
using PagedList;

namespace NCDSB_ConferenceForm_Submit.Controllers
{
    [Authorize]
    public class ConferencesController : Controller
    {
        private ConferenceFormEntities db = new ConferenceFormEntities();

        // GET: Conferences
        public ActionResult Index(string sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.StartSort = String.IsNullOrEmpty(sortOrder) ? "start_desc" : "";
            ViewBag.EndSort = sortOrder == "End" ? "end_desc" : "End";
            ViewBag.CostSort = sortOrder == "Cost" ? "cost_desc" : "Cost";            

            var conferences = db.Conferences.AsNoTracking()
                .Include(c => c.Address);

            switch (sortOrder)
            {
                case "start_desc":
                    conferences = conferences.OrderByDescending(s => s.StartDate);
                    break;
                case "End":
                    conferences = conferences.OrderBy(s => s.EndDate);
                    break;
                case "end_desc":
                    conferences = conferences.OrderByDescending(s => s.EndDate);
                    break;
                case "Cost":
                    conferences = conferences.OrderBy(s => s.Cost);
                    break;
                case "cost_desc":
                    conferences = conferences.OrderByDescending(s => s.Cost);
                    break;
                default:
                    conferences = conferences.OrderBy(s => s.StartDate);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(conferences.ToPagedList(pageNumber, pageSize));
        }

        // GET: Conferences/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Conference conference = db.Conferences.Find(id);

            if (conference == null)
            {
                return HttpNotFound();
            }

            return View(conference);
        }

        // GET: Conferences/Create
        public ActionResult Create(string addConference)
        {
            ViewBag.addConference = addConference;
            PopulateDropDownList();
            return View();
        }        

        // POST: Conferences/Create
        // To protect from over-posting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,AddressID,Description,StartDate,EndDate," +
            "Cost,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] Conference conference, string addConference)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Conferences.Add(conference);
                    db.SaveChanges();
                    if (String.IsNullOrEmpty(addConference))
                    {
                        //closes the popup window, fixed (kinda lol)
                        return View("Close");
                    }
                    else
                    {
                        //This will close the window
                        return View("Close");
                    }

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
                    ModelState.AddModelError("", "Unable to save changes. Combination of Conference Name and Date must be unique.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }

            PopulateDropDownList();
            return View(conference);
        }

        // GET: Conferences/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Conference conference = db.Conferences.Find(id);

            if (conference == null)
            {
                return HttpNotFound();
            }

            PopulateDropDownList(conference);
            return View(conference);
        }

        // POST: Conferences/Edit/5
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

            var conferenceToUpdate = db.Conferences.Find(id);

            if (TryUpdateModel(conferenceToUpdate, "",
                new string[] { "ID", "Name", "AddressID", "Description", "StartDate", "EndDate", "Cost" }))
            {
                try
                {
                    db.Entry(conferenceToUpdate).State = EntityState.Modified;
                    db.Entry(conferenceToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)// Added for concurrency
                {
                    var entry = ex.Entries.Single();
                    var theValues = (Conference)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("", "Unable to save changes. The Conference was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Conference)databaseEntry.ToObject();
                        if (databaseValues.Name != theValues.Name)
                            ModelState.AddModelError("Name", "Current value: "
                                + databaseValues.Name);
                        if (databaseValues.AddressID != theValues.AddressID)
                            ModelState.AddModelError("AddressID", "Current value: "
                                + db.Addresses.Find(databaseValues.AddressID).SiteName);
                        if (databaseValues.Description != theValues.Description)
                            ModelState.AddModelError("Description", "Current value: "
                                + databaseValues.Description);
                        if (databaseValues.StartDate != theValues.StartDate)
                            ModelState.AddModelError("StartDate", "Current value: "
                                + databaseValues.StartDate);
                        if (databaseValues.EndDate != theValues.EndDate)
                            ModelState.AddModelError("EndDate", "Current value: "
                                + databaseValues.EndDate);
                        if (databaseValues.Cost != theValues.Cost)
                            ModelState.AddModelError("Cost", "Current value: "
                                + databaseValues.Cost);
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                + "was modified by another user after you received your values. The "
                                + "edit operation was canceled and the current values in the database "
                                + "have been displayed. If you still want to save your version of this record, click "
                                + "the Save button again.");
                        conferenceToUpdate.RowVersion = databaseValues.RowVersion;
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
                        ModelState.AddModelError("", "Unable to save changes. Combination of Conference Name and Date must be unique.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                    }
                }
            }

            PopulateDropDownList(conferenceToUpdate);
            return View(conferenceToUpdate);
        }

        // GET: Conferences/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conference conference = db.Conferences.Find(id);
            if (conference == null)
            {
                return HttpNotFound();
            }
            return View(conference);
        }

        // POST: Conferences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Conference conference = db.Conferences.Find(id);
            db.Entry(conference).State = EntityState.Modified;
            conference.IsRemoved = true;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected void PopulateDropDownList(Conference conference = null)
        {
            var conferences = from c in db.Addresses
                              join a in db.Cities
                              on c.CityID equals a.ID
                              orderby c.SiteName
                              select c;

            ViewBag.AddressID = new SelectList(db.Addresses
                .OrderBy(m=>m.SiteName), 
                "ID", "FullAddress", conference?.AddressID);
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
