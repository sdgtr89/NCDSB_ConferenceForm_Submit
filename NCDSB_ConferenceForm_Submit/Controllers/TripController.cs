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
    public class TripController : Controller
    {
        private ConferenceFormEntities db = new ConferenceFormEntities();

        // GET: Trip
        public ActionResult Index()
        {
            var trips = db.Trips.Include(t => t.EndAddress)
                .Include(t => t.MileageForm)
                .Include(t => t.StartAddress);
            return View(trips.ToList());
        }

        // GET: Trip/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trip trip = db.Trips.Find(id);
            if (trip == null)
            {
                return HttpNotFound();
            }
            return View(trip);
        }

        // GET: Trip/Create
        public ActionResult Add(int mileageFormID, string mileageSummary)
        {
            ViewBag.MileageForm = mileageSummary;
            ViewBag.MileageFormID = mileageFormID;

            Trip trip = new Trip
            {
                MileageFormID = mileageFormID
            };

            PopulateDropDownLists();
            return View();
        }

        // POST: Trip/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "ID,StartAddressID,EndAddressID,Distance,Date,MileageFormID," +
            "CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] Trip trip)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Trips.Add(trip);
                    db.SaveChanges();
                    return RedirectToAction("Details", "MileageForms", new { id = trip.MileageFormID });
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, " +
                    "and if the problem persists, see your system administrator.");
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists " +
                    "contact your system administrator.");
            }
            PopulateDropDownLists(trip);
            return View(trip);
        }

        // GET: Trip/Create For Conference Form
        [HttpGet, ActionName("AddForConference")]
        public ActionResult AddForConference(int conferenceFormID, string conferenceSummary)
        {
            ViewBag.ConferenceForm = conferenceSummary;
            ViewBag.ConferenceFormID = conferenceFormID;

            Trip trip = new Trip
            {
                ConferenceFormID = conferenceFormID
            };

            PopulateDropDownLists();

            return View();
        }        

        // POST: Trip/Create For Conference Form
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("AddForConference")]
        [ValidateAntiForgeryToken]
        public ActionResult AddForConference([Bind(Include = "ID,StartAddressID,EndAddressID,Distance,Date,ConferenceFormID," +
            "CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] Trip trip)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ValidateTrips(trip.ConferenceFormID);
                    db.Trips.Add(trip);
                    db.SaveChanges();
                    return RedirectToAction("Details", "ConferenceForms", new { id = trip.ConferenceFormID });
                }
            }
            catch(TripException tex)
            {
                ModelState.AddModelError("", tex.Message);
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, " +
                    "and if the problem persists, see your system administrator.");
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem " +
                    "persists contact your system administrator.");
            }
            PopulateDropDownLists(trip);
            return View(trip);
        }

        // GET: Trip/Edit/5
        public ActionResult Update(int? id, int mileageFormID, string mileageSummary)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Trip trip = db.Trips.Find(id);

            if (trip == null)
            {
                return HttpNotFound();
            }

            ViewBag.MileageForm = mileageSummary;
            ViewBag.MileageFormID = mileageFormID;

            PopulateDropDownLists(trip);
            return View(trip);
        }

        // POST: Trip/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Update")]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePost(int? id, int mileageFormID, string mileageSummary, Byte[] rowVersion)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tripToUpdate = db.Trips.Find(id);

            if (TryUpdateModel(tripToUpdate, "", new string[] { "ID", "StartAddressID", "EndAddressID", "Distance", "Date", "MileageFormID" }))
            {
                try
                {
                    db.Entry(tripToUpdate).State = EntityState.Modified;
                    db.Entry(tripToUpdate).OriginalValues["RowVersion"] = rowVersion;                    
                    db.SaveChanges();
                    return RedirectToAction("Details", "MileageForms", new { id = mileageFormID });
                }

                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
                }                
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var theValues = (Trip)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();

                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("", "Unable to save changes. The form status was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Trip)databaseEntry.ToObject();

                        if (databaseValues.StartAddressID != theValues.StartAddressID)
                            ModelState.AddModelError("StartAddressID", "Current value: "
                                + db.Addresses.Find(databaseValues.StartAddressID).SiteName);
                        if (databaseValues.EndAddressID != theValues.EndAddressID)
                            ModelState.AddModelError("EndAddressID", "Current value: "
                                + db.Addresses.Find(databaseValues.EndAddressID).SiteName);
                        if (databaseValues.Distance != theValues.Distance)
                            ModelState.AddModelError("Distance", "Current value: "
                                + databaseValues.Distance);
                        if (databaseValues.Date != theValues.Date)
                            ModelState.AddModelError("Date", "Current value: "
                                + databaseValues.Date);
                        if (databaseValues.MileageFormID != theValues.MileageFormID)
                            ModelState.AddModelError("MileageFormID", "Current value: "
                                + db.MileageForms.Find(databaseValues.MileageFormID).StaffMember);
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                + "was modified by another user after you received your values. The "
                                + "edit operation was canceled and the current values in the database "
                                + "have been displayed. If you still want to save your version of this "
                                + "record, click the Save button again.");

                        tripToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
                }
            }
            PopulateDropDownLists(tripToUpdate);
            return View(tripToUpdate);
        }

        // GET: Trip/Edit/5 For Conference Form
        public ActionResult UpdateForConference(int? id, int conferenceFormID, string conferenceSummary)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Trip trip = db.Trips.Find(id);

            if (trip == null)
            {
                return HttpNotFound();
            }

            ViewBag.ConferenceSummary = conferenceSummary;
            ViewBag.ConferenceFormID = conferenceFormID;

            PopulateDropDownLists(trip);
            return View(trip);
        }        

        // POST: Trip/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("UpdateForConference")]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateForConferencePost(int? id, int conferenceFormID, string conferenceSummary, Byte[] rowVersion)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tripToUpdate = db.Trips.Find(id);

            if (TryUpdateModel(tripToUpdate, "", new string[] { "ID", "StartAddressID", "EndAddressID", "Distance", "Date", "ConferenceFormID" }))
            {
                try
                {
                    db.Entry(tripToUpdate).State = EntityState.Modified;
                    db.Entry(tripToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    db.SaveChanges();
                    return RedirectToAction("Details", "ConferenceForms", new { id = conferenceFormID });
                }

                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, " +
                        "and if the problem persists, see your system administrator.");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var theValues = (Trip)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();

                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("", "Unable to save changes. The form status was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Trip)databaseEntry.ToObject();

                        if (databaseValues.StartAddressID != theValues.StartAddressID)
                            ModelState.AddModelError("StartAddressID", "Current value: "
                                + db.Addresses.Find(databaseValues.StartAddressID).SiteName);
                        if (databaseValues.EndAddressID != theValues.EndAddressID)
                            ModelState.AddModelError("EndAddressID", "Current value: "
                                + db.Addresses.Find(databaseValues.EndAddressID).SiteName);
                        if (databaseValues.Distance != theValues.Distance)
                            ModelState.AddModelError("Distance", "Current value: "
                                + databaseValues.Distance);
                        if (databaseValues.Date != theValues.Date)
                            ModelState.AddModelError("Date", "Current value: "
                                + databaseValues.Date);
                        if (databaseValues.MileageFormID != theValues.MileageFormID)
                            ModelState.AddModelError("MileageFormID", "Current value: "
                                + db.MileageForms.Find(databaseValues.MileageFormID).StaffMember);
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                + "was modified by another user after you received your values. The "
                                + "edit operation was canceled and the current values in the database "
                                + "have been displayed. If you still want to save your version of this "
                                + "record, click the Save button again.");

                        tripToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem " +
                        "persists contact your system administrator.");
                }
            }

            PopulateDropDownLists(tripToUpdate);
            return View(tripToUpdate);
        }

        // GET: Trip/Delete/5
        public ActionResult Remove(int? id, int? mileageFormID, string mileageSummary)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trip trip = db.Trips.Find(id);
            if (trip == null)
            {
                return HttpNotFound();
            }

            ViewBag.MileageForm = mileageSummary;
            ViewBag.MileageFormID = mileageFormID;

            return View(trip);
        }

        // POST: Trip/Delete/5
        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveConfirmed(int id, int? mileageFormID, string mileageSummary)
        {
            Trip trip = db.Trips.Find(id);
            db.Trips.Remove(trip);
            trip.IsRemoved = true;
            db.SaveChanges();
            ViewBag.MileageForm = mileageSummary;
            ViewBag.MileageFormID = mileageFormID;
            return RedirectToAction("Details", "MileageForms", new { id = mileageFormID });
        }

        // GET: Trip/Delete/5
        public ActionResult RemoveForConference(int? id, int? conferenceFormID, string conferenceSummary)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trip trip = db.Trips.Find(id);
            if (trip == null)
            {
                return HttpNotFound();
            }

            ViewBag.ConferenceForm = conferenceSummary;
            ViewBag.ConferenceFormID = conferenceFormID;

            return View(trip);
        }

        // POST: Trip/Delete/5
        [HttpPost, ActionName("RemoveForConference")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveForConferenceConfirmed(int id, int? conferenceFormID, string conferenceSummary)
        {
            Trip trip = db.Trips.Find(id);
            db.Trips.Remove(trip);
            trip.IsRemoved = true;
            db.SaveChanges();
            ViewBag.ConferenceForm = conferenceSummary;
            ViewBag.ConferenceFormID = conferenceFormID;
            return RedirectToAction("Details", "ConferenceForms", new { id = conferenceFormID });
        }

        private void PopulateDropDownLists(Trip trip = null)
        {
            var start = from a in db.Addresses
                            orderby a.SiteName + a.StreetAddress
                            select a;
            var end = from a in db.Addresses
                            orderby a.SiteName + a.StreetAddress
                            select a;

            ViewBag.EndAddressID = new SelectList(start, "ID", "FullAddress", trip?.StartAddressID);
            ViewBag.StartAddressID = new SelectList(end, "ID", "FullAddress", trip?.StartAddressID);
        }

        /// <summary>
        /// Checks how many entries in the Trip entity contain the conference form ID that is
        /// passed. Throws a Trip Exception if there are more than two.
        /// </summary>
        /// <param name="conferenceFormID">the conference form</param>
        private void ValidateTrips(int? conferenceFormID)
        {
            int count = 0;

            var trips = db.Trips.Where(t => t.ConferenceFormID == conferenceFormID);

            foreach (var item in trips)
            {
                count++;
            }

            if (count >= 2)
            {
                throw new TripException();
            }
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
