using NCDSB_ConferenceForm_Submit.DAL;
using NCDSB_ConferenceForm_Submit.Models;
using NCDSB_ConferenceForm_Submit.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace NCDSB_ConferenceForm_Submit.Controllers
{
    public class MileageFormMasterController : Controller
    {
        private ConferenceFormEntities db = new ConferenceFormEntities();

        [HttpPost]
        public ActionResult GetAddresses(string prefix)
        {
            var customers = (from address in db.Addresses
                             where address.SiteName.StartsWith(prefix)
                             select new
                             {
                                 label = address.SiteName + " - " + address.StreetAddress + ", " + address.City.Name,
                                 val = address.ID
                             }).ToList();

            return Json(customers);
        }

        // GET: MileageFormMaster/Create
        public ActionResult Create(MileageFormVM model, int mileageFormID, string mileageSummary)
        {
            var trip = new Trip { MileageFormID = mileageFormID };           

            ViewBag.MileageFormID = mileageFormID;
            ViewBag.MileageSummary = mileageSummary;           

            return View();
        }

        // POST: MileageFormMaster/Create
        [HttpPost]
        public ActionResult Create(MileageFormVM model, Trip trip)
        {

            var startAddress = ParseAddress(model.StartAddress);
            var endAddress = ParseAddress(model.EndAddress);

            string startCityValue = startAddress[2].Trim();
            string startSiteName = startAddress[0].Trim();
            string startStreetAddress = startAddress[1].Trim();

            string endCityValue = endAddress[2].Trim();
            string endSiteName = endAddress[0].Trim();
            string endStreetAddress = endAddress[1].Trim();

            //create the cities
            var startCity = new City { Name = startCityValue };
            var endCity = new City { Name = endCityValue };

            var startAddressCreate = new Address();
            var endAddressCreate = new Address();

            var checkStartCity = db.Cities.Where(c => c.Name == startCityValue);
            int startCityCount = 0;
            checkStartCity.ToList().ForEach(x => { startCityCount++; });

            if (startCityCount < 1)
            {
                new CitiesController().Create(startCity);                
            }
            
            startAddressCreate.CityID = db.Cities.Where(c => c.Name == startCityValue).SingleOrDefault().ID;            

            var checkEndCity = db.Cities.Where(c => c.Name == endCityValue);
            int endCityCount = 0;
            checkEndCity.ToList().ForEach(x => { endCityCount++; });

            if (endCityCount < 1)
            {
                new CitiesController().Create(endCity);
            }
            
            endAddressCreate.CityID = db.Cities.Where(c => c.Name == endCityValue).SingleOrDefault().ID;

            startAddressCreate.SiteName = startSiteName;
            startAddressCreate.StreetAddress = startStreetAddress;

            var checkStartAddress = db.Addresses.Where(c => c.SiteName == startSiteName);
            int startAddressCount = 0;
            checkStartAddress.ToList().ForEach(x => { startAddressCount++; });

            if (startAddressCount < 1)
            {
                new AddressesController().Create(startAddressCreate);
            }

            
            trip.StartAddressID = db.Addresses.Where(a => a.SiteName == startSiteName).SingleOrDefault().ID;

            endAddressCreate.SiteName = endSiteName;
            endAddressCreate.StreetAddress = endStreetAddress;

            var checkEndAddress = db.Addresses.Where(c => c.SiteName == endSiteName);
            int endAddressCount = 0;
            checkEndAddress.ToList().ForEach(x => { endAddressCount++; });

            if (endAddressCount < 1)
            {                
                new AddressesController().Create(endAddressCreate); 
            }

            trip.EndAddressID = db.Addresses.Where(a => a.SiteName == endSiteName).SingleOrDefault().ID;

            trip.Distance = model.Distance;
            trip.Date = model.Date;

            new TripController().Add(trip);

            return RedirectToAction("Details", "MileageForms", new { id = trip.MileageFormID });
        }

        // GET: MileageFormMaster/Create
        public ActionResult CreateForConference(MileageFormVM model, int conferenceFormID, string conferenceSummary)
        {
            var trip = new Trip { ConferenceFormID = conferenceFormID };

            ViewBag.ConferenceFormID = conferenceFormID;
            ViewBag.ConferenceSummary = conferenceSummary;

            return View();
        }

        // POST: MileageFormMaster/Create
        [HttpPost]
        public ActionResult CreateForConference(MileageFormVM model, Trip trip)
        {

            var startAddress = ParseAddress(model.StartAddress);
            var endAddress = ParseAddress(model.EndAddress);

            string startCityValue = startAddress[2].Trim();
            string startSiteName = startAddress[0].Trim();
            string startStreetAddress = startAddress[1].Trim();

            string endCityValue = endAddress[2].Trim();
            string endSiteName = endAddress[0].Trim();
            string endStreetAddress = endAddress[1].Trim();

            //create the cities
            var startCity = new City { Name = startCityValue };
            var endCity = new City { Name = endCityValue };

            var startAddressCreate = new Address();
            var endAddressCreate = new Address();

            var checkStartCity = db.Cities.Where(c => c.Name == startCityValue);
            int startCityCount = 0;
            checkStartCity.ToList().ForEach(x => { startCityCount++; });

            if (startCityCount < 1)
            {
                new CitiesController().Create(startCity);
            }

            startAddressCreate.CityID = db.Cities.Where(c => c.Name == startCityValue).SingleOrDefault().ID;

            var checkEndCity = db.Cities.Where(c => c.Name == endCityValue);
            int endCityCount = 0;
            checkEndCity.ToList().ForEach(x => { endCityCount++; });

            if (endCityCount < 1)
            {
                new CitiesController().Create(endCity);
            }

            endAddressCreate.CityID = db.Cities.Where(c => c.Name == endCityValue).SingleOrDefault().ID;

            startAddressCreate.SiteName = startSiteName;
            startAddressCreate.StreetAddress = startStreetAddress;

            var checkStartAddress = db.Addresses.Where(c => c.SiteName == startSiteName);
            int startAddressCount = 0;
            checkStartAddress.ToList().ForEach(x => { startAddressCount++; });

            if (startAddressCount < 1)
            {
                new AddressesController().Create(startAddressCreate);
            }


            trip.StartAddressID = db.Addresses.Where(a => a.SiteName == startSiteName).SingleOrDefault().ID;

            endAddressCreate.SiteName = endSiteName;
            endAddressCreate.StreetAddress = endStreetAddress;

            var checkEndAddress = db.Addresses.Where(c => c.SiteName == endSiteName);
            int endAddressCount = 0;
            checkStartAddress.ToList().ForEach(x => { endAddressCount++; });

            if (endAddressCount < 1)
            {
                new AddressesController().Create(endAddressCreate);
            }

            trip.EndAddressID = db.Addresses.Where(a => a.SiteName == endSiteName).SingleOrDefault().ID;

            trip.Distance = model.Distance;
            trip.Date = model.Date;

            new TripController().AddForConference(trip);

            return RedirectToAction("Details", "ConferenceForms", new { id = trip.ConferenceFormID });
        }

        private List<string> ParseAddress(string fullAddress)
        {
            string[] values = fullAddress.Split('-', ',');

            string site = values[0];
            string streetAddress = values[1];
            string city = values[2];

            var addressList = new List<string>
            {
                site,
                streetAddress,
                city
            };

            return addressList;
        }
    }


}
