﻿using KafeCruisers.Models;
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
            newOrder.FillTime = DateTime.Now;
            newOrder.StartTime = DateTime.Now;
            db.Orders.Add(newOrder);
            db.SaveChanges();
            customer.CurrentOrderId = newOrder.OrderId;
            db.SaveChanges();
            
            Menu truckMenu = db.Menus.FirstOrDefault(m => m.TruckId == customer.OrderTruckId);

            return View(db.MenuItems.Where(m => m.MenuId == truckMenu.MenuId && m.Category == "Drink").ToList());
        }

        //Same method as above, only the customer continues on the same order
        public ActionResult AdditionalDrink()
        {
            Customer customer = GetLoggedInCustomer();
            Menu truckMenu = db.Menus.FirstOrDefault(m => m.TruckId == customer.OrderTruckId);
            
            return View(db.MenuItems.Where(m => m.MenuId == truckMenu.MenuId && m.Category == "Drink").ToList());
        }


        // Gets the id of the drink from the view
        public ActionResult EditOrderItem(int id)
        {
            Customer customer = GetLoggedInCustomer();
            OrderItem orderItem = new OrderItem();
            MenuItem menuItem = db.MenuItems.FirstOrDefault(m => m.MenuItemId == id);
            orderItem.OrderId = customer.CurrentOrderId;
            orderItem.ItemName = menuItem.Name;
            orderItem.Price = menuItem.Price;
            orderItem.IdFromMenu = id;
            db.OrderItems.Add(orderItem);
            db.SaveChanges();
            customer.CurrentOrderItemId = orderItem.OrderItemId;
            db.SaveChanges();

            ViewBag.temperatures = new SelectList(db.Temperatures.Where(s => s.MenuItemId == id && s.TemperatureName != null).ToList(), "TemperatureName", "TemperatureName");
            ViewBag.sizes = new SelectList(db.Sizes.Where(s => s.MenuItemId == id && s.SizeName != null).ToList(), "SizeName", "SizeName");


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
                if (setCreamer.Quantity == null)
                {
                    setCreamer.Quantity = 0;
                }
                Creamer newCreamer = new Creamer();
                newCreamer.CreamerType = setCreamer.Name;
                newCreamer.Splashes = setCreamer.Quantity;
                newCreamer.OrderItemId = orderItem.OrderItemId;
                newCreamer.Price = (setCreamer.Price * setCreamer.Quantity);
                
                creamerList.Add(null);
                creamerList[i] = new Creamer();
                creamerList[i].CreamerType = newCreamer.CreamerType;
                creamerList[i].Splashes = newCreamer.Splashes;
                creamerList[i].OrderItemId = newCreamer.OrderItemId;
                creamerList[i].Price = newCreamer.Price;
                
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
            List<Shot> shotList = new List<Shot>();

            for (int i = 0; i < shots.Count(); i++)
            {
                MenuItem setShot = shots.ElementAt(i);
                if (setShot.Quantity == null)
                {
                    setShot.Quantity = 0;
                }
                Shot newShot = new Shot();
                newShot.ShotType = setShot.Name;
                newShot.Shots = setShot.Quantity;
                newShot.OrderItemId = orderItem.OrderItemId;
                newShot.Price = (setShot.Price * setShot.Quantity);

                shotList.Add(null);
                shotList[i] = new Shot();
                shotList[i].ShotType = newShot.ShotType;
                shotList[i].Shots = newShot.Shots;
                shotList[i].OrderItemId = newShot.OrderItemId;
                shotList[i].Price = newShot.Price;

                db.SaveChanges();
            }
            orderItem.Shots = shotList;

            db.SaveChanges();
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
            List<Sweetener> sweetenerList = new List<Sweetener>();

            for (int i = 0; i < sweeteners.Count(); i++)
            {
                MenuItem setSweetener = sweeteners.ElementAt(i);
                if (setSweetener.Quantity == null)
                {
                    setSweetener.Quantity = 0;
                }
                Sweetener newSweetener = new Sweetener();
                newSweetener.SweetenerType = setSweetener.Name;
                newSweetener.Scoops = setSweetener.Quantity;
                newSweetener.OrderItemId = orderItem.OrderItemId;
                newSweetener.Price = (setSweetener.Price * setSweetener.Quantity);

                sweetenerList.Add(null);
                sweetenerList[i] = new Sweetener();
                sweetenerList[i].SweetenerType = newSweetener.SweetenerType;
                sweetenerList[i].Scoops = newSweetener.Scoops;
                sweetenerList[i].OrderItemId = newSweetener.OrderItemId;
                sweetenerList[i].Price = newSweetener.Price;

                db.SaveChanges();
            }
            orderItem.Sweeteners = sweetenerList;

            db.SaveChanges();
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
            List<Sauce> sauceList = new List<Sauce>();

            for (int i = 0; i < sauces.Count(); i++)
            {
                MenuItem setSauce = sauces.ElementAt(i);
                if (setSauce.Quantity == null)
                {
                    setSauce.Quantity = 0;
                }
                Sauce newSauce = new Sauce();
                newSauce.SauceType = setSauce.Name;
                newSauce.SaucePumps = setSauce.Quantity;
                newSauce.OrderItemId = orderItem.OrderItemId;
                newSauce.Price = (setSauce.Price * setSauce.Quantity);

                sauceList.Add(null);
                sauceList[i] = new Sauce();
                sauceList[i].SauceType = newSauce.SauceType;
                sauceList[i].SaucePumps = newSauce.SaucePumps;
                sauceList[i].OrderItemId = newSauce.OrderItemId;
                sauceList[i].Price = newSauce.Price;

                db.SaveChanges();
            }
            orderItem.Sauces = sauceList;

            db.SaveChanges();
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
            List<Syrup> syrupList = new List<Syrup>();

            for (int i = 0; i < syrups.Count(); i++)
            {
                MenuItem setSyrup = syrups.ElementAt(i);
                if (setSyrup.Quantity == null)
                {
                    setSyrup.Quantity = 0;
                }
                Syrup newSyrup = new Syrup();
                newSyrup.SyrupType = setSyrup.Name;
                newSyrup.SyrupPumps = setSyrup.Quantity;
                newSyrup.OrderItemId = orderItem.OrderItemId;
                newSyrup.Price = (setSyrup.Price * setSyrup.Quantity);

                syrupList.Add(null);
                syrupList[i] = new Syrup();
                syrupList[i].SyrupType = newSyrup.SyrupType;
                syrupList[i].SyrupPumps = newSyrup.SyrupPumps;
                syrupList[i].OrderItemId = newSyrup.OrderItemId;
                syrupList[i].Price = newSyrup.Price;

                db.SaveChanges();
            }
            orderItem.Syrups = syrupList;

            db.SaveChanges();
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
            List<Drizzle> drizzleList = new List<Drizzle>();

            for (int i = 0; i < drizzles.Count(); i++)
            {
                MenuItem setDrizzle = drizzles.ElementAt(i);
                if (setDrizzle.Quantity == null)
                {
                    setDrizzle.Quantity = 0;
                }
                Drizzle newDrizzle = new Drizzle();
                newDrizzle.DrizzleType = setDrizzle.Name;
                newDrizzle.Drizzles = setDrizzle.Quantity;
                newDrizzle.OrderItemId = orderItem.OrderItemId;
                newDrizzle.Price = (setDrizzle.Price * setDrizzle.Quantity);

                drizzleList.Add(null);
                drizzleList[i] = new Drizzle();
                drizzleList[i].DrizzleType = newDrizzle.DrizzleType;
                drizzleList[i].Drizzles = newDrizzle.Drizzles;
                drizzleList[i].OrderItemId = newDrizzle.OrderItemId;
                drizzleList[i].Price = newDrizzle.Price;

                db.SaveChanges();
            }
            orderItem.Drizzles = drizzleList;

            db.SaveChanges();
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
            List<Powder> powderList = new List<Powder>();

            for (int i = 0; i < powders.Count(); i++)
            {
                MenuItem setPowder = powders.ElementAt(i);
                if (setPowder.Quantity == null)
                {
                    setPowder.Quantity = 0;
                }
                Powder newPowder = new Powder();
                newPowder.PowderType = setPowder.Name;
                newPowder.Scoops = setPowder.Quantity;
                newPowder.OrderItemId = orderItem.OrderItemId;
                newPowder.Price = (setPowder.Price * setPowder.Quantity);

                powderList.Add(null);
                powderList[i] = new Powder();
                powderList[i].PowderType = newPowder.PowderType;
                powderList[i].Scoops = newPowder.Scoops;
                powderList[i].OrderItemId = newPowder.OrderItemId;
                powderList[i].Price = newPowder.Price;

                db.SaveChanges();
            }
            orderItem.Powders = powderList;

            db.SaveChanges();
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
            List<Toppings> toppingsList = new List<Toppings>();

            for (int i = 0; i < toppings.Count(); i++)
            {
                MenuItem setToppings = toppings.ElementAt(i);
                if (setToppings.Quantity == null)
                {
                    setToppings.Quantity = 0;
                }
                Toppings newToppings = new Toppings();
                newToppings.ToppingsType = setToppings.Name;
                newToppings.ToppingsAmmount = setToppings.Quantity;
                newToppings.OrderItemId = orderItem.OrderItemId;
                newToppings.Price = (setToppings.Price * setToppings.Quantity);

                toppingsList.Add(null);
                toppingsList[i] = new Toppings();
                toppingsList[i].ToppingsType = newToppings.ToppingsType;
                toppingsList[i].ToppingsAmmount = newToppings.ToppingsAmmount;
                toppingsList[i].OrderItemId = newToppings.OrderItemId;
                toppingsList[i].Price = newToppings.Price;

                db.SaveChanges();
            }
            orderItem.Toppings = toppingsList;

            db.SaveChanges();
            return RedirectToAction("ReviewOrder", new { id = orderItem.OrderItemId });
        }


        // This method takes current OrderItem of logged in customer, compiles total price and gibes additions list to the viewbag
        // Additional method(s) may be needed for reviewing entire order

        public ActionResult ReviewOrder(int id)
        {
            Customer customer = GetLoggedInCustomer();
            OrderItem orderItem = db.OrderItems.FirstOrDefault(o => o.OrderItemId == id);
            double? additionsPrice = 0;
            orderItem.Price = 0;
            orderItem.SizePrice = 0;

            MenuItem menuItem = db.MenuItems.FirstOrDefault(m => m.MenuItemId == orderItem.IdFromMenu);
            double? sizePrice = db.Sizes.FirstOrDefault(s => s.MenuItemId == menuItem.MenuItemId && s.SizeName == orderItem.Size).AdditionalCost;
            
            ICollection <Creamer> creamerList = db.Creamers.Where(c => c.OrderItemId == orderItem.OrderItemId && c.Splashes != 0).ToList();
            ViewBag.creamers = creamerList;
            foreach (var item in creamerList) { additionsPrice += item.Price; }
            ICollection<Drizzle> drizzleList = db.Drizzles.Where(c => c.OrderItemId == orderItem.OrderItemId && c.Drizzles != 0).ToList();
            ViewBag.drizzles = drizzleList;
            foreach (var item in drizzleList) { additionsPrice += item.Price; }
            ICollection<Powder> powderList = db.Powders.Where(c => c.OrderItemId == orderItem.OrderItemId && c.Scoops != 0).ToList();
            ViewBag.powders = powderList;
            foreach (var item in powderList) { additionsPrice += item.Price; }
            ICollection<Sauce> sauceList = db.Sauces.Where(c => c.OrderItemId == orderItem.OrderItemId && c.SaucePumps != 0).ToList();
            ViewBag.sauces = sauceList;
            foreach (var item in sauceList) { additionsPrice += item.Price; }
            ICollection<Shot> shotList = db.Shots.Where(c => c.OrderItemId == orderItem.OrderItemId && c.Shots != 0).ToList();
            ViewBag.shots = shotList;
            foreach (var item in shotList) { additionsPrice += item.Price; }
            ICollection<Sweetener> sweetenerList = db.Sweeteners.Where(c => c.OrderItemId == orderItem.OrderItemId && c.Scoops != 0).ToList();
            ViewBag.sweeteners = sweetenerList;
            foreach (var item in sweetenerList) { additionsPrice += item.Price; }
            ICollection<Syrup> syrupList = db.Syrups.Where(c => c.OrderItemId == orderItem.OrderItemId && c.SyrupPumps != 0).ToList();
            ViewBag.syrups = syrupList;
            foreach (var item in syrupList) { additionsPrice += item.Price; }
            ICollection<Toppings> toppingsList = db.Toppings.Where(c => c.OrderItemId == orderItem.OrderItemId && c.ToppingsAmmount != 0).ToList();
            ViewBag.toppings = toppingsList;
            foreach (var item in toppingsList) { additionsPrice += item.Price; }

            orderItem.Price = menuItem.Price;
            orderItem.Price += additionsPrice;
            orderItem.Price += sizePrice;
            ViewBag.price = orderItem.Price;
            db.SaveChanges();
            return View(orderItem);

        }

        public ActionResult ReviewOrderByEmployee(int id)
        {
            OrderItem orderItem = db.OrderItems.FirstOrDefault(o => o.OrderItemId == id);
            
            ICollection<Creamer> creamerList = db.Creamers.Where(c => c.OrderItemId == orderItem.OrderItemId && c.Splashes != 0).ToList();
            ViewBag.creamers = creamerList;
            ICollection<Drizzle> drizzleList = db.Drizzles.Where(c => c.OrderItemId == orderItem.OrderItemId && c.Drizzles != 0).ToList();
            ViewBag.drizzles = drizzleList;
            ICollection<Powder> powderList = db.Powders.Where(c => c.OrderItemId == orderItem.OrderItemId && c.Scoops != 0).ToList();
            ViewBag.powders = powderList;
            ICollection<Sauce> sauceList = db.Sauces.Where(c => c.OrderItemId == orderItem.OrderItemId && c.SaucePumps != 0).ToList();
            ViewBag.sauces = sauceList;
            ICollection<Shot> shotList = db.Shots.Where(c => c.OrderItemId == orderItem.OrderItemId && c.Shots != 0).ToList();
            ViewBag.shots = shotList;
            ICollection<Sweetener> sweetenerList = db.Sweeteners.Where(c => c.OrderItemId == orderItem.OrderItemId && c.Scoops != 0).ToList();
            ViewBag.sweeteners = sweetenerList;
            ICollection<Syrup> syrupList = db.Syrups.Where(c => c.OrderItemId == orderItem.OrderItemId && c.SyrupPumps != 0).ToList();
            ViewBag.syrups = syrupList;
            ICollection<Toppings> toppingsList = db.Toppings.Where(c => c.OrderItemId == orderItem.OrderItemId && c.ToppingsAmmount != 0).ToList();
            ViewBag.toppings = toppingsList;
            ViewBag.price = orderItem.Price;
            
            return View(orderItem);

        }






        public ActionResult ReviewSelectedItem(int id)
        {
            Customer currentCustomer = GetLoggedInCustomer();
            currentCustomer.CurrentOrderItemId = id;
            db.SaveChanges();

            return RedirectToAction("ReviewOrder", new { id = id });
        }

        public ActionResult ReviewSelectedItemByEmployee(int id)
        {
            OrderItem orderItem = db.OrderItems.FirstOrDefault(o => o.OrderItemId == id);
            Order order = db.Orders.FirstOrDefault(o => o.OrderId == orderItem.OrderId);

            return RedirectToAction("ReviewOrderByEmployee", new { id = id });
        }
        public ActionResult ReviewOrderBeforeCheckout()
        {
            Customer currentCustomer = GetLoggedInCustomer();
            Order customerOrder = db.Orders.FirstOrDefault(o => o.OrderId == currentCustomer.CurrentOrderId);
            List<OrderItem> currentOrderItems = db.OrderItems.Where(o => o.OrderId == customerOrder.OrderId).ToList();
            customerOrder.OrderPrice = 0;

            for (int i = 0; i < currentOrderItems.Count(); i++)
            {
                customerOrder.OrderPrice += currentOrderItems[i].Price;
            }
            ViewBag.TotalOrderPrice = customerOrder.OrderPrice;
            db.SaveChanges();
            return View(currentOrderItems);
             
        }


        public ActionResult SchedulePickUp()
        {
            Customer currentCustomer = GetLoggedInCustomer();
            Order currentOrder = db.Orders.FirstOrDefault(o => o.OrderId == currentCustomer.CurrentOrderId);
            Truck truck = db.Trucks.FirstOrDefault(t => t.TruckId == currentOrder.TruckId);
            double orderMinimumTime = GetOrderFillDuration(currentOrder);

            if (DateTime.Now > truck.StartTime)
            {
                DateTime time = (DateTime.Now);
                time.AddMinutes(orderMinimumTime);
                ViewBag.TruckOpens = TimeConverter(time);
            }
            if (DateTime.Now > truck.EndTime && DateTime.Now < truck.StartTime)
            {
                DateTime time = truck.StartTime;
                time.AddMinutes(orderMinimumTime);
                ViewBag.TruckOpens = TimeConverter(time);
            }
            
            ViewBag.TruckCloses = TimeConverter(truck.EndTime);
            List<Order> ordersList = db.Orders.Where(o => o.TruckId == truck.TruckId).ToList();

            string[][] avaibsArray = new string[ordersList.Count][];

            for (int i = 0; i < ordersList.Count; i++)
            {
                string[] getArray = GetUnavailableTimes(ordersList[i].OrderId);
                avaibsArray[i] = new string[2];
                avaibsArray[i][0] = getArray[0];
                avaibsArray[i][1] = getArray[1];
            }

            string[] finalArray = new string[ordersList.Count];

            for(int i = 0; i < ordersList.Count; i++)
            {
                string[] arr = avaibsArray[i];
                string joined = "[ '" + arr[0] + "', '" + arr[1] + "']";
                
                finalArray[i] = joined;

            }

            string almostJoined = "[ " + string.Join(",", finalArray) + " ]";
            ViewBag.AvaibsArray = almostJoined;

            return View(currentOrder);
        }

        // THAT THING'S OPERATIONAL 



        [HttpPost]
        public ActionResult SchedulePickUp(Order order)
        {
            Customer currentCustomer = GetLoggedInCustomer();
            Order setTimeOrder = db.Orders.FirstOrDefault(o => o.OrderId == currentCustomer.CurrentOrderId);
            setTimeOrder.FillTime = order.FillTime;
            db.SaveChanges();
            return RedirectToAction("FinalOrderReview");
        }


        public ActionResult FinalOrderReview()
        {
            Customer currentCustomer = GetLoggedInCustomer();
            Order currentOrder = db.Orders.FirstOrDefault(o => o.OrderId == currentCustomer.CurrentOrderId);
            List<OrderItem> orderItems = db.OrderItems.Where(o => o.OrderId == currentOrder.OrderId).ToList();
            // Here the customer will select payment option and place the order. They will then be taken to the Unique OrderId page.
            // The Unique Id page will have to be callable once the order is finally placed. We will need to database it up once the order
            // is placed or filled. Also, once the order is placed and the ID assigned, the employee order list will update. 

            ViewBag.Time = currentOrder.FillTime;
            return View(orderItems);
        }

        // Methods for accepting or cancelling final order

        public ActionResult OrderAccepted()
        {
            Customer currentCustomer = GetLoggedInCustomer();
            Order currentOrder = db.Orders.FirstOrDefault(o => o.OrderId == currentCustomer.CurrentOrderId);
            currentOrder.UniqueId = (currentOrder.OrderId % 1000);
            db.SaveChanges();
            return View(currentOrder);
        }

        public ActionResult OrderCancelled()
        {
            return View();
        }


        public ActionResult OrderFilled(int id)
        {
            Order filledOrder = db.Orders.FirstOrDefault(o => o.OrderId == id);
            filledOrder.UniqueId = null;
            db.SaveChanges();
            return RedirectToAction("ViewTruckOrders", "Truck", new { id = filledOrder.TruckId });
        }







        public double GetOrderFillDuration(Order order)
        {
            double minutes = 0;
            List<OrderItem> orderItems = db.OrderItems.Where(o => o.OrderId == order.OrderId).ToList();
            foreach (OrderItem orderItem in orderItems)
            {
                minutes += 1;
            }
            return (minutes);
        }


        public ActionResult TestReviewOrder()
        {
            int id = 99;
            return RedirectToAction("ReviewOrder", new { id = id });

        }


        public string TimeConverter(DateTime time)
        {
            string timeString = time.ToString("hh:mm tt");
            return (timeString);

        }


        public string[] GetUnavailableTimes(int orderId)
        {
            Order order = db.Orders.FirstOrDefault(o => o.OrderId == orderId);

            string[] unavaibs = new string[2];

                double duration = GetOrderFillDuration(order);
                unavaibs[0] = TimeConverter(order.FillTime.AddMinutes(duration * -1));
                unavaibs[1] = TimeConverter(order.FillTime);
            
            return (unavaibs);
        }

       

    }
}
