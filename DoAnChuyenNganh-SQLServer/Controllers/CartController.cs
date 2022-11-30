using DoAnChuyenNganh_SQLServer.Models;
using DoAnChuyenNganh_SQLServer.Interface;
using DoAnChuyenNganh_SQLServer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace DoAnChuyenNganh.Controllers
{
    public class CartController : Controller
    {
        ICart cart = new CartService();
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("~/cart/getyourcart")]
        public ActionResult GetYourCart()
        {
            if (Session["Customer"] == null)
                return Json(new { url = Url.Action("Index", "Home") }, JsonRequestBehavior.AllowGet);
            var customer = (Session["Customer"] as Customer).CustomerID;
            var data = cart.GetYourCard(customer);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Route("~/cart/addtocart")]
        public JsonResult AddToCart(Cart item)
        {
            if (Session["Customer"] == null)
            {
                return Json(new { url = Url.Action("Index", "Login") }, JsonRequestBehavior.AllowGet);
            }
            item.CustomerID = (Session["Customer"] as Customer).CustomerID;
            cart.AddCart(item);
            return Json(new {message = "Added to cart"}, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        [Route("~/cart/deletecart")]
        public JsonResult DeleteCart(Cart item)
        {
            if (Session["Customer"] == null)
            {
                return Json(new { url = Url.Action("Index", "Login") }, JsonRequestBehavior.AllowGet);
            }
            item.CustomerID = (Session["Customer"] as Customer).CustomerID;
            cart.DeleteCart(item);
            return Json(new { message = "Deleted from cart" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        [Route("~/cart/updatecart")]
        public JsonResult UpdateCart(List<Cart> item)
        {
            if (Session["Customer"] == null)
            {
                return Json(new { url = Url.Action("Index", "Login") }, JsonRequestBehavior.AllowGet);
            }
            if(item.Count() == 0)
            {
                return Json(new { error = "No item in cart" }, JsonRequestBehavior.AllowGet);
            }
            string CustomerID = (Session["Customer"] as Customer).CustomerID;
            foreach (var i in item)
            {
                i.CustomerID = CustomerID;
            }

            cart.UpdateCart(item, CustomerID);
            return Json(new { message = "Update sussces" });
        }

        [HttpPost]
        [Route("~/cart/applycoupon")]
        public JsonResult ApplyCoupon(string coupon)
        {
            if (Session["Customer"] == null)
            {
                return Json(new { url = Url.Action("Index", "Login") }, JsonRequestBehavior.AllowGet);
            }
            var data = cart.ApplyCoupon(coupon);
            if(data == null)
            {
                return Json(new { error = "The coupon can not use" });
            }
            return Json(new {succes="Apply coupon succes", data= data}, JsonRequestBehavior.AllowGet);
        }



        public ActionResult CheckOut()
        {
            return View();
        }

        [HttpPost]
        [Route("~/cart/checkoutsuccess")]
        public ActionResult CheckOutSuccess(List<OrderDetail> order, string Address, double total, string discount)
        {
            if (Session["Customer"] == null)
            {
                return Json(new { url = Url.Action("Index", "Login") }, JsonRequestBehavior.AllowGet);
            }
            if (order.Count() == 0)
            {
                return Json(new { error = "No item in cart" }, JsonRequestBehavior.AllowGet);
            }
            Order ord = new Order();
            var Customer = (Session["Customer"] as Customer);
            ord.CustomerID = Customer.CustomerID;
            ord.OrderDate = DateTime.Now;
            ord.DiscountID = discount;
            ord.Status = true;
            Invoice invoice = new Invoice();
            invoice.TotalPayment = total;
            cart.CheckOut(order, Address, invoice, ord, Customer);
            return Json(new { url = Url.Action("Index", "Login") }, JsonRequestBehavior.AllowGet);
        }



    }
}