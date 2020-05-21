﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EmarketTask.Models;
using EmarketTask.ViewModels;

namespace EmarketTask.Controllers
{
    public class productsController : Controller
    {
        private storeDBEntities db = new storeDBEntities();

        // GET: products
        public ActionResult Index(string search)
        {
                var carts = db.Carts.ToList();
                var products = db.products.ToList();
                var categories = db.categories.ToList();
           
                if (!string.IsNullOrEmpty(search))
                {
                    category x = db.categories
                        .Where(c => c.CName == search).FirstOrDefault<category>();
                    if (x != null)
                    {
                        products = db.products
                            .Where(s => s.CID == x.CID).ToList();
                    }
                
            }
            ProductCart productcartmodel = new ProductCart()
            {
                cart = carts,
                myproducts = products
            };
            return View(productcartmodel);
            
            /*var products = db.products.Include(p => p.Cart).Include(p => p.category);
            return View(products.ToList());*/
        }

        // GET: products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: products/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.Carts, "product_id", "product_id");
            ViewBag.CID = new SelectList(db.categories, "CID", "CName");
            return View();
        }

        // POST: products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Price,Image,Description,CID")] product product, HttpPostedFileBase imgFile)
        {
            if (ModelState.IsValid)
            {
                string path = "";
                if (imgFile.FileName.Length > 0)
                {
                    path = "~/Images/" + Path.GetFileName(imgFile.FileName);
                    imgFile.SaveAs(Server.MapPath(path));
                }
                product.Image = path;
                db.products.Add(product);
                var categoryindb = db.categories.Single(c => c.CID == product.CID);
                categoryindb.Nun_of_products++;
                db.SaveChanges();
                return RedirectToAction("Index");
                
            }

            ViewBag.Id = new SelectList(db.Carts, "product_id", "product_id", product.Id);
            ViewBag.CID = new SelectList(db.categories, "CID", "CName", product.CID);
            return View(product);
        }

        // GET: products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.Carts, "product_id", "product_id", product.Id);
            ViewBag.CID = new SelectList(db.categories, "CID", "CName", product.CID);
            return View(product);
        }

        // POST: products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,Image,Description,CID")] product product, HttpPostedFileBase imgFile)
        {
            if (ModelState.IsValid)
            {
                string path = "";
                product p = new product();
                product product1 = db.products.Find(p.Image);
                if (imgFile.FileName.Length > 0)
                {
                    path = "~/Images/" + Path.GetFileName(imgFile.FileName);
                    imgFile.SaveAs(Server.MapPath(path));
                }
                product.Image = path;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.Carts, "product_id", "product_id", product.Id);
            ViewBag.CID = new SelectList(db.categories, "CID", "CName", product.CID);
            return View(product);
        }

        // GET: products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            product product = db.products.Find(id);
            db.products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        
    }
}
