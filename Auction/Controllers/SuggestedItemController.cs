using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Auction.Models;
using Auction.Helpers;

namespace Auction.Controllers
{
    [Authorize]
    public class SuggestedItemController : Controller
    {
        private ItemDBContext db = new ItemDBContext();

        // GET: SuggestedItem
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View(db.SuggestedItems.ToList());
        }

        // GET: SuggestedItem/Details/5
        [Authorize(Roles = "admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SuggestedItem suggestedItem = db.SuggestedItems.Find(id);
            if (suggestedItem == null)
            {
                return HttpNotFound();
            }
            return View(suggestedItem);
        }

        // GET: SuggestedItem/Create
        [Authorize(Roles = "registeredCustomer")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: SuggestedItem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "registeredCustomer")]
        public ActionResult Create(SuggestedItem suggestedItem, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    suggestedItem.PhotoData = new byte[image.ContentLength];
                    image.InputStream.Read(suggestedItem.PhotoData, 0, image.ContentLength);
                }

                db.SuggestedItems.Add(suggestedItem);
                db.SaveChanges();
                return RedirectToAction("ThankYou", "SuggestedItem");
            }

            return View(suggestedItem);
        }

        // GET: SuggestedItem/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SuggestedItem suggestedItem = db.SuggestedItems.Find(id);
            if (suggestedItem == null)
            {
                return HttpNotFound();
            }
            return View(suggestedItem);
        }

        // POST: SuggestedItem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit([Bind(Include = "ID,Name,Description,Rating,PhotoData")] SuggestedItem suggestedItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(suggestedItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(suggestedItem);
        }

        // GET: SuggestedItem/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SuggestedItem suggestedItem = db.SuggestedItems.Find(id);
            if (suggestedItem == null)
            {
                return HttpNotFound();
            }
            return View(suggestedItem);
        }

        // POST: SuggestedItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            SuggestedItem suggestedItem = db.SuggestedItems.Find(id);
            db.SuggestedItems.Remove(suggestedItem);
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

        [Authorize(Roles = "admin")]
        public ActionResult Approve(int id)
        {
            SuggestedItem suggestedItem = db.SuggestedItems.Find(id);
            if (suggestedItem == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                //TODO: add appropriate category

                var item = new Item { Name = suggestedItem.Name, Category = "Suggested", Price = 0,
                  Rating = suggestedItem.Rating, imageData = suggestedItem.PhotoData };
                db.Items.Add(item);
                db.SuggestedItems.Remove(suggestedItem);
                db.SaveChanges();
                return RedirectToAction("Approved");
            }

            return View(suggestedItem.ID);
        }

        [NoDirectAccess]
        public ActionResult Approved()
        {
            ViewBag.Message = "The suggested item was approved!";

            return View();
        }

        [NoDirectAccess]
        public ActionResult ThankYou()
        {
            ViewBag.Message = "The administrator will review your submission soon.";

            return View();
        }

        [NoDirectAccess]
        [Authorize(Roles = "admin")]
        public FileContentResult GetImage(int id)
        {
            SuggestedItem suggestedItem = db.SuggestedItems.Find(id);
            if (suggestedItem.PhotoData != null)
            {
                return File(suggestedItem.PhotoData, "jpg");
            }
            else
            {
                return null;
            }
        }

    }
}
