using KafeCruisers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
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



        
        

        

        // GET: Location/Delete/5
        public ActionResult Delete(int id)
        {
            Location deleteLocation = db.Locations.FirstOrDefault(l => l.LocationId == id);
            Truck locationTruck = db.Trucks.FirstOrDefault(t => t.LocationId == id);
            try {
                locationTruck.LocationId = null;
                db.SaveChanges();
            }
            catch { }
            db.Locations.Remove(deleteLocation);
            db.SaveChanges();
            return RedirectToAction("LocationsList");
        }

        

        

    }
}
