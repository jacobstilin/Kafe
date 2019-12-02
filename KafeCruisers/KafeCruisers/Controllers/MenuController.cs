using KafeCruisers.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KafeCruisers.Controllers
{
    [Authorize]
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
            if (menuItem.Duration == null)
            {
                newMenuItem.Duration = 0;
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

            return RedirectToAction("ViewTruckMenu", "Menu", new { id = menu.TruckId} );
            
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
            Menu menu = db.Menus.FirstOrDefault(m => m.MenuId == menuItem.MenuId);

            return RedirectToAction("ViewTruckMenu", "Menu", new { id = menu.TruckId });
        }


        public ActionResult ViewTruckMenu(int id)
        {
            Menu currentMenu = db.Menus.FirstOrDefault(m => m.TruckId == id);
            Truck currentTruck = db.Trucks.FirstOrDefault(t => t.TruckId == currentMenu.TruckId);
            List<MenuItem> drinks = db.MenuItems.Where(i => i.Category == "Drink" && i.MenuId == currentMenu.MenuId).ToList();
            List<MenuItem> creamers = db.MenuItems.Where(i => i.Category == "Creamer" && i.MenuId == currentMenu.MenuId).ToList();
            List<MenuItem> sweeteners = db.MenuItems.Where(i => i.Category == "Sweetener" && i.MenuId == currentMenu.MenuId).ToList();
            List<MenuItem> shots = db.MenuItems.Where(i => i.Category == "Shot" && i.MenuId == currentMenu.MenuId).ToList();
            List<MenuItem> drizzles = db.MenuItems.Where(i => i.Category == "Drizzle" && i.MenuId == currentMenu.MenuId).ToList();
            List<MenuItem> powders = db.MenuItems.Where(i => i.Category == "Powder" && i.MenuId == currentMenu.MenuId).ToList();
            List<MenuItem> sauces = db.MenuItems.Where(i => i.Category == "Sauce" && i.MenuId == currentMenu.MenuId).ToList();
            List<MenuItem> syrups = db.MenuItems.Where(i => i.Category == "Syrup" && i.MenuId == currentMenu.MenuId).ToList();
            List<MenuItem> toppings = db.MenuItems.Where(i => i.Category == "Toppings" && i.MenuId == currentMenu.MenuId).ToList();
            ViewBag.Drinks = drinks;
            ViewBag.Creamers = creamers;
            ViewBag.Sweeteners = sweeteners;
            ViewBag.Shots = shots;
            ViewBag.Drizzles = drizzles;
            ViewBag.Powders = powders;
            ViewBag.Sauce = sauces;
            ViewBag.Syrups = syrups;
            ViewBag.Toppings = toppings;
            ViewBag.TruckId = currentTruck.TruckId;
            return View();
        }

        

        

       

        // GET: Menu/Delete/5
        public ActionResult DeleteItem(int id)
        {
            MenuItem menuItem = db.MenuItems.FirstOrDefault(i => i.MenuItemId == id);
            Menu menu = db.Menus.FirstOrDefault(m => m.MenuId == menuItem.MenuId);
            Truck currentTruck = db.Trucks.FirstOrDefault(t => t.TruckId == menu.TruckId);
            db.MenuItems.Remove(menuItem);
            db.SaveChanges();

            return RedirectToAction("ViewTruckMenu", new { id = currentTruck.TruckId });
        }

        
    }
}
