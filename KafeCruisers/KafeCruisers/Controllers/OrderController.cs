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
            Customer customer = GetLoggedInUser();
            customer.OrderTruckId = id;
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public Customer GetLoggedInUser()
        {
            string currentId = User.Identity.GetUserId();
            Customer customer = db.Customers.FirstOrDefault(u => u.ApplicationId == currentId);
            return (customer);
        }


        // GET: Order/Create
        public ActionResult StartOrder()
        {
            Customer customer = GetLoggedInUser();
            Order newOrder = new Order();
            newOrder.CustomerId = customer.CustomerId;
            newOrder.TruckId = customer.OrderTruckId;
            Menu truckMenu = db.Menus.FirstOrDefault(m => m.TruckId == customer.OrderTruckId);
            return View(db.MenuItems.Where(m => m.MenuId == truckMenu.MenuId && m.Category == "drink").ToList());
        }

        

        // GET: Order/Edit/5
        public ActionResult EditOrderItem(int id)
        {
            OrderItem orderItem = new OrderItem();
            MenuItem menuItem = db.MenuItems.FirstOrDefault(m => m.MenuItemId == id);
            orderItem.ItemName = menuItem.Name;
            return View(orderItem);
        }

        // POST: Order/Edit/5
        [HttpPost]
        public ActionResult EditOrderItem(int id, OrderItem orderItem)
        {
            try
            {
                OrderItem newOrderItem = new OrderItem();
                newOrderItem.Size = orderItem.Size;
                newOrderItem.Room = orderItem.Room;
                newOrderItem.Decaf = orderItem.Decaf;
                newOrderItem.Iced = orderItem.Iced;
                newOrderItem.Temperature = orderItem.Temperature;
                newOrderItem.WhippedCream = orderItem.WhippedCream;


                db.SaveChanges();

                return RedirectToAction("EditCreamers", new { id = newOrderItem.OrderItemId });
            }
            catch
            {
                return View();
            }
        }


        public ActionResult EditCreamers(int id)
        {
            OrderItem orderItem = db.OrderItems.FirstOrDefault(o => o.OrderId == id);
            ICollection<Creamer> creamers = db.Creamers.Where(c => c.OrderItemId == orderItem.OrderItemId).ToList();
            return View(creamers);

        }
        
        /*[HttpPost]
        public ActionResult EditCreamers(int id, ICollection<Creamer> creamers)
        {
            OrderItem orderItem = db.OrderItems.FirstOrDefault(o => o.OrderItemId == id);
            int creamersId = db.Creamers.FirstOrDefault(c => c.OrderItemId == id).CreamerId;

            for (int i = 0; i < creamers.Count(); i++)
            {
                var creamerChange = db.Creamers.FirstOrDefault(c => c.CreamerId == creamersId);
                Creamer setCreamer = creamers.ElementAt(i);
                creamerChange.OnePercentMilk = creamers.

                    //youre doing this all wrong. each order item does not have a icollection
                    //it has creamer in it. one creamer model per order item
            }
        }*/


        // you stoopid af! tomorrow take these mfs off ya mf'n orderitem model, stooopid!

       
    }
}
