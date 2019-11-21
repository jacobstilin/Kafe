using KafeCruisers.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace KafeCruisers.Controllers
{
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult Index(int id)
        {
            Customer customer = GetLoggedInCustomer();
            customer.OrderTruckId = id;
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public Customer GetLoggedInCustomer()
        {
            string currentId = User.Identity.GetUserId();
            Customer customer = db.Customers.FirstOrDefault(u => u.ApplicationId == currentId);
            return (customer);
        }


        
        public ActionResult StartOrder()
        {
            Customer customer = GetLoggedInCustomer();
            Order newOrder = new Order();
            newOrder.CustomerId = customer.CustomerId;
            newOrder.TruckId = customer.OrderTruckId;
            db.Orders.Add(newOrder);
            db.SaveChanges();
            customer.CurrentOrderId = newOrder.OrderId;
            db.SaveChanges();
            
            Menu truckMenu = db.Menus.FirstOrDefault(m => m.TruckId == customer.OrderTruckId);

            return View(db.MenuItems.Where(m => m.MenuId == truckMenu.MenuId && m.Category == "drink").ToList());
        }

        

        // Gets the id of the drink from the view
        public ActionResult EditOrderItem(int id)
        {
            Customer customer = GetLoggedInCustomer();
            OrderItem orderItem = new OrderItem();
            MenuItem menuItem = db.MenuItems.FirstOrDefault(m => m.MenuItemId == id);
            orderItem.OrderId = customer.CurrentOrderId;
            orderItem.ItemName = menuItem.Name;
            db.OrderItems.Add(orderItem);
            db.SaveChanges();
            customer.CurrentOrderItemId = orderItem.OrderItemId;
            db.SaveChanges();
            return View(orderItem);
        }

        
        [HttpPost]
        public ActionResult EditOrderItem(int id, OrderItem orderItem)
        {
            try
            {
                Customer customer = GetLoggedInCustomer();
                OrderItem newOrderItem = db.OrderItems.FirstOrDefault(o => o.OrderItemId == customer.CurrentOrderItemId);
                newOrderItem.Size = orderItem.Size;
                newOrderItem.Room = orderItem.Room;
                newOrderItem.Decaf = orderItem.Decaf;
                newOrderItem.Iced = orderItem.Iced;
                newOrderItem.Temperature = orderItem.Temperature;
                newOrderItem.WhippedCream = orderItem.WhippedCream;


                db.SaveChanges();



                return RedirectToAction("EditCreamers");
            }
            catch
            {
                return View();
            }
        }



        // In this method we must pass the creamer menu items to the view. These items must be added to the DB by app manager I guess
        public ActionResult EditCreamers()
        {
            ICollection<MenuItem> creamers = db.MenuItems.Where(m => m.Category == "Creamer").ToList();
            return View(creamers);

        }
        
        [HttpPost]
        public ActionResult EditCreamers(ICollection<MenuItem> creamers)
        {
            Customer customer = GetLoggedInCustomer();
            OrderItem orderItem = db.OrderItems.FirstOrDefault(o => o.OrderItemId == customer.CurrentOrderItemId);

            for (int i = 0; i < creamers.Count(); i++)
            {
                MenuItem setCreamer = creamers.ElementAt(i);
                Creamer newCreamer = new Creamer();
                newCreamer.CreamerType = setCreamer.Name;
                newCreamer.Splashes = setCreamer.Quantity;
                newCreamer.OrderItemId = orderItem.OrderItemId;
                db.Creamers.Add(newCreamer);
                db.SaveChanges();
            }
            return RedirectToAction("EditShots");
        }


        public ActionResult EditShots()
        {
            ICollection<MenuItem> shots = db.MenuItems.Where(m => m.Category == "Shot").ToList();
            return View(shots);

        }

        [HttpPost]
        public ActionResult EditShots(ICollection<MenuItem> shots)
        {
            Customer customer = GetLoggedInCustomer();
            OrderItem orderItem = db.OrderItems.FirstOrDefault(o => o.OrderItemId == customer.CurrentOrderItemId);

            for (int i = 0; i < shots.Count(); i++)
            {
                MenuItem setShot = shots.ElementAt(i);
                Shot newShot = new Shot();
                newShot.ShotType = setShot.Name;
                newShot.Shots = setShot.Quantity;
                newShot.OrderItemId = orderItem.OrderItemId;
                db.Shots.Add(newShot);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // you stoopid af! tomorrow take these mfs off ya mf'n orderitem model, stooopid!


    }
}
