using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetShopeBusiness.Model;
using NetShopeWeb.EfContext;

namespace MyEcommerceAdmin.Controllers
{
    public class SubCategoryController : Controller
    {
        dbContext db = new dbContext();
        // GET: SubCategory
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            ViewBag.categoryList = new SelectList(db.Categories, "CategoryID", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(SubCategory sctg)
        {
            if (ModelState.IsValid)
            {
                db.SubCategories.Add(sctg);
                db.SaveChanges();
                return PartialView("_Success");
            }
            ViewBag.supplierList = new SelectList(db.Categories, "CategoryID", "Name");
            return PartialView("_Error");
        }

    }
}