﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetShopeBusiness.Model;
using NetShopeWeb.EfContext;

namespace MyEcommerceAdmin.Controllers
{
    public class HomeController : Controller
    {
        dbContext db = new dbContext();

        // GET: Home
        public ActionResult Index()
        {
            ViewBag.MenProduct = db.Products.Where(x => x.Category.Name.Equals("Diaper")).ToList();
            ViewBag.WomenProduct = db.Products.Where(x => x.Category.Name.Equals("Women's Fashion")).ToList();
            ViewBag.AccessoriesProduct = db.Products.Where(x => x.Category.Name.Equals("Electronic Accessories")).ToList();
            ViewBag.ElectronicsProduct = db.Products.Where(x => x.Category.Name.Equals("Electronic Devices")).ToList();
            ViewBag.Slider = db.genMainSliders.ToList();
            ViewBag.PromoRight = db.genPromoRights.ToList();

            this.GetDefaultData();

            return View();
        }
    }
}