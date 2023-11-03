using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetShopeBusiness;
using NetShopeWeb.EfContext;

namespace MyEcommerceAdmin.Controllers
{
    public class ProfileController : Controller
    {
        dbContext db = new dbContext();
        // GET: Profile
        public ActionResult Index()
        {
            return View(db.admin_Employee.Find(TemData.EmpID));
        }
    }
}