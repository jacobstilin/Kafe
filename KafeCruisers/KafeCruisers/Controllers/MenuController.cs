using KafeCruisers.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KafeCruisers.Controllers
{
    public class MenuController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public Employee GetLoggedInEmployee()
        {
            string currentId = User.Identity.GetUserId();
            Employee employee = db.Employees.FirstOrDefault(u => u.ApplicationId == currentId);
            return (employee);
        }

        public ActionResult AddMenuItem(int id)
        {
            Employee employee = GetLoggedInEmployee();
            employee.MenuTruckId = id;
            db.SaveChanges();
            return View();
        }

        [HttpPost]
        public ActionResult AddMenuItem(MenuItem menuItem)
        {
            Employee employee = GetLoggedInEmployee();
            Menu menu = db.Menus.FirstOrDefault(m => m.TruckId == employee.MenuTruckId);
            MenuItem newMenuItem = new MenuItem();
            newMenuItem.Name = menuItem.Name;
            newMenuItem.Category = menuItem.Category;
            newMenuItem.MenuId = menu.MenuId;

            if (menuItem.Price == null)
            {
                newMenuItem.Price = 0;
            }
            else
            {
                newMenuItem.Price = menuItem.Price;
            }

            
            
            db.MenuItems.Add(newMenuItem);
            db.SaveChanges();

            if (menuItem.Category == "Drink")
            {
                return RedirectToAction("AddSizes", new { id = newMenuItem.MenuItemId });
            }

            return RedirectToAction("Index", "Home");
            
        }

        public ActionResult AddSizes(int id)
        {
            MenuItem newMenuItem = db.MenuItems.FirstOrDefault(m => m.MenuItemId == id);
            List<Size> sizesList = new List<Size>();

            for (int i = 0; i < 5; i++)
            {
                sizesList.Add(null);
                sizesList[i] = new Size();
                sizesList[i].MenuItemId = newMenuItem.MenuItemId;
            }
            newMenuItem.Sizes = sizesList;
            db.SaveChanges();

            return View(newMenuItem.Sizes);
        }

        [HttpPost]
        public ActionResult AddSizes(ICollection<Size> sizes)
        {
            int thisMenuItem = sizes.ElementAt(0).MenuItemId;
            MenuItem menuItem = db.MenuItems.FirstOrDefault(m => m.MenuItemId == thisMenuItem);
           
            menuItem.Sizes = sizes;
            db.SaveChanges();

            return RedirectToAction("AddTemperatures", new { id = menuItem.MenuItemId });
        }

        public ActionResult AddTemperatures(int id)
        {
            MenuItem newMenuItem = db.MenuItems.FirstOrDefault(m => m.MenuItemId == id);
            List<Temperature> temperaturesList = new List<Temperature>();

            for (int i = 0; i < 5; i++)
            {
                temperaturesList.Add(null);
                temperaturesList[i] = new Temperature();
                temperaturesList[i].MenuItemId = newMenuItem.MenuItemId;
            }
            newMenuItem.Temperatures = temperaturesList;
            db.SaveChanges();

            return View(newMenuItem.Temperatures);
        }

        [HttpPost]
        public ActionResult AddTemperatures(ICollection<Temperature> temperatures)
        {
            int thisMenuItem = temperatures.ElementAt(0).MenuItemId;
            MenuItem menuItem = db.MenuItems.FirstOrDefault(m => m.MenuItemId == thisMenuItem);

            menuItem.Temperatures = temperatures;
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }




        // GET: Menu
        public ActionResult Index(int id)
        {
            Employee employee = GetLoggedInEmployee();
            employee.MenuTruckId = id;
            db.SaveChanges();
            return RedirectToAction("AddMenuItem");
        }

        // GET: Menu/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Menu/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Menu/Create
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

        // GET: Menu/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Menu/Edit/5
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

        // GET: Menu/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Menu/Delete/5
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
