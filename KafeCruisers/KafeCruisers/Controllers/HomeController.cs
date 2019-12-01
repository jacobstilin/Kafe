using KafeCruisers.Models;
using Microsoft.AspNet.Identity;
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


        [AllowAnonymous]
        public ActionResult Index()
        {
            
            if (User.IsInRole("AppManager") == true)
            {
                ViewBag.role = "AppManager";
            }
            if (User.IsInRole("Customer") == true)
            {
                ViewBag.role = "Customer";
            }
            else
            {
                ViewBag.role = "Visitor";
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}