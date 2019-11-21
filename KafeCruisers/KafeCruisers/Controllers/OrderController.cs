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
            List<Creamer> creamerList = new List<Creamer>();

            for (int i = 0; i < creamers.Count(); i++)
            {
                MenuItem setCreamer = creamers.ElementAt(i);
                Creamer newCreamer = new Creamer();
                newCreamer.CreamerType = setCreamer.Name;
                newCreamer.Splashes = setCreamer.Quantity;
                newCreamer.OrderItemId = orderItem.OrderItemId;

                
                creamerList.Add(null);
                creamerList[i] = new Creamer();
                creamerList[i].CreamerType = newCreamer.CreamerType;
                creamerList[i].Splashes = newCreamer.Splashes;
                creamerList[i].OrderItemId = newCreamer.OrderItemId;
                
                db.SaveChanges();
            }
            orderItem.Creamers = creamerList;

            db.SaveChanges();
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
            return RedirectToAction("EditSweeteners");
        }


        public ActionResult EditSweeteners()
        {
            ICollection<MenuItem> sweeteners = db.MenuItems.Where(m => m.Category == "Sweetener").ToList();
            return View(sweeteners);

        }

        [HttpPost]
        public ActionResult EditSweeteners(ICollection<MenuItem> sweeteners)
        {
            Customer customer = GetLoggedInCustomer();
            OrderItem orderItem = db.OrderItems.FirstOrDefault(o => o.OrderItemId == customer.CurrentOrderItemId);

            for (int i = 0; i < sweeteners.Count(); i++)
            {
                MenuItem setSweetener = sweeteners.ElementAt(i);
                Sweetener newSweetener = new Sweetener();
                newSweetener.SweetenerType = setSweetener.Name;
                newSweetener.Scoops = setSweetener.Quantity;
                newSweetener.OrderItemId = orderItem.OrderItemId;
                db.Sweeteners.Add(newSweetener);
                db.SaveChanges();
            }
            return RedirectToAction("EditSauces");
        }



        public ActionResult EditSauces()
        {
            ICollection<MenuItem> sauces = db.MenuItems.Where(m => m.Category == "Sauce").ToList();
            return View(sauces);

        }

        [HttpPost]
        public ActionResult EditSauces(ICollection<MenuItem> sauces)
        {
            Customer customer = GetLoggedInCustomer();
            OrderItem orderItem = db.OrderItems.FirstOrDefault(o => o.OrderItemId == customer.CurrentOrderItemId);

            for (int i = 0; i < sauces.Count(); i++)
            {
                MenuItem setSauce = sauces.ElementAt(i);
                Sauce newSauce = new Sauce();
                newSauce.SauceType = setSauce.Name;
                newSauce.SaucePumps = setSauce.Quantity;
                newSauce.OrderItemId = orderItem.OrderItemId;
                db.Sauces.Add(newSauce);
                db.SaveChanges();
            }
            return RedirectToAction("EditSyrups");
        }



        public ActionResult EditSyrups()
        {
            ICollection<MenuItem> syrups = db.MenuItems.Where(m => m.Category == "Syrup").ToList();
            return View(syrups);

        }

        [HttpPost]
        public ActionResult EditSyrups(ICollection<MenuItem> syrups)
        {
            Customer customer = GetLoggedInCustomer();
            OrderItem orderItem = db.OrderItems.FirstOrDefault(o => o.OrderItemId == customer.CurrentOrderItemId);

            for (int i = 0; i < syrups.Count(); i++)
            {
                MenuItem setSyrup = syrups.ElementAt(i);
                Syrup newSyrup = new Syrup();
                newSyrup.SyrupType = setSyrup.Name;
                newSyrup.SyrupPumps = setSyrup.Quantity;
                newSyrup.OrderItemId = orderItem.OrderItemId;
                db.Syrups.Add(newSyrup);
                db.SaveChanges();
            }
            return RedirectToAction("EditDrizzles");
        }





        public ActionResult EditDrizzles()
        {
            ICollection<MenuItem> drizzles = db.MenuItems.Where(m => m.Category == "Drizzle").ToList();
            return View(drizzles);

        }

        [HttpPost]
        public ActionResult EditDrizzles(ICollection<MenuItem> drizzles)
        {
            Customer customer = GetLoggedInCustomer();
            OrderItem orderItem = db.OrderItems.FirstOrDefault(o => o.OrderItemId == customer.CurrentOrderItemId);

            for (int i = 0; i < drizzles.Count(); i++)
            {
                MenuItem setDrizzle = drizzles.ElementAt(i);
                Drizzle newDrizzle = new Drizzle();
                newDrizzle.DrizzleType = setDrizzle.Name;
                newDrizzle.Drizzles = setDrizzle.Quantity;
                newDrizzle.OrderItemId = orderItem.OrderItemId;
                db.Drizzles.Add(newDrizzle);
                db.SaveChanges();
            }
            return RedirectToAction("EditPowders");
        }



        public ActionResult EditPowders()
        {
            ICollection<MenuItem> powders = db.MenuItems.Where(m => m.Category == "Powder").ToList();
            return View(powders);

        }

        [HttpPost]
        public ActionResult EditPowders(ICollection<MenuItem> powders)
        {
            Customer customer = GetLoggedInCustomer();
            OrderItem orderItem = db.OrderItems.FirstOrDefault(o => o.OrderItemId == customer.CurrentOrderItemId);

            for (int i = 0; i < powders.Count(); i++)
            {
                MenuItem setPowder = powders.ElementAt(i);
                Powder newPowder = new Powder();
                newPowder.PowderType = setPowder.Name;
                newPowder.Scoops = setPowder.Quantity;
                newPowder.OrderItemId = orderItem.OrderItemId;
                db.Powders.Add(newPowder);
                db.SaveChanges();
            }
            return RedirectToAction("EditToppings");
        }



        public ActionResult EditToppings()
        {
            ICollection<MenuItem> powders = db.MenuItems.Where(m => m.Category == "Toppings").ToList();
            return View(powders);

        }

        [HttpPost]
        public ActionResult EditToppings(ICollection<MenuItem> toppings)
        {
            Customer customer = GetLoggedInCustomer();
            OrderItem orderItem = db.OrderItems.FirstOrDefault(o => o.OrderItemId == customer.CurrentOrderItemId);

            for (int i = 0; i < toppings.Count(); i++)
            {
                MenuItem setToppings = toppings.ElementAt(i);
                Toppings newToppings = new Toppings();
                newToppings.ToppingsType = setToppings.Name;
                newToppings.ToppingsAmmount = setToppings.Quantity;
                newToppings.OrderItemId = orderItem.OrderItemId;
                db.Toppings.Add(newToppings);
                db.SaveChanges();
            }
            return RedirectToAction("ReviewOrder");
        }


        public ActionResult ReviewOrder()
        {
            Customer customer = GetLoggedInCustomer();
            OrderItem orderItem = db.OrderItems.FirstOrDefault(o => o.OrderId == customer.CurrentOrderId);
            ICollection<Creamer> creamerList = db.Creamers.Where(c => c.OrderItemId == orderItem.OrderItemId).ToList();
            ViewBag.creamers = creamerList;
            return View(orderItem);

        }

        public ActionResult TestReviewOrder()
        {
            
            OrderItem orderItem = db.OrderItems.FirstOrDefault(o => o.OrderItemId == 19);
            ICollection<Creamer> creamerList = db.Creamers.Where(c => c.OrderItemId == orderItem.OrderItemId).ToList();
            ViewBag.creamers = creamerList;
            return RedirectToAction("ReviewOrder", orderItem);

        }






    }
}
