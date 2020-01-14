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
using Stripe;
using Microsoft.AspNet.Identity.EntityFramework;

namespace KafeCruisers.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult Index(int id)
        {
            Models.Customer customer = GetLoggedInCustomer();
            customer.OrderTruckId = id;
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public Models.Customer GetLoggedInCustomer()
        {
            string currentId = User.Identity.GetUserId();
            Models.Customer customer = db.Customers.FirstOrDefault(u => u.ApplicationId == currentId);
            return (customer);
        }


        
        public ActionResult StartOrder()
        {
            Models.Customer customer = GetLoggedInCustomer();
            Models.Order newOrder = new Models.Order();
            newOrder.CustomerId = customer.CustomerId;
            newOrder.TruckId = customer.OrderTruckId;
            newOrder.FillTime = DateTime.Now;
            newOrder.StartTime = DateTime.Now;
            newOrder.Status = 1;
            db.Orders.Add(newOrder);
            db.SaveChanges();
            customer.CurrentOrderId = newOrder.OrderId;
            db.SaveChanges();
            
            Menu truckMenu =GetTruckMenu(customer.OrderTruckId);

            return View(db.MenuItems.Where(m => m.MenuId == truckMenu.MenuId && m.Category == "Drink").ToList());
        }

        //Same method as above, only the customer continues on the same order
        public ActionResult AdditionalDrink()
        {
            Models.Customer customer = GetLoggedInCustomer();
            Menu truckMenu = GetTruckMenu(customer.OrderTruckId);
            
            return View(db.MenuItems.Where(m => m.MenuId == truckMenu.MenuId && m.Category == "Drink").ToList());
        }

        public Menu GetTruckMenu(int? id)
        {
            return db.Menus.FirstOrDefault(m => m.TruckId == id);
        }

        // Gets the id of the drink from the view
        public ActionResult EditOrderItem(int id)
        {
            Models.Customer customer = GetLoggedInCustomer();
            Models.OrderItem orderItem = new Models.OrderItem();
            MenuItem menuItem = db.MenuItems.FirstOrDefault(m => m.MenuItemId == id);
            orderItem.OrderId = customer.CurrentOrderId;
            orderItem.ItemName = menuItem.Name;
            orderItem.Price = menuItem.Price;
            orderItem.IdFromMenu = id;
            orderItem.Duration = menuItem.Duration;
            db.OrderItems.Add(orderItem);
            db.SaveChanges();
            customer.CurrentOrderItemId = orderItem.OrderItemId;
            db.SaveChanges();

            ViewBag.temperatures = new SelectList(db.Temperatures.Where(s => s.MenuItemId == id && s.TemperatureName != null).ToList(), "TemperatureName", "TemperatureName");
            ViewBag.sizes = new SelectList(db.Sizes.Where(s => s.MenuItemId == id && s.SizeName != null).ToList(), "SizeName", "SizeName");


            return View(orderItem);
        }

        public Models.Order GetCurrentOrder(int? id)
        {
            return db.Orders.FirstOrDefault(o => o.OrderId == id);
        }

        [HttpPost]
        public ActionResult EditOrderItem(int id, Models.OrderItem orderItem)
        {
            try
            {
                Models.Customer customer = GetLoggedInCustomer();
                Models.OrderItem newOrderItem = db.OrderItems.FirstOrDefault(o => o.OrderItemId == customer.CurrentOrderItemId);
                newOrderItem.Size = orderItem.Size;
                newOrderItem.Room = orderItem.Room;
                newOrderItem.Decaf = orderItem.Decaf;
                newOrderItem.Temperature = orderItem.Temperature;
                newOrderItem.WhippedCream = orderItem.WhippedCream;

                Models.Order currentOrder = GetCurrentOrder(customer.CurrentOrderId);
                currentOrder.Status = 2;

                db.SaveChanges();



                return RedirectToAction("ReviewOrder", new { id = newOrderItem.OrderItemId });
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
        
        public Models.OrderItem GetOrderItem(int? id)
        {
            return db.OrderItems.FirstOrDefault(o => o.OrderItemId == id);
        }

        [HttpPost]
        public ActionResult EditCreamers(ICollection<MenuItem> creamers)
        {
            Models.Customer customer = GetLoggedInCustomer();
            Models.OrderItem orderItem = GetOrderItem(customer.CurrentOrderItemId);
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
            return RedirectToAction("ReviewOrder", new { id = orderItem.OrderItemId });
        }


        public ActionResult EditShots()
        {
            ICollection<MenuItem> shots = db.MenuItems.Where(m => m.Category == "Shot").ToList();
            return View(shots);

        }

        [HttpPost]
        public ActionResult EditShots(ICollection<MenuItem> shots)
        {
            Models.Customer customer = GetLoggedInCustomer();
            Models.OrderItem orderItem = GetOrderItem(customer.CurrentOrderItemId);
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
            return RedirectToAction("ReviewOrder", new { id = orderItem.OrderItemId });
        }


        public ActionResult EditSweeteners()
        {
            ICollection<MenuItem> sweeteners = db.MenuItems.Where(m => m.Category == "Sweetener").ToList();
            return View(sweeteners);

        }

        [HttpPost]
        public ActionResult EditSweeteners(ICollection<MenuItem> sweeteners)
        {
            Models.Customer customer = GetLoggedInCustomer();
            Models.OrderItem orderItem = GetOrderItem(customer.CurrentOrderItemId);
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
            return RedirectToAction("ReviewOrder", new { id = orderItem.OrderItemId });
        }



        public ActionResult EditSauces()
        {
            ICollection<MenuItem> sauces = db.MenuItems.Where(m => m.Category == "Sauce").ToList();
            return View(sauces);

        }

        [HttpPost]
        public ActionResult EditSauces(ICollection<MenuItem> sauces)
        {
            Models.Customer customer = GetLoggedInCustomer();
            Models.OrderItem orderItem = GetOrderItem(customer.CurrentOrderItemId);
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
            return RedirectToAction("ReviewOrder", new { id = orderItem.OrderItemId });
        }



        public ActionResult EditSyrups()
        {
            ICollection<MenuItem> syrups = db.MenuItems.Where(m => m.Category == "Syrup").ToList();
            return View(syrups);

        }

        [HttpPost]
        public ActionResult EditSyrups(ICollection<MenuItem> syrups)
        {
            Models.Customer customer = GetLoggedInCustomer();
            Models.OrderItem orderItem = GetOrderItem(customer.CurrentOrderItemId);
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
            return RedirectToAction("ReviewOrder", new { id = orderItem.OrderItemId });
        }





        public ActionResult EditDrizzles()
        {
            ICollection<MenuItem> drizzles = db.MenuItems.Where(m => m.Category == "Drizzle").ToList();
            return View(drizzles);

        }

        [HttpPost]
        public ActionResult EditDrizzles(ICollection<MenuItem> drizzles)
        {
            Models.Customer customer = GetLoggedInCustomer();
            Models.OrderItem orderItem = GetOrderItem(customer.CurrentOrderItemId);
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
            return RedirectToAction("ReviewOrder", new { id = orderItem.OrderItemId });
        }



        public ActionResult EditPowders()
        {
            ICollection<MenuItem> powders = db.MenuItems.Where(m => m.Category == "Powder").ToList();
            return View(powders);

        }

        [HttpPost]
        public ActionResult EditPowders(ICollection<MenuItem> powders)
        {
            Models.Customer customer = GetLoggedInCustomer();
            Models.OrderItem orderItem = GetOrderItem(customer.CurrentOrderItemId);
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
            return RedirectToAction("ReviewOrder", new { id = orderItem.OrderItemId });
        }



        public ActionResult EditToppings()
        {
            ICollection<MenuItem> powders = db.MenuItems.Where(m => m.Category == "Toppings").ToList();
            return View(powders);

        }

        [HttpPost]
        public ActionResult EditToppings(ICollection<MenuItem> toppings)
        {
            Models.Customer customer = GetLoggedInCustomer();
            Models.OrderItem orderItem = GetOrderItem(customer.CurrentOrderItemId);
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
            Models.Customer customer = GetLoggedInCustomer();
            Models.OrderItem orderItem = GetOrderItem(id);
            double? additionsPrice = 0;
            orderItem.Price = 0;
            orderItem.SizePrice = 0;

            MenuItem menuItem = db.MenuItems.FirstOrDefault(m => m.MenuItemId == orderItem.IdFromMenu);
            double? sizePrice = db.Sizes.FirstOrDefault(s => s.MenuItemId == menuItem.MenuItemId && s.SizeName == orderItem.Size).AdditionalCost;
            if (sizePrice == null)
            {
                sizePrice = 0;
            }
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
            Models.OrderItem orderItem = db.OrderItems.FirstOrDefault(o => o.OrderItemId == id);
            
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
            Models.Customer currentCustomer = GetLoggedInCustomer();
            currentCustomer.CurrentOrderItemId = id;
            db.SaveChanges();

            return RedirectToAction("ReviewOrder", new { id = id });
        }

        public ActionResult ReviewSelectedItemByEmployee(int id)
        {
            Models.OrderItem orderItem = db.OrderItems.FirstOrDefault(o => o.OrderItemId == id);
            Models.Order order = db.Orders.FirstOrDefault(o => o.OrderId == orderItem.OrderId);

            return RedirectToAction("ReviewOrderByEmployee", new { id = id });
        }
        public ActionResult ReviewOrderBeforeCheckout()
        {
            Models.Customer currentCustomer = GetLoggedInCustomer();
            Models.Order customerOrder = db.Orders.FirstOrDefault(o => o.OrderId == currentCustomer.CurrentOrderId);
            List<Models.OrderItem> currentOrderItems = db.OrderItems.Where(o => o.OrderId == customerOrder.OrderId).ToList();
            customerOrder.OrderPrice = GetOrderPrice(customerOrder.OrderId);
            customerOrder.Duration = 1;

            for (int i = 0; i < currentOrderItems.Count(); i++)
            {
                customerOrder.Duration += currentOrderItems[i].Duration;
            }
            ViewBag.TotalOrderPrice = customerOrder.OrderPrice;
            db.SaveChanges();
            return View(currentOrderItems);
             
        }

        public double? GetOrderPrice(int id)
        {
            double? orderPrice = 0;
            Models.Order order = db.Orders.FirstOrDefault(o => o.OrderId == id);
            List<Models.OrderItem> orderItems = db.OrderItems.Where(o => o.OrderId == id).ToList();
            for (int i = 0; i < orderItems.Count(); i++)
            {
                orderPrice += orderItems[i].Price;
            }

            return orderPrice;
        }

        public ActionResult RemoveOrderItem(int id)
        {
            Models.OrderItem orderItem = db.OrderItems.FirstOrDefault(o => o.OrderItemId == id);
            db.OrderItems.Remove(orderItem);
            db.SaveChanges();
            return RedirectToAction("ReviewOrderBeforeCheckout");

        }

        public void RemoveIncompleteItem(int id)
        {
            Models.OrderItem orderItem = db.OrderItems.FirstOrDefault(o => o.OrderItemId == id);
            db.OrderItems.Remove(orderItem);
            db.SaveChanges();
        }


        public ActionResult SchedulePickUp()
        {
            Models.Customer currentCustomer = GetLoggedInCustomer();
            Models.Order currentOrder = db.Orders.FirstOrDefault(o => o.OrderId == currentCustomer.CurrentOrderId);
            Truck truck = db.Trucks.FirstOrDefault(t => t.TruckId == currentOrder.TruckId);
            double orderMinimumTime = GetOrderFillDuration(currentOrder) + 5;

            // Convert truck open time to the time on the current day

            DateTime closedTime = (DateTime.Now);
            DateTime closedNewTime = closedTime.AddMinutes(orderMinimumTime);
            ViewBag.TruckOpens = TimeConverter(closedNewTime);

            if (DateTime.Now.TimeOfDay > truck.StartTime.TimeOfDay)
            {
                DateTime time = (DateTime.Now);
                DateTime newTime = time.AddMinutes(orderMinimumTime);
                ViewBag.TruckOpens = TimeConverter(newTime);
            }
            if (DateTime.Now.TimeOfDay > truck.EndTime.TimeOfDay)
            {
                DateTime time = truck.StartTime;
                DateTime newTime = time.AddMinutes(orderMinimumTime);
                ViewBag.TruckOpens = TimeConverter(newTime);
            }
            
            ViewBag.TruckCloses = TimeConverter(truck.EndTime);
            List<Models.Order> ordersList = db.Orders.Where(o => o.TruckId == truck.TruckId).ToList();

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
        public ActionResult SchedulePickUp(Models.Order order)
        {
            Models.Customer currentCustomer = GetLoggedInCustomer();
            Models.Order setTimeOrder = db.Orders.FirstOrDefault(o => o.OrderId == currentCustomer.CurrentOrderId);
            double orderMinimumTime = GetOrderFillDuration(setTimeOrder);
            setTimeOrder.FillTime = order.FillTime;
            setTimeOrder.StartTime = order.FillTime.AddMinutes(orderMinimumTime * -1);
            setTimeOrder.Status = 3;
            db.SaveChanges();
            return RedirectToAction("FinalOrderReview");
        }


        public ActionResult FinalOrderReview()
        {
            Models.Customer currentCustomer = GetLoggedInCustomer();
            Models.Order currentOrder = db.Orders.FirstOrDefault(o => o.OrderId == currentCustomer.CurrentOrderId);
            List<Models.OrderItem> orderItems = db.OrderItems.Where(o => o.OrderId == currentOrder.OrderId).ToList();
            // Here the customer will select payment option and place the order. They will then be taken to the Unique OrderId page.
            // The Unique Id page will have to be callable once the order is finally placed. We will need to database it up once the order
            // is placed or filled. Also, once the order is placed and the ID assigned, the employee order list will update. 


            ViewBag.Time = currentOrder.FillTime;
            return View(orderItems);
        }

        // Methods for accepting or cancelling final order
        // More on this later--let's use the cart model and controller I suppose
        public ActionResult OrderAccepted()
        {
            Models.Customer currentCustomer = GetLoggedInCustomer();
            CardInfo card = db.Cards.FirstOrDefault(c => c.CustomerId == currentCustomer.CustomerId);
            
            if (currentCustomer.IsStripeSetUp == true)
            {

                return View(card);  // Create charge using customer info
            }
            else
            {
                return RedirectToAction("SetUpStripe");
            }
        }

        
        public ActionResult UsePreloadedCard()  //We can pass whatever we need here since current order can always be called
        {
            Models.Customer currentCustomer = GetLoggedInCustomer();
            Models.Order currentOrder = db.Orders.FirstOrDefault(o => o.OrderId == currentCustomer.CurrentOrderId);
            StripeConfiguration.ApiKey = (APIKeys.StripeApiKey);

            // `source` is obtained with Stripe.js; see https://stripe.com/docs/payments/accept-a-payment-charges#web-create-token
            var options = new ChargeCreateOptions
            {
                Amount = Convert.ToInt64(currentOrder.OrderPrice * 100),
                Currency = "usd",
                Customer = currentCustomer.CustomerStripeId,
                Description = "Order for " + currentCustomer.FirstName + " " + currentCustomer.lastName,
            };
            var service = new ChargeService();
            Charge charge = service.Create(options);
            var model = new Cart();
            model.ChargeId = charge.Id;

            return RedirectToAction("UniqueIdScreen");
        }

        public ActionResult SetUpStripe() //Build this view for customer info input
        {
            return View();
        }

        [HttpPost]
        public ActionResult SetUpStripe(CardInfo cardInfo)  // Create customer then create charge in next view
        {
            Models.Customer currentCustomer = GetLoggedInCustomer();
            Models.Order currentOrder = db.Orders.FirstOrDefault(o => o.OrderId == currentCustomer.CurrentOrderId);
            CardInfo newCard = new CardInfo();
            newCard.CardName = cardInfo.CardName;
            newCard.Email = cardInfo.Email;
            newCard.CustomerId = currentCustomer.CustomerId;
            db.Cards.Add(newCard);

            StripeConfiguration.ApiKey = (APIKeys.StripeApiKey);

            var options = new CustomerCreateOptions
            {
                Description = ("Customer for " + cardInfo.Email),
                Email = cardInfo.Email,
                Source = cardInfo.StripeToken
            };
            var service = new CustomerService();
            Stripe.Customer customer = service.Create(options);
            currentCustomer.CustomerStripeId = customer.Id;
            currentCustomer.IsStripeSetUp = true;
            db.SaveChanges();

            var createOptions = new ChargeCreateOptions
            {
                Amount = Convert.ToInt64(currentOrder.OrderPrice * 100),
                Currency = "usd",
                Customer = currentCustomer.CustomerStripeId, // Have to pass in customer ID since token can't be called twice
                Description = "Order for " + currentCustomer.FirstName + " " + currentCustomer.lastName,
            };
            var createService = new ChargeService();
            Charge charge = createService.Create(createOptions);

            var model = new Cart(); //Cart is like the charge view model from video
            model.ChargeId = charge.Id;

            // Perhaps payment will happen with this setup for the first time

            return RedirectToAction("UniqueIdScreen"); // This redirect may have to change
        }


        

        public ActionResult UniqueIdScreen()   //Order is all set for pickup, all roads lead here
        {
            Models.Customer currentCustomer = GetLoggedInCustomer();
            Models.Order currentOrder = db.Orders.FirstOrDefault(o => o.OrderId == currentCustomer.CurrentOrderId);
            currentOrder.UniqueId = (currentOrder.OrderId % 1000);
            currentOrder.Status = 4;
            db.SaveChanges();

            return View(currentOrder);
        }

        public ActionResult ResumePastOrder(int id)
        {
            Models.Customer customer = GetLoggedInCustomer();
            customer.CurrentOrderId = id;
            db.SaveChanges();

            return RedirectToAction("ResumeOrder");
        }

        public ActionResult ResumeOrder()
        {
            Models.Customer customer = GetLoggedInCustomer();
            Models.Order order = db.Orders.FirstOrDefault(o => o.OrderId == customer.CurrentOrderId);

            if(order.StartTime < DateTime.Now) // if they try to schedule a time after the start time of the order? should work
            {
                order.Status = 2;
                db.SaveChanges();
            }

            List<Models.OrderItem> incompleteOrders = db.OrderItems.Where(o => o.Size == null).ToList();
            foreach (Models.OrderItem item in incompleteOrders)
            {
                db.OrderItems.Remove(item);
                db.SaveChanges();
            }

            if (order.Status == 1)
            {
                return RedirectToAction("AdditionalDrink");
            }

            if (order.Status == 2)
            {
                return RedirectToAction("ReviewOrderBeforeCheckout");
            }

            if (order.Status == 3)
            {
                return RedirectToAction("FinalOrderReview"); // take customer to payment
            }

            // If order has no drinks, directs to start order or additional drink using status 1
            // If order has drinks, directs to the order review/add drinks menu
            // If order is scheduled, determines whether order must be rescheduled and directs accordingly
            // If order is paid, goes to unique id
            // If order is filled, resume order is not present. 

            
                return RedirectToAction("UniqueIdScreen");
            
            
            
        }



        public ActionResult OrderCancelled()
        {
            Models.Customer customer = GetLoggedInCustomer();
            Models.Order order = db.Orders.FirstOrDefault(o => o.OrderId == customer.CurrentOrderId);
            List<Models.OrderItem> deleteList = db.OrderItems.Where(o => o.OrderId == order.OrderId).ToList();
            foreach (var item in deleteList)
            {
                db.OrderItems.Remove(item);
            }
            db.Orders.Remove(order);
            customer.CurrentOrderId = null;
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }


        public ActionResult OrderFilled(int id)
        {
            Models.Order filledOrder = db.Orders.FirstOrDefault(o => o.OrderId == id);
            try {
                filledOrder.Status = 5;
                var now = DateTime.Now;
                filledOrder.FillTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            }
            catch
            {
                
            }
            db.SaveChanges();
            return RedirectToAction("ViewTruckOrders", "Truck", new { id = filledOrder.TruckId });
        }

        public ActionResult ViewMyOrders()
        {
            Models.Customer customer = GetLoggedInCustomer();
            List<Models.Order> currentOrders = db.Orders.Where(o => o.CustomerId == customer.CustomerId && o.Status != 5).ToList();
            foreach (var item in currentOrders)
            {
                item.OrderPrice = GetOrderPrice(item.OrderId);
            }
            List<Models.Order> pastOrders = db.Orders.Where(o => o.CustomerId == customer.CustomerId && o.Status == 5).ToList();
            db.SaveChanges();
            ViewBag.CurrentOrders = currentOrders;
            ViewBag.PastOrders = pastOrders;

            return View();
        }





        public double GetOrderFillDuration(Models.Order order)
        {
            double minutes = Convert.ToDouble(order.Duration);
            
            return (minutes);
        }


        public ActionResult TestReviewOrder()
        {
            int id = 68;
            return RedirectToAction("ReviewOrder", new { id = id });

        }


        public string TimeConverter(DateTime time)
        {
            string timeString = time.ToString("hh:mm tt");
            return (timeString);

        }


        public string[] GetUnavailableTimes(int orderId)
        {
            Models.Order order = db.Orders.FirstOrDefault(o => o.OrderId == orderId);

            string[] unavaibs = new string[2];

                double duration = GetOrderFillDuration(order);
                unavaibs[0] = TimeConverter(order.FillTime.AddMinutes(duration * -1));
                unavaibs[1] = TimeConverter(order.FillTime);
            
            return (unavaibs);
        }





        // The following methods are used for authentication

        public bool isAppManager()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "AppManager")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }


        public bool isCustomer()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Customer")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}
