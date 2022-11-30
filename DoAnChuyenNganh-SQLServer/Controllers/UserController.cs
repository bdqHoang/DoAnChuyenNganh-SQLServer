using DoAnChuyenNganh_SQLServer.Interface;
using DoAnChuyenNganh_SQLServer.Models;
using DoAnChuyenNganh_SQLServer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace DoAnChuyenNganh.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        IUser user = new UserService();
        LoginService login = new LoginService();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("~/user/information")]
        public ActionResult Information()
        {
            if(Session["Customer"] == null)
            {
                return Json(new { url = Url.Action("Index", "Login") }, JsonRequestBehavior.AllowGet);
            }
            string customer = (Session["Customer"] as Customer).CustomerID;
            var data = user.GetInformation(customer);
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        //forgot password
        [HttpGet]
        [Route("~/user/forgotpassword")]
        public JsonResult ForgotPassword(string email)
        {
            var data = login.SentPassword(email);
            if (data)
            {
                return Json(new { success = "The password has been sent to your email", url = Url.Action("Index", "Login") }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { error = "Can not send password" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("~/user/uploadfeedback")]
        public JsonResult UploadFeedback(FeedBack feed)
        {
            feed.CustomerID = (Session["Customer"] as Customer).CustomerID;
            var data = user.ratting(feed);
            if (data == feed)
            {
                return Json(new { success = "Thank you for your feedback" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { error = "The feedback updated" }, JsonRequestBehavior.AllowGet);
        }
    }
}