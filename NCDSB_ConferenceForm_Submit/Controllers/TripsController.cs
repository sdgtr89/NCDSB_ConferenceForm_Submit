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
    public class TripsController : Controller
    {
        private ConferenceFormEntities db = new ConferenceFormEntities();

        // GET: Trips
        public ActionResult Index()
        {
            var trips = db.Trips.Include(t => t.EndAddress).Include(t => t.MileageForm).Include(t => t.StartAddress);
            return View(trips.ToList());
        }

        // GET: Trips/Details/5
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

        // GET: Trips/Create
        public ActionResult Create()
        {
            ViewBag.EndAddressID = new SelectList(db.Addresses, "ID", "SiteName");
            ViewBag.MileageFormID = new SelectList(db.MileageForms, "ID", "CreatedBy");
            ViewBag.StartAddressID = new SelectList(db.Addresses, "ID", "SiteName");
            return View();
        }

        // POST: Trips/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,StartAddressID,EndAddressID,Distance,Date,MileageFormID")] Trip trip)
        {
            if (ModelState.IsValid)
            {
                db.Trips.Add(trip);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EndAddressID = new SelectList(db.Addresses, "ID", "SiteName", trip.EndAddressID);
            ViewBag.MileageFormID = new SelectList(db.MileageForms, "ID", "CreatedBy", trip.MileageFormID);
            ViewBag.StartAddressID = new SelectList(db.Addresses, "ID", "SiteName", trip.StartAddressID);
            return View(trip);
        }

        // GET: Trips/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.EndAddressID = new SelectList(db.Addresses, "ID", "SiteName", trip.EndAddressID);
            ViewBag.MileageFormID = new SelectList(db.MileageForms, "ID", "CreatedBy", trip.MileageFormID);
            ViewBag.StartAddressID = new SelectList(db.Addresses, "ID", "SiteName", trip.StartAddressID);
            return View(trip);
        }

        // POST: Trips/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,StartAddressID,EndAddressID,Distance,Date,MileageFormID")] Trip trip)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trip).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EndAddressID = new SelectList(db.Addresses, "ID", "SiteName", trip.EndAddressID);
            ViewBag.MileageFormID = new SelectList(db.MileageForms, "ID", "CreatedBy", trip.MileageFormID);
            ViewBag.StartAddressID = new SelectList(db.Addresses, "ID", "SiteName", trip.StartAddressID);
            return View(trip);
        }

        // GET: Trips/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Trips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trip trip = db.Trips.Find(id);
            db.Trips.Remove(trip);
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
