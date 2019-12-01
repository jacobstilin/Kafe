using KafeCruisers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KafeCruisers.Controllers
{
    [Authorize]
    public class LocationController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult LocationsList()
        {
            List<Location> locations = db.Locations.ToList();
            return View(locations);
        }
        public ActionResult ViewLocations(int id)
        {
            ViewBag.truck = id;
            List<Location> locations = db.Locations.ToList();
            return View(locations);
        }

        public ActionResult AddLocation()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddLocation(Location location)
        {
            Location newLocation = new Location();
            newLocation.LocationName = location.LocationName;
            newLocation.Longitude = location.Longitude;
            newLocation.Latitude = location.Latitude;
            db.Locations.Add(newLocation);
            db.SaveChanges();
            return RedirectToAction("LocationsList");
        }



        // GET: Location
        public ActionResult Index()
        {
            return View();
        }

        // GET: Location/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Location/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Location/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Location/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Location/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Location/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Location/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
