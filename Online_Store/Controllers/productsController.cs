using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Online_Store.Models;
using System.Drawing;
using System.IO;
using System.Security.Claims;
using System.Threading;
using ImageResizer;
using Microsoft.AspNet.Identity;
using Online_Store.Classes;
using PagedList;


namespace Online_Store.Controllers
{
    public class productsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        

        // GET: products
        public ActionResult Index(int? pageNumber, int? pageSize, int? sortBy)
        {
            //var products = db.products.Include(p => p.catgorie).Include(p => p.type);
            //var product = db.products.ToList();

            if (sortBy == 2)
            {
                var product = db.products.ToList().OrderBy(z => z.name);
               return View(product.ToPagedList(pageNumber ?? 1, pageSize ?? 6));
              
            }
            else
            {
                var product = db.products.ToList().OrderBy(z => z.price);
                return View(product.ToPagedList(pageNumber ?? 1, pageSize ?? 6));
            }
             
            //switch (sortBy)
            //{
            //    case 1:
            //        var product = db.products.ToList().OrderBy(z => z.price);
            //        return View(product.ToPagedList(pageNumber ?? 1, pageSize ?? 6));
            //        break;
            //    case 2:
            //        product = db.products.ToList().OrderBy(z => z.name);
            //        return View(product.ToPagedList(pageNumber ?? 1, pageSize ?? 6));
            //        break;
            //    default:
            //        product = db.products.ToList().OrderBy(z => z.name);
            //        return View(product.ToPagedList(pageNumber ?? 1, pageSize ?? 6));
            //        break;
            //}
        }

        public ActionResult catSelect(int? pageNumber, int? pageSize, int catSelected)
        {
            ViewBag.TopicName = db.Cats.Where(w => w.CatId == catSelected).SingleOrDefault().CatName;
            if (catSelected == db.Cats.Where(w => w.CatId == catSelected).SingleOrDefault().CatId)
            {
                var product = db.products.Where(z => z.catgorie == catSelected).ToList();
                return View(product.ToPagedList(pageNumber ?? 1, pageSize ?? 6));
            }
            return View();
        }
        public ActionResult typeSelect(int? pageNumber, int? pageSize, int typeSelected)
        {
            ViewBag.TopicName = db.Types.Where(w => w.TypeId == typeSelected).SingleOrDefault().TypeName;
            if (typeSelected == db.Types.Where(w => w.TypeId == typeSelected).SingleOrDefault().TypeId)
            {
                var product = db.products.Where(z => z.type == typeSelected).ToList();
                return View(product.ToPagedList(pageNumber ?? 1, pageSize ?? 6));
            }
            return View();
        }
        // GET: products/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HttpCookie myCookie = new HttpCookie("recentViewed");
            var id2 = Convert.ToString(id);
            // var id2 = Convert.ToChar(id);
           // myCookie = Request.Cookies["recentViewed"];

            if (Request.Cookies["recentViewed"] != null)
            {

                var Isexicit = Request.Cookies["recentViewed"].Value.Split('&').ToList();
                Isexicit.Add(id2);
                myCookie.Value = string.Join("&", Isexicit);
                Response.Cookies.Add(myCookie);
            }
            else
            {
                Response.Cookies["recentViewed"].Value = id2;
            }
            //if (myCookie["zed"] == null)
            //{
            //    myCookie["zed"].ToList().Add(id2);
            //}
            //else
            //{
            //    myCookie["zed"] = string.Join('&', myCookie["zed"].ToList().Add(id2));
            //}
           // myCookie["zed"] = string.Join("&", Isexicit);
           
            //var id2 = Convert.ToString(id);
            //Response.Cookies["product"][id2] = id2;
            //Response.Cookies["product"].Expires = DateTime.Now.AddDays(1);
            //Convert.ToInt32(Response.Cookies["userInfo"]["userName"]) == id;

            //Response.Cookies["userInfo"]["lastVisit"] = DateTime.Now.ToString();
            //Response.Cookies["userInfo"].Expires = DateTime.Now.AddDays(1);
            product product = db.products.Find(id);
            //productImage producimg = new productImage();

            ViewBag.id = db.productImage.Where(m => m.productId == id).FirstOrDefault().Id;
            ViewBag.extension = db.productImage.Where(m => m.productId == id).FirstOrDefault().Extension;

