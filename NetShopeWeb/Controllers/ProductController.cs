﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetShopeBusiness.Model;
using NetShopeBusiness.Model.Model;
using NetShopeWeb.EfContext;
using NetShopeWeb.ViewModel;

namespace MyEcommerceAdmin.Controllers
{
    public class ProductController : Controller
    {
        dbContext db = new dbContext();
        // GET: Product
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }


        // CREATE: Product

        public ActionResult Create()
        {
            ViewBag.supplierList = new SelectList(db.Suppliers, "SupplierID", "CompanyName");
            ViewBag.categoryList = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.SubCategoryList = new SelectList(db.SubCategories, "SubCategoryID", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(ProductVM pvm)
        {
            if (ModelState.IsValid)
            {
                if (pvm.Picture != null)
                {

                    Product p = new Product
                    {
                        ProductID = pvm.ProductID,
                        Name = pvm.Name,
                        SupplierID = pvm.SupplierID,
                        CategoryID = pvm.CategoryID,
                        SubCategoryID = pvm.SubCategoryID,
                        UnitPrice = pvm.UnitPrice,
                        OldPrice = pvm.OldPrice,
                        Discount = pvm.Discount,
                        UnitInStock = pvm.UnitInStock,
                        ProductAvailable = pvm.ProductAvailable,
                        ShortDescription = pvm.ShortDescription,
                        Note = pvm.Note,
                        //PicturePath = filePath
                    };
                    SavePicture(pvm);
                    db.Products.Add(p);
                    db.SaveChanges();
                    return PartialView("_Success");
                }
            }
            ViewBag.supplierList = new SelectList(db.Suppliers, "SupplierID", "CompanyName");
            ViewBag.categoryList = new SelectList(db.Suppliers, "CategoryID", "Name");
            ViewBag.SubCategoryList = new SelectList(db.Suppliers, "SubCategoryID", "Name");
            return PartialView("_Error");
        }
        private void SavePicture(ProductVM pvm)
        {
            if (pvm.Picture != null)
            {
                foreach (var file in pvm.Picture)
                {
                    if (file != null && file.ContentLength > 0) // Check if the file has content
                    {
                        string directoryPath = Server.MapPath("~/Images");

                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string filePath = Path.Combine(directoryPath, fileName);
                        file.SaveAs(filePath);

                        ProductPicture picture = new ProductPicture
                        {
                            PicturePath = fileName, // Save only the file name in the database, not the full path
                            ProductID = pvm.ProductID,
                        };
                        db.ProductPictures.Add(picture);
                    }
                }

                //db.SaveChanges(); // Save changes to the database after adding pictures
            }
        }




        // EDIT: Product


        public ActionResult Edit(int id)
        {
            Product p = db.Products.Find(id);

            ProductVM pvm = new ProductVM
            {
                ProductID = p.ProductID,
                Name = p.Name,
                SupplierID = p.SupplierID,
                CategoryID = p.CategoryID,
                SubCategoryID = p.SubCategoryID,
                UnitPrice = p.UnitPrice,
                OldPrice = p.OldPrice,
                Discount = p.Discount,
                UnitInStock = p.UnitInStock,
                ProductAvailable = p.ProductAvailable,
                ShortDescription = p.ShortDescription,
                Note = p.Note,
                //PicturePath = p.PicturePath

            };
            ViewBag.supplierList = new SelectList(db.Suppliers, "SupplierID", "CompanyName");
            ViewBag.categoryList = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.SubCategoryList = new SelectList(db.SubCategories, "SubCategoryID", "Name");
            return View(pvm);
        }

        [HttpPost]

        public ActionResult Edit(ProductVM pvm)
        {

            if (ModelState.IsValid)
            {
                string filePath = pvm.PicturePath;
                if (pvm.Picture != null)
                {
                    //filePath = Path.Combine("~/Images", Guid.NewGuid().ToString() + Path.GetExtension(pvm.Picture.FileName));
                    //pvm.Picture.SaveAs(Server.MapPath(filePath));

                    Product p = new Product
                    {
                        ProductID = pvm.ProductID,
                        Name = pvm.Name,
                        SupplierID = pvm.SupplierID,
                        CategoryID = pvm.CategoryID,
                        SubCategoryID = pvm.SubCategoryID,
                        UnitPrice = pvm.UnitPrice,
                        OldPrice = pvm.OldPrice,
                        Discount = pvm.Discount,
                        UnitInStock = pvm.UnitInStock,
                        ProductAvailable = pvm.ProductAvailable,
                        ShortDescription = pvm.ShortDescription,
                        Note = pvm.Note,
                        //PicturePath = filePath
                    };
                    db.Entry(p).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    Product p = new Product
                    {
                        ProductID = pvm.ProductID,
                        Name = pvm.Name,
                        SupplierID = pvm.SupplierID,
                        CategoryID = pvm.CategoryID,
                        SubCategoryID = pvm.SubCategoryID,
                        UnitPrice = pvm.UnitPrice,
                        OldPrice = pvm.OldPrice,
                        Discount = pvm.Discount,
                        UnitInStock = pvm.UnitInStock,
                        ProductAvailable = pvm.ProductAvailable,
                        ShortDescription = pvm.ShortDescription,
                        Note = pvm.Note,
                        //PicturePath = filePath
                    };
                    db.Entry(p).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.supplierList = new SelectList(db.Suppliers, "SupplierID", "CompanyName");
            ViewBag.categoryList = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.SubCategoryList = new SelectList(db.SubCategories, "SubCategoryID", "Name");
            return PartialView(pvm);
        }


        // DETAILS: Product
        public ActionResult Details(int id)
        {
            Product p = db.Products.Find(id);

            ProductVM pvm = new ProductVM
            {
                ProductID = p.ProductID,
                Name = p.Name,
                SupplierID = p.SupplierID,
                CategoryID = p.CategoryID,
                SubCategoryID = p.SubCategoryID,
                UnitPrice = p.UnitPrice,
                OldPrice = p.OldPrice,
                Discount = p.Discount,
                UnitInStock = p.UnitInStock,
                ProductAvailable = p.ProductAvailable,
                ShortDescription = p.ShortDescription,
                Note = p.Note,
                //PicturePath = p.PicturePath

            };
            ViewBag.supplierList = new SelectList(db.Suppliers, "SupplierID", "CompanyName");
            ViewBag.categoryList = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.SubCategoryList = new SelectList(db.SubCategories, "SubCategoryID", "Name");
            return View(pvm);
        }

        [HttpPost]

        public ActionResult Details(ProductVM pvm)
        {

            if (ModelState.IsValid)
            {
                string filePath = pvm.PicturePath;
                if (pvm.Picture != null)
                {
                    //    filePath = Path.Combine("~/Images", Guid.NewGuid().ToString() + Path.GetExtension(pvm.Picture.FileName));
                    //    pvm.Picture.SaveAs(Server.MapPath(filePath));

                    Product p = new Product
                    {
                        ProductID = pvm.ProductID,
                        Name = pvm.Name,
                        SupplierID = pvm.SupplierID,
                        CategoryID = pvm.CategoryID,
                        SubCategoryID = pvm.SubCategoryID,
                        UnitPrice = pvm.UnitPrice,
                        OldPrice = pvm.OldPrice,
                        Discount = pvm.Discount,
                        UnitInStock = pvm.UnitInStock,
                        ProductAvailable = pvm.ProductAvailable,
                        ShortDescription = pvm.ShortDescription,
                        Note = pvm.Note,
                        //PicturePath = filePath
                    };
                    db.Entry(p).State = System.Data.Entity.EntityState.Unchanged;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    Product p = new Product
                    {
                        ProductID = pvm.ProductID,
                        Name = pvm.Name,
                        SupplierID = pvm.SupplierID,
                        CategoryID = pvm.CategoryID,
                        SubCategoryID = pvm.SubCategoryID,
                        UnitPrice = pvm.UnitPrice,
                        OldPrice = pvm.OldPrice,
                        Discount = pvm.Discount,
                        UnitInStock = pvm.UnitInStock,
                        ProductAvailable = pvm.ProductAvailable,
                        ShortDescription = pvm.ShortDescription,
                        Note = pvm.Note,
                        //PicturePath = filePath
                    };
                    db.Entry(p).State = System.Data.Entity.EntityState.Unchanged;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.supplierList = new SelectList(db.Suppliers, "SupplierID", "CompanyName");
            ViewBag.categoryList = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.SubCategoryList = new SelectList(db.SubCategories, "SubCategoryID", "Name");
            return PartialView(pvm);
        }


        // DELETE: Product

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirm(int id)
        //{
        //    Product product = db.Products.Find(id);
        //    string file_name = product.PicturePath;
        //    string path = Server.MapPath(file_name);
        //    FileInfo file = new FileInfo(path);
        //    if (file.Exists)
        //    {
        //        file.Delete();
        //    }
        //    db.Products.Remove(product);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
    }
}