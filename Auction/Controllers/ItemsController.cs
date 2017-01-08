using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Auction.Models;

namespace Auction.Controllers
{
    public class ItemsController : Controller
    {
        private ItemDBContext db = new ItemDBContext();

        // GET: /Items/
        public ActionResult Index(string itemcategory, string searchString)
        {
            var catlst = new List<string>();
            var items = from d in db.Categories
                        select d.Name;
            catlst.AddRange(items.Distinct());
            ViewBag.itemcategory = new SelectList(catlst);
            var it = from ite in db.Items
                     select ite;
            if (!String.IsNullOrEmpty(searchString))
            {
                it = it.Where(s => s.Name.Contains(searchString));
            }
            if (!String.IsNullOrEmpty(itemcategory))
            {
                it = it.Where(s => s.Category == itemcategory);
            }
            return View(it);
        }

        // GET: /Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: /Items/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            CompositeModel mod = new CompositeModel { cat = db.Categories.ToList() };
            return View(mod);
        }

        // POST: /Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Create(CompositeModel comp, HttpPostedFileBase image1)
        {
            if (ModelState.IsValid)
            {
                if (image1 != null)
                {
                    comp.it.imageData = new byte[image1.ContentLength];
                    image1.InputStream.Read(comp.it.imageData, 0, image1.ContentLength);
                }
                comp.it.Approved = false;
                db.Items.Add(comp.it);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var mod = new CompositeModel { it = comp.it, cat = db.Categories.ToList() };
            return View(mod);
        }
  
        [HttpPost]
        public ActionResult Suggested()
        {
            var items = from it in db.Items
                        where it.Approved == false
                        select it;
            return View(items);
        }

        public ActionResult Suggested(string itemcategory, string searchString)
        {
            var catlst = new List<string>();
            var items = from d in db.Categories
                        select d.Name;
            catlst.AddRange(items.Distinct());
            ViewBag.itemcategory = new SelectList(catlst);
            var it = from ite in db.Items
                     select ite;
            if (!String.IsNullOrEmpty(searchString))
            {
                it = it.Where(s => s.Name.Contains(searchString));
            }
            if (!String.IsNullOrEmpty(itemcategory))
            {
                it = it.Where(s => s.Category == itemcategory);
            }
            return View(it);
        }
        public  ActionResult approveItem(int itemId)
        {
            var ite = from it in db.Items
                      where it.ID == itemId
                      select it;
            var itm = ite.FirstOrDefault();
            itm.Approved = true;
            if (ModelState.IsValid)
            {
                db.SaveChanges();
                return View("~/Views/Home/Index.cshtml");
            }
            return View("~/Views/Home/Index.cshtml");
        }
        // GET: /Items/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: /Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit([Bind(Include="ID,name,category,price,rating")] Item item)
        {
            var it = from i in db.Items
                     where i.ID == item.ID
                     select i;
            var itm = it.FirstOrDefault();
            itm.Name = item.Name;
            itm.Category = item.Category;
            itm.Price = item.Price;
            //Console.WriteLine(item.Name);
            if (ModelState.IsValid)
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(itm);
        }

        public ActionResult Personal()
        {
            var ite = from a in db.Items
                      select a;
            return View(ite);
        }
        public ActionResult CategoryFilter(string cat)
        {
            var ite = from a in db.Items
                      where a.Category == cat
                      select a;
            return View(ite);
        }
        // GET: /Items/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: /Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
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