            ViewBag.allpic = db.productImage.Where(m => m.productId == id).Select(s => s.Id);

            if (product == null)
            {
                return HttpNotFound();
            }
            var list = Session["RecentProductList"] as List<RecentProduct>;
            if (list == null)
            {
                // If list not found in session, create list and store it in a session
                list = new List<RecentProduct>();
                Session["RecentProductList"] = list;
            }

            // Add product to recent list (make list contain max 10 items; change if you like)
            AddRecentProduct(list, id.Value, product.name, 10);

            // Store recentProductList to ViewData keyed as "RecentProductList" to use it in a view
            ViewData["RecentProductList"] = list;
            return View(product);
        }

        // GET: products/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var product = new product();
            //var typeenumm = new List<DDL>();
            //foreach (typeenumm lang in Enum.GetValues(typeof(typeenumm)))
            //    typeenumm.Add(new DDL { Value = (int)lang, Text = lang.ToString() });
            //var typeenumm = Models.typeenumm;
            ViewBag.typeenumm = db.Types;

            return View(product);
        }

        // POST: products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(product support)
        {

            if (ModelState.IsValid)
            {
                List<productImage> fileDetails = new List<productImage>();
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        productImage fileDetail = new productImage()
                        {
                            FileName = fileName,
                            Extension = Path.GetExtension(fileName),
                            Id = Guid.NewGuid()
                        };
                        fileDetails.Add(fileDetail);

                        var path = Path.Combine(Server.MapPath("~/saveImages/"), fileDetail.Id + fileDetail.Extension);
                        file.SaveAs(path); // save to folder
                        ResizeSettings resizeSetting = new ResizeSettings
                        {
                            Width = 253,
                            Height = 337,
                            Format = "jpg"
                        };
                        ImageBuilder.Current.Build(path, path, resizeSetting);

                    }
                }
                ViewBag.typeenumm = db.Types;
                support.date = DateTime.Now;
                support.productImage = fileDetails; // save to db
                db.products.Add(support);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(support);
        }

        [Authorize(Roles = "Admin")]
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
            //ViewBag.catgorieID = new SelectList(db.catgoris, "id", "name", product.catgorieID);
            //ViewBag.typeID = new SelectList(db.types, "id", "name", product.typeID);
            return View(product);
        }

        // POST: products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "productID,name,Desciption,price,date,stock,typeID,catgorieID")] product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.catgorieID = new SelectList(db.catgoris, "id", "name", product.catgorieID);
            //ViewBag.typeID = new SelectList(db.types, "id", "name", product.typeID);
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
        public ActionResult FillCity(int type)
        {
            var cat = db.Cats.Where(c => c.TypeId == type);
            return Json(cat, JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public void AddRecentProduct(List<RecentProduct> list, int id, string name, int maxItems)
        {
            // list is current list of RecentProduct objects
            // Check if item already exists

            //cookie.Values["1"] = "id";

            var item = list.FirstOrDefault(t => t.ProductId == id);
            // TODO: here if item is found, you could do some more coding
            //       to move item to the end of the list, since this is the
            //       last product referenced.
            if (item == null)
            {
                // Add item only if it does not exist
                list.Add(new RecentProduct
                {
                    ProductId = id,
                    ProdutName = name,
                    LastVisited = DateTime.Now,
                });
            }

            // Check that recent product list does not exceed maxItems
            // (items at the top of the list are removed on FIFO basis;
            // first in, first out).
            while (list.Count > maxItems)
            {
                list.RemoveAt(0);
            }
        }

        public ActionResult Recentview()
        {
            ViewData["RecentProductList"] = Session["RecentProductList"] as List<RecentProduct>;

            return PartialView("_RecentProduct");
        }

        public ActionResult Showcomment()
        {
            return PartialView(db.CommentAjaxes.Where(c => c.ParentID == null).ToList());
        }

        public ActionResult InsertComment(int parentid = 0)
        {
            if (parentid != 0)
            {
                return PartialView(new CommentAjax()
                {
                    ParentID = parentid
                });
            }
            return PartialView();
        }

        [HttpPost]
        public ActionResult InsertComment(CommentAjax comment)
        {
            comment.Date = DateTime.Now;
            db.CommentAjaxes.Add(comment);
            db.SaveChanges();
            return PartialView("Showcomment", db.CommentAjaxes.Where(c => c.ParentID == null).ToList());
        }
    }
}
