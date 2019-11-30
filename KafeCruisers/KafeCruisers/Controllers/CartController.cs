using KafeCruisers.Models;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KafeCruisers.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult CreateCharge()
        {
            return View();
        }


        [HttpPost]
        public ActionResult CreateCharge(string stripeToken)
        {
            StripeConfiguration.ApiKey = "sk_test_JdPkYZWnWCpi8HniD2sZLWcs00NG91yvTs";

            // `source` is obtained with Stripe.js; see https://stripe.com/docs/payments/accept-a-payment-charges#web-create-token
            var options = new ChargeCreateOptions
            {
                Amount = 2000,
                Currency = "usd",
                Source = stripeToken,
                Description = "Aow",
            };
            var service = new ChargeService();
            Charge charge = service.Create(options);

            var model = new Cart();
            model.ChargeId = charge.Id;

            return View();
        }
    }
}