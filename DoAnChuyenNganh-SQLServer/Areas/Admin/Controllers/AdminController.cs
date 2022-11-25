using DoAnChuyenNganh_SQLServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnChuyenNganh_SQLServer.Areas.Admin.Data;
using System.Security.Cryptography;
using System.Text;

namespace DoAnChuyenNganh_SQLServer.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        GearShopAdminDataContext data = new GearShopAdminDataContext();
        // GET: Admin/Admin
        public ActionResult Index()
        {
            if (Session["AdminAccount"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            /*ViewBag.TodayProfit = GetTodayProfit();*/
            ViewBag.CountCustomer = data.Customers.Count();
            ViewBag.CountEmployee = data.Employees.Count();
            
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["AdminAccount"] == null)
                return View();
            else
                return RedirectToAction("Index", "Admin");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                var username = collection["txtUsername"];
                var password = MD5Hash(Base64Encode(collection["txtPassword"]));
                var adminAccount = data.Employees.FirstOrDefault(x => x.UserName == username && x.Password == password);
                if (adminAccount != null)
                {
                    if (adminAccount.Status == true)
                    {
                        Session["AdminAccount"] = adminAccount;
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        ViewBag.LoginFailed = "× Tài khoản đã bị khóa!";
                    }
                }
                else
                {
                    ViewBag.LoginFailed = "× Sai tài khoản hoặc mật khẩu!";
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult Logout()
        {
            Session["AdminAccount"] = null;
            return RedirectToAction("Login", "Admin");
        }
        static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
/*        public decimal GetTodayProfit()
        {
            decimal todayProfit = 0;
            var today = DateTime.Now;
            var all_Order = from tt in data.Orders select tt;
            foreach (var item in all_Order)
            {
                if (item.OrderDate.Value.Day == today.Day && item.OrderDate.Value.Month == today.Month && item.OrderDate.Value.Year == today.Year)
                {
                    todayProfit += item.Invoices.
                }
            }
            return todayProfit;
        }*/
    }
}