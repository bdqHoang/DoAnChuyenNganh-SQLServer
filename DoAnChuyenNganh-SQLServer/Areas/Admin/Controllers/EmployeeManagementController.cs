using DoAnChuyenNganh_SQLServer.Areas.Admin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DoAnChuyenNganh_SQLServer.Areas.Admin.Controllers
{
    public class EmployeeManagementController : Controller
    {
        GearShopAdminDataContext data = new GearShopAdminDataContext();
        // GET: Admin/EmployeeManagement
        public ActionResult EmployeeManagement()
        {
            if (Session["AdminAccount"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.Position = new SelectList(data.Positions, "PositionID", "DisplayName");
            var all_Emp = from tt in data.Employees select tt;
            return View(all_Emp);
        }
        public ActionResult Create()
        {
            if (Session["AdminAccount"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.Position = new SelectList(data.Positions, "PositionID", "DisplayName");
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, Employee a)
        {
            var DisplayName = collection["DisplayName"];
            var UserName = collection["UserName"];
            var Password = collection["Password"];
            var Phone = collection["Phone"];
            var Email = collection["Email"];
            var Address = collection["Address"];
            var Position = string.IsNullOrEmpty(collection["PositionID"]) ? "2" : collection["PositionID"];
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(Password))
            {
                ModelState.AddModelError(string.Empty, "X Vui lòng nhập đầy đủ thông tin!");
                return View();
            }
            a.EmployeeID = CreateEmployeeID(11);
            a.PositionID = Position;
            a.DisplayName = DisplayName;
            a.UserName = UserName;
            a.Password = MD5Hash(Base64Encode(Password));
            a.Phone = Phone;
            a.Email = Email;
            a.Address = Address;
            a.CreatedAt = DateTime.Now;
            a.CreatedBy = (Session["AdminAccount"] as Employee).DisplayName;
            a.UpdateAt = DateTime.Now;
            a.UpdateBy = (Session["AdminAccount"] as Employee).DisplayName;
            a.Status = true;
            data.Employees.InsertOnSubmit(a);
            data.SubmitChanges();
            return RedirectToAction("EmployeeManagement");
        }
        public ActionResult Edit(string id)
        {
            if (Session["AdminAccount"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var e = data.Employees.Where(t => t.EmployeeID == id).FirstOrDefault();
            ViewBag.Position = new SelectList(data.Positions, "PositionID", "DisplayName");
            return View(e);
        }
        [HttpPost]
        public ActionResult Edit(FormCollection collection, string id)
        {
            var u = data.Employees.Where(t => t.EmployeeID == id).FirstOrDefault();
            var DisplayName = collection["DisplayName"];
            var Position = string.IsNullOrEmpty(collection["PositionID"]) ? "2" : collection["PositionID"];
            var Username = collection["Username"];
            var Password = collection["Password"];
            var Phone = collection["Phone"];
            var Email = collection["Email"];
            var Address = collection["Address"];
            ViewBag.Position = new SelectList(data.Positions, "PositionID", "DisplayName");
            if (string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(Username))
            {
                ModelState.AddModelError(string.Empty, "× Vui lòng nhập đầy đủ thông tin!");
                return View(u);
            }
            u.DisplayName = DisplayName;
            u.PositionID = Position;
            u.UserName = Username;
            u.Phone = Phone;
            u.Email = Email;
            UpdateModel(u);
            data.SubmitChanges();
            return RedirectToAction("EmployeeManagement");
        }
        public ActionResult ActiveUser(string id)
        {
            var e = data.Employees.Where(t => t.EmployeeID.Equals(id)).FirstOrDefault();
            if (e.Status == true)
            {
                e.Status = false;
            }
            else
            {
                e.Status = true;
            }
            data.SubmitChanges();
            return RedirectToAction("EmployeeManagement");
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            if (Session["AdminAccount"] != null)
                return View();
            else
                return RedirectToAction("Login", "Admin", "");
        }
        [HttpPost]
        public ActionResult ChangePassword(FormCollection collection)
        {
            var userAccount = data.Employees.FirstOrDefault(x => x.EmployeeID == (Session["AdminAccount"] as Employee).EmployeeID);
            if (userAccount == null)
            {
                ModelState.AddModelError(string.Empty, "× Lỗi không tìm thấy tài khoản!");
                return View();
            }
            bool checkPassword = (userAccount.Password == MD5Hash(Base64Encode(collection["OldPassword"])));
            if (!checkPassword)
            {
                ViewBag.PasswordError = "× Mật khẩu cũ không đúng!";
                return View();
            }
            ViewBag.PasswordError = CheckPassword(collection["Password"]);
            ViewBag.ComparePasswordError = collection["Password"] != collection["ComparePassword"] ? "× Mật khẩu mới nhập lại không khớp!" : null;
            if (ViewBag.PasswordError != null || ViewBag.ComparePasswordError != null)
            {
                return View();
            }
            userAccount.Password = MD5Hash(Base64Encode(collection["Password"]));
            data.SubmitChanges();
            ViewBag.UpdatedSuccess = "Thay đổi mật khẩu thành công!";
            return View();
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
        private static Random random = new Random();

        public static string CreateEmployeeID(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public string CheckPassword(string password)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
            string error = null;
            if (!hasMinimum8Chars.IsMatch(password))
            {
                error = "× Mật khẩu phải chứa ít nhất 8 ký tự!";
                return error;
            }
            else if (!hasLowerChar.IsMatch(password))
            {
                error = "× Mật khẩu phải chứa ít nhất một chữ cái thường!";
                return error;
            }
            else if (!hasUpperChar.IsMatch(password))
            {
                error = "× Mật khẩu phải chứa ít nhất một chữ cái hoa!";
                return error;
            }
            else if (!hasNumber.IsMatch(password))
            {
                error = "× Mật khẩu phải chứa ít nhất một chữ số!";
                return error;
            }

            else if (!hasSymbols.IsMatch(password))
            {
                error = "× Mật khẩu phải chứa ít nhất một ký tự đặc biệt!";
                return error;
            }
            else
            {
                return error;
            }
        }
    }
}