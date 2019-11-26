﻿using KafeCruisers.Models;
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
                Menu menu = new Menu();
                string newMenuName = (truck.TruckName + "Menu");
                menu.MenuName = newMenuName;
                db.Trucks.Add(newTruck);
                db.SaveChanges();
                menu.TruckId = newTruck.TruckId;
                db.Menus.Add(menu);
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
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


        public ActionResult EditTruckHours(int id)
        {
            Truck truck = db.Trucks.FirstOrDefault(t => t.TruckId == id);
            return View(truck);
        }

        [HttpPost]
        public ActionResult EditTruckHours(Truck truck)
        {
            Truck editTruck = db.Trucks.FirstOrDefault(t => t.TruckId == truck.TruckId);
            editTruck.StartTime = truck.StartTime;
            editTruck.EndTime = truck.EndTime;
            db.SaveChanges();
            return RedirectToAction("TruckSelector");
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
