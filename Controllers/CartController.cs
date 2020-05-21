using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using EmarketTask.Models;

namespace EmarketTask.Controllers
{
    public class CartController : Controller
    {
        private storeDBEntities db = new storeDBEntities();
        // GET: Cart
        public ActionResult Addtocart(int id)
        {
        //    var product = db.products.SingleOrDefault(c => c.CID == id);
        //    if (product == null)
        //        return HttpNotFound();

            var cart_list = db.Carts.SingleOrDefault(c => c.product_id == id);
            if (cart_list != null)
            {
                return RedirectToAction("Index", "products");
            }
            else
            {
                Cart cart = new Cart();
                cart.product_id = id;
                cart.added_at = DateTime.Now;
                db.Carts.Add(cart);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "products");
        }
        public ActionResult Remove(int id)
        {
            var product = db.Carts.Single(c => c.product_id == id);
            db.Carts.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index", "products");
        }
    }
}