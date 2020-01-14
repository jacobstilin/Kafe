using KafeCruisers.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KafeCruisers.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public Employee GetLoggedInEmployee()
        {
            string currentId = User.Identity.GetUserId();
            Employee employee = db.Employees.FirstOrDefault(u => u.ApplicationId == currentId);
            return (employee);
        }

        public Models.Customer GetLoggedInCustomer()
        {
            string currentId = User.Identity.GetUserId();
            Models.Customer customer = db.Customers.FirstOrDefault(u => u.ApplicationId == currentId);
            return (customer);
        }


        [AllowAnonymous]
        public ActionResult Index()
        {
            
            if (isAppManager())
            {
                ViewBag.role = "AppManager";
            }
            if (isCustomer())
            {
                Customer customer = GetLoggedInCustomer();
                Order currentCustomerOrder = db.Orders.FirstOrDefault(o => o.OrderId == customer.CurrentOrderId);
                try
                {
                    if (customer.CurrentOrderId != null && currentCustomerOrder.Status != 5)
                    {
                        ViewBag.Resume = true;
                    }
                }
                catch { }
                ViewBag.role = "Customer";
            }
            else if(!isCustomer() && !isAppManager())
            {
                ViewBag.role = "Visitor";
            }
            return View();
        }

        
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