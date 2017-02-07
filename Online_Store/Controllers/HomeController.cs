using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Online_Store.Models;

namespace Online_Store.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(string search)
        {
            if (!String.IsNullOrEmpty(search))
            {
                return View("searchResult",db.products.Where(s => s.name.Contains(search)));
            } 
            return View(db.products.OrderByDescending(x => x.date).Take(10).ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        [AllowAnonymous]
        [HttpGet]
        public ActionResult search(string search)
        {
            if (!String.IsNullOrEmpty(search))
            {
                var resulat = db.products.Where(s => s.name.Contains(search));
                if (resulat == null)
                {
                    return View("searchResult");
                }
                return View("searchResult", resulat);
            } 
            return View();
        }


        //[HttpPost]
        //[AllowAnonymous]
        
        //public ActionResult search(string search)
        //{
        //    if (!String.IsNullOrEmpty(search))
        //    {
        //        var product = db.products.Where(s => s.name.Contains(search)).ToList();
        //        return View("searchResult",product);
        //    }
        //    return View();
        //}
        //public ActionResult search(string search)
        //{
        //    if (!String.IsNullOrEmpty(search))
        //    {
        //        var product = db.products.Where(s => s.name.Contains(search)).ToList();
        //        return Json(product, JsonRequestBehavior.AllowGet);
        //    }
        //    return View();
        //}
        public List<string> Searches(string name)
        {
            return db.products.Where(z => z.name.StartsWith(name)).
                Select(p => p.name).ToList();
        }

    }
}