using KafeCruisers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KafeCruisers.Controllers
{
    public class TruckController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Truck
        public ActionResult Index()
        {
            return View();
        }

        // GET: Truck/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Truck/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Truck/Create
        [HttpPost]
        public ActionResult Create(Truck truck)
        {
            try
            {
                Truck newTruck = new Truck();
                newTruck.TruckName = truck.TruckName;


                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult TruckSelector()
        {
            return View(db.Trucks.ToList());
        }











        // GET: Truck/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Truck/Edit/5
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

        // GET: Truck/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Truck/Delete/5
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
