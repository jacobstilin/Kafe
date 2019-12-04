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
    public class TruckController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public Customer GetLoggedInCustomer()
        {
            string currentId = User.Identity.GetUserId();
            Customer customer = db.Customers.FirstOrDefault(u => u.ApplicationId == currentId);
            return (customer);
        }


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
            /*try
            {*/
                Truck newTruck = new Truck();
                newTruck.TruckName = truck.TruckName;
                newTruck.StartTime = DateTime.Now;
                newTruck.EndTime = DateTime.Now;
                Menu menu = new Menu();
                string newMenuName = (truck.TruckName + "Menu");
                menu.MenuName = newMenuName;
                db.Trucks.Add(newTruck);
                db.SaveChanges();
                menu.TruckId = newTruck.TruckId;
                db.Menus.Add(menu);
                db.SaveChanges();

                return RedirectToAction("TruckSelector", "Truck");
            /*}
            catch
            {
                return View();
            }*/
        }


        public ActionResult TruckSelector()
        {
            return View(db.Trucks.ToList());
        }


        public ActionResult CustomerTruckSelector()
        {
            List<Truck> trucks = db.Trucks.ToList();
            string api = APIKeys.GoogleMapsApiKey;
            
            ViewBag.key = ("https://maps.googleapis.com/maps/api/js?key=" + api + "&callback=initMap");
            ViewBag.coordinates = GetTruckCoordinates();
            return View();
        }


        public string GetTruckCoordinates()
        {
            // Change later to be a list of trucks that are open...mf staying past close n shit
            // List<Truck> trucksList = db.Trucks.Where(t => t.StartTime > DateTime.Now && t.EndTime < DateTime.Now).ToList();
            List<Truck> trucksList = db.Trucks.ToList();
            string[] stringArray = new string[trucksList.Count];
            for (int i = 0; i < trucksList.Count; i++)
            {
                Truck truck = trucksList[i];
                Location location = db.Locations.FirstOrDefault(l => l.LocationId == truck.LocationId);
                
                string arrayString = "['" + location.LocationName + "', " + location.Longitude + ", " + location.Latitude + ", 0, 'SelectedTruck/" + truck.TruckId + "']";
                stringArray[i] = arrayString;
            }

            string finalArray = "[ " + string.Join(",", stringArray) + " ]";
            return finalArray;
        }


        public ActionResult SelectedTruck(int id)
        {
            // Set this id as the customer's current truck and give a mf menu
            Customer customer = GetLoggedInCustomer();
            customer.OrderTruckId = id;
            db.SaveChanges();
            return RedirectToAction("StartOrder", "Order");
        }


        public ActionResult SetLocation(int id, int truckId)
        {
            Location location = db.Locations.FirstOrDefault(l => l.LocationId == id);
            Truck truck = db.Trucks.FirstOrDefault(t => t.TruckId == truckId);

            truck.LocationId = location.LocationId;
            db.SaveChanges();
            return RedirectToAction("TruckSelector");
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


        public ActionResult ViewTruckOrders(int id)
        {
            List<Order> truckOrders = db.Orders.Where(o => o.TruckId == id && o.UniqueId != null).ToList();
            List<Order> sortedList = truckOrders.OrderBy(o => o.UniqueId).ToList();
            return View(sortedList);
        }


        public ActionResult ViewTruckOrdersByItem(int id)
        {
            OrderItem orderItem = db.OrderItems.FirstOrDefault(o => o.OrderItemId == id);
            Order order = db.Orders.FirstOrDefault(o => o.OrderId == orderItem.OrderId);
            
            return View("ViewTruckOrders", new { id = order.TruckId });
        }


        public ActionResult ViewOrderItems(int id)
        {
            Order order = db.Orders.FirstOrDefault(o => o.OrderId == id);
            List<OrderItem> orderItems = db.OrderItems.Where(o => o.OrderId == id).ToList();
            ViewBag.Return = order.TruckId;
            return View(orderItems);
        }


        public ActionResult ViewOrderItemsByItemId(int id)
        {
            OrderItem orderItem = db.OrderItems.FirstOrDefault(o => o.OrderItemId == id);
            Order order = db.Orders.FirstOrDefault(o => o.OrderId == orderItem.OrderId);
            
            return RedirectToAction("ViewOrderItems", new { id = order.OrderId });
        }


        public ActionResult ViewOrderItem(int id)
        {
            OrderItem orderItem = db.OrderItems.FirstOrDefault(o => o.OrderItemId == id);
            return View(orderItem);
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
