using Online_Store.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Online_Store.Classes;
using Microsoft.Owin.Host.SystemWeb;




namespace Online_Store.Controllers
{
    public class ShoppingCartController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();
       // private int count = 0;
        
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CartSummary()
        {

            if (Session["cart"] == null)
            {
                ViewData["CartCount"] = 0;
            }

            else
            {
                foreach (Item item in (List<Item>) Session["cart"])
                {
                    ViewData["CartCount"] = item.Quantity;
                }
            }
            return PartialView("CartSummary");
        }
        private int isExisting(int id)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Pr.productID == id)
                {
                    //count++;
                    //Session["count"] = count;
                    return i;
                }
               
            }
            return -1;
        }

        public ActionResult Delete(int id)
        {
            int index = isExisting(id);
            List<Item> cart = (List<Item>)Session["cart"];
            cart.RemoveAt(index);
            Session["cart"] = cart;
            return View("Index");
        }

    public ActionResult OrderNow(int id)
    {
        if (Session["cart"] == null)
        {
            List<Item> cart = new List<Item>();
            //int index = isExisting(id);
            //if(index==-1)
            cart.Add(new Item(db.products.Find(id),1));
            Session["cart"] = cart;
            //List<Item>
        }

        else
        {    List<Item> cart = (List<Item>)Session["cart"];
             int index = isExisting(id);
            if (index == -1)
                cart.Add(new Item(db.products.Find(id), 1));
            else
                cart[index].Quantity++;
            Session["cart"] = cart;
        }
        return View("Index"); 
    }

    //public ActionResult CartSummary()
    //{
    //    var cart = ShoppingCart_class.GetCart(this.HttpContext);

    //    ViewData["CartCount"] = cart.GetCount();
    //    return PartialView("CartSummary");
    //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(FormCollection fc)
        {
            string[] quanitites = fc.GetValues("quantity");
            //string[] quantiyForCartSummery = fc.GetValues("quantity");
            //quantiyForCartSummery = Convert.ToInt32(quantiyForCartSummery)
            List<Item> cart = (List<Item>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                cart[i].Quantity = Convert.ToInt32(quanitites[i]);
          
            Session["cart"] = cart;
            return View();

        }
        [HttpGet]
        public ActionResult Proceed()
        {
            var userid = User.Identity.GetUserId();
            var manag = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var UserDetails = manag.FindById(userid);
            ViewBag.UserDetails = UserDetails;
            //ViewBag.UserEmail = UserDetails.Email;
            //ViewBag.UserName = UserDetails.UserName;
            //Session["janeshin"] = total;
          //  ViewBag.UserTotalPurshes = (decimal)Session["janeshin"];
            Order order = new Order();
            if (UserDetails != null )
            {
                order.Username = UserDetails.UserName;
                order.Email = UserDetails.Email;
                order.Total = Convert.ToDecimal(Session["janeshin"]);

            }
            else
            {

                order.Total = Convert.ToDecimal(Session["janeshin"]);

            }
            
            return View(order);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Proceed(Order order)
        {
            var janeshinekhar = Convert.ToDecimal(Session["janeshin"]);
            //ViewBag.UserEmail = UserDetails.Email;
            //ViewBag.UserName = UserDetails.UserName;
            //ViewBag.UserTotalPurshes = total;
            Order order2 = new Order()
            {
                Username = order.Username,
                FirstName = order.FirstName,
                LastName = order.LastName,
                Address = order.Address,
                City = order.City,
                State = order.State,
                PostalCode = order.PostalCode,
                Phone = order.Phone,
                Email = order.Email,
                Total = janeshinekhar,
                OrderDate = DateTime.Now


            };
            
            try
            {
                db.Orders.Add(order2);
                db.SaveChanges();
               
                return RedirectToAction("Delivery",new {id = order2.Id});
            }

            catch(DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                            validationError.PropertyName,
                            validationError.ErrorMessage);
                    }
                }
                return View(order2);
            }

            //return View(order2);

        }

        [HttpGet]
        public ActionResult Delivery(int id)
        {
            DevliveryMethod dv = new DevliveryMethod()
            {
                OrderId = id
            };
        return View(dv);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delivery(DevliveryMethod devliveryMethod)
        {
            //DevliveryMethod del = new DevliveryMethod
            //{
            //   OrderId = devliveryMethod.OrderId,
            //    DeliveryEnum = devliveryMethod.DeliveryEnum
            //};
           /*************************
            * moshkeli k inja dashtim in bud k yeki ro @html.radiobuttonfor karde budim vali baghiash ro vel karde budim be raveshe 
            * <input type="radio"> - baad az tabdile hamashun POST dorost kar kard 
            * ******************
            */
            //db.Orders.Where(Or => Or.Id == devliveryMethod.OrderId).SingleOrDefault().
            //devliveryMethod.DeliveryEnum.ToString();
            var total = db.Orders.Where(t => t.Id == devliveryMethod.OrderId).FirstOrDefault();
             decimal delivery;
            if (devliveryMethod.DeliveryEnum == Models.Delivery.post)
            {
                delivery = 500.00m;
                total.Total += delivery;
                //db.SaveChanges();
                Session["delivery"] = delivery;
            }
            if (devliveryMethod.DeliveryEnum == Models.Delivery.pishtaz)
            {
                delivery = (decimal)1000.00m;
                total.Total += delivery;
                //db.SaveChanges();
                Session["delivery"] = delivery;
            }
            if (devliveryMethod.DeliveryEnum == Models.Delivery.peyk)
            {
                delivery = 2000.00m;
                total.Total += delivery;
                //db.SaveChanges();
                Session["delivery"] = delivery;
            }

            //var currentCitizen = db.Orders.Find(devliveryMethod.OrderId);
            //db.Entry(currentCitizen).CurrentValues.SetValues(total);
            //db.SaveChanges();

            //db.Orders.Attach(Order);
            db.Entry(total).State = EntityState.Modified;
            db.SaveChanges();

            db.DeliverMethods.Add(devliveryMethod);
            db.SaveChanges();
            return RedirectToAction("PaymentMethod", new { id = devliveryMethod.OrderId });
        }
        [HttpGet]
        public ActionResult PaymentMethod(int id)
        {
            var total = db.Orders.Where(t => t.Id == id).SingleOrDefault().Total;
            ViewBag.total = total;
            PaymentMethod py = new PaymentMethod()
            {
                OrderId = id
            };
            return View(py);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentMethod(PaymentMethod paymentMethod)
        {

            db.PaymentMethod.Add(paymentMethod);
            db.SaveChanges();
            return View();
        }
        //[HttpGet]
        //public ActionResult OrderReview(int id)
        //{
        //    db.PaymentMethod.Add(paymentMethod);
        //    db.SaveChanges();
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult OrderReview(int id)
        //{
        //    db.PaymentMethod.Add(paymentMethod);
        //    db.SaveChanges();
        //    return View();
        //}

        //public ActionResult Success()
        //{
        //    System.Web.UI.WebControls.View.result = PDTHolder.Success(Request.QueryString.Get("tx"));
        //    return View("Success");
        //}
    }
}