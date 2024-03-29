﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnChuyenNganh_SQLServer.Areas.Admin.Data;
using System.Security.Cryptography;
using System.Text;
using System.Web.WebSockets;
using Microsoft.EntityFrameworkCore;

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
            List<int> ThisMonthProfit = new List<int> {0,0,0,0,0,0,0,0,0,0,0,0};
            List<int> dayProfit = new List<int> {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
            ViewBag.CountCustomer = data.Customers.Count();
            ViewBag.CountEmployee = data.Employees.Count();
            ViewBag.TotalWareHouse = data.WareHouses.Sum(x => x.quantity);
            ViewBag.TodayProfit = data.Invoices.Where(x => x.CreatedAt.Value.Day == DateTime.Now.Day && x.CreatedAt.Value.Month == DateTime.Now.Month && x.CreatedAt.Value.Year == DateTime.Now.Year && x.Status == true).Sum(x => x.TotalPayment);
            // doanh thu theo thang
            var listMonthRevenue = data.Invoices.Where(s => s.CreatedAt.Value.Year == DateTime.Now.Year && s.Status == true).GroupBy(s => s.CreatedAt.Value.Month)
                .Select(s => new
                {
                    month = s.FirstOrDefault().CreatedAt.Value.Month,
                    profit = s.Sum(v => v.TotalPayment)
                });
            foreach(var item in listMonthRevenue)
            {
                ThisMonthProfit[item.month - 1] =(int)item.profit;
            } 
            ViewBag.ThisMonthProfit = ThisMonthProfit;
            ViewBag.ThisMonth = data.Invoices.Include(s => s.Order).ThenInclude(s => s.Customer)
                .Select(s => new InvoiceInfo { 
                    InvoiceId = s.InvoiceID,
                    CustomerName= s.Order.Customer.DisplayName,
                    TotalPayment = s.TotalPayment,
                    CreatedBy = s.CreatedBy
                    
                }).ToList();
            
            ViewBag.ThisYearProfit = data.Invoices.Where(x => x.CreatedAt.Value.Year == DateTime.Now.Year && x.Status == true).Sum(x => x.TotalPayment);
            //doanh thu theo ngay
            var listDaysRevenue = data.Invoices.Where(s => s.CreatedAt.Value.Year == DateTime.Now.Year && s.CreatedAt.Value.Month == DateTime.Now.Month && s.Status == true).GroupBy(s => s.CreatedAt.Value.Day)
                .Select(s => new
                {
                    day = s.FirstOrDefault().CreatedAt.Value.Day,
                    profit = s.Sum(v => v.TotalPayment)
                });
            foreach (var item in listDaysRevenue)
            {
                dayProfit[item.day - 1] = (int)item.profit;
            }
            ViewBag.DayProfit = dayProfit;
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