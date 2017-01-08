using Auction.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Auction.Controllers
{
    public class HomeController : Controller
    {
        private ItemDBContext db = new ItemDBContext();
        public ActionResult Index()
        {
            populateAuctions();
            return View(db.Categories.ToList());
        }
        bool isPresent(Auct a, List<Auct> ls)
        {
            foreach (var it in ls)
            {
                if (it.Name == a.Name)
                    return true;
            }
            return false;
        }
        public void populateAuctions()
        {
            ViewBag.auctionitems = new List<Auct>();
            var lst = new List<Auct>();
            var its = from d in db.Aucts
                      select d;
            lst.AddRange(its.Distinct());
            foreach (var i in lst)
            {
                if (!isPresent(i, ViewBag.auctionitems))
                    ViewBag.auctionitems.Add(i);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Welcome to AUBG Auction!";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}