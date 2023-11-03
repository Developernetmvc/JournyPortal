using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetShopeBusiness.Model;

namespace MyEcommerceAdmin.Controllers
{
    public class ThankYouController : Controller
    {
        //dbContext db = new dbContext();
        // GET: ThankYou
        public ActionResult Index()
        {
            ViewBag.cartBox = null;
            ViewBag.Total = null;
            ViewBag.NoOfItem = null;
            TempShpData.items = null;
            return View("Thankyou");
        }
    }
}