using DoAnChuyenNganh_SQLServer.Models;
using DoAnChuyenNganh_SQLServer.Interface;
using DoAnChuyenNganh_SQLServer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace DoAnChuyenNganh_SQLServer.Views.Home
{
    public class LoginController : Controller
    {
        ILogin login = new LoginService();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// login
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("~/login/login")]
        public ActionResult Login(string email, string password)
        {            
            if (!string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(password))
            {
                var data = login.Login(email, password);
                if (data == null)
                {
                    return Json(new { erorr = "The email or password is incorrect" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Session["Customer"] = data;
                    return Json(new { success = Url.Action("Index", "Home") }, JsonRequestBehavior.AllowGet);
                }
            }

            return View();
        }
        [HttpDelete]
        [Route("~/login/logout")]
        public ActionResult LogOut()
        {
            Session["Customer"] = null;
            return Json(new { url = Url.Action("Index", "Login") }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [Route("~/login/signupform")]
        public JsonResult SignUpForm(Customer cu)
        {
            var data = login.SignUp(cu);
            if (data)
            {
                return Json(new { success = "Sign up commplete", url = Url.Action("Index")});
            }
            else
            {
                return Json(new {erorr = "An erorr"});
            }
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }


        private static Random random = new Random();

        public static string genaratePass(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}