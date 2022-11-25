using DoAnChuyenNganh_SQLServer.Areas.Admin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DoAnChuyenNganh_SQLServer.Areas.Admin.Controllers
{
    public class UserManagementController : Controller
    {
        // GET: Admin/UserManagement
        GearShopAdminDataContext data = new GearShopAdminDataContext();
        public ActionResult UserManagement()
        {
            if (Session["AdminAccount"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var all_Cus = from tt in data.Customers select tt;
            return View(all_Cus);
        }
        public ActionResult Create()
        {
            if (Session["AdminAccount"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, Customer a)
        {
            var DisplayName = collection["DisplayName"];
            var UserName = collection["UserName"];
            var Password = collection["Password"];
            var Email = collection["Email"];
            var Phone = collection["Phone"];
            var Status = true;
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(Password))
            {
                ModelState.AddModelError(string.Empty, "X Vui lòng nhập đầy đủ thông tin!");
                return View();
            }
            a.CustomerID = CreateCustomerID(11);
            a.DisplayName = DisplayName;
            a.Password = MD5Hash(Base64Encode(Password));
            a.Phone = Phone;
            a.Email = Email;
            a.CreatedAt = DateTime.Now;
            a.UpdateAt = DateTime.Now;
            a.UpdateBy = (Session["AdminAccount"] as Employee).DisplayName;
            a.Status = Status;
            data.Customers.InsertOnSubmit(a);
            data.SubmitChanges();
            return RedirectToAction("UserManagement");
        }
        public ActionResult Edit(string id)
        {
            var e = data.Customers.Where(t => t.CustomerID == id).FirstOrDefault();
            return View(e);
        }
        [HttpPost]
        public ActionResult Edit(FormCollection collection, string id)
        {
            var u = data.Customers.Where(t => t.CustomerID == id).FirstOrDefault();
            var DisplayName = collection["DisplayName"];
            var Password = collection["Password"];
            var Phone = collection["Phone"];
            var Email = collection["Email"];
            var Status = bool.Parse(collection["Status"]);
            if (string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(Password))
            {
                ModelState.AddModelError(string.Empty, "× Vui lòng nhập đầy đủ thông tin!");
                return View(u);
            }
            u.DisplayName = DisplayName;
            u.Password = MD5Hash(Base64Encode(Password));
            u.Phone = Phone;
            u.Email = Email;
            u.UpdateAt = DateTime.Now;
            u.UpdateBy = (Session["AdminAccount"] as Employee).DisplayName;
            u.Status = Status;
            UpdateModel(u);
            data.SubmitChanges();
            return RedirectToAction("UserManagement");
        }
        public ActionResult ActiveUser(string id)
        {
            var e = data.Customers.Where(t => t.CustomerID == id).FirstOrDefault();
            if (e.Status == true)
            {
                e.Status = false;
            }
            else
            {
                e.Status = true;
            }
            data.SubmitChanges();
            return RedirectToAction("UserManagement");
        }
        //Encode password
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
        //auto generate customerid
        private static Random random = new Random();

        public static string CreateCustomerID(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}