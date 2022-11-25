using DoAnChuyenNganh_SQLServer.Areas.Admin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace DoAnChuyenNganh_SQLServer.Areas.Admin.Controllers
{
    public class DiscountManagementController : Controller
    {
        GearShopAdminDataContext data = new GearShopAdminDataContext();
        // GET: Admin/DiscountManagement
        public ActionResult DiscountManagement()
        {
            if (Session["AdminAccount"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var all_Dis = from tt in data.Discounts select tt;
            return View(all_Dis);
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
        public ActionResult Create(FormCollection collection, Discount a)
        {
            var DiscountID = collection["DiscountID"];
            var DisplayName = collection["DisplayName"];
            var DiscountPersent = collection["DiscountPersent"];
            var NumberOfUse = collection["NumberOfUse"];
            var StartDate = collection["StartDate"];
            var EndDate = collection["EndDate"];
            ViewBag.IDError = CheckID(collection["DiscountID"]);
            if (string.IsNullOrEmpty(DiscountID) || string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(DiscountPersent) || string.IsNullOrEmpty(StartDate) || string.IsNullOrEmpty(EndDate))
            {
                return Json(new { Message = "X Vui lòng nhập đầy đủ thông tin!" });
            }
            if (ViewBag.IDError != null)
            {
                return RedirectToAction("Create");
            }
            a.DiscountID = DiscountID;
            a.DisplayName = DisplayName;
            a.DiscountPersent = Convert.ToInt32(DiscountPersent);
            a.NumberOfUse = Convert.ToInt32(NumberOfUse);
            a.StartDate = Convert.ToDateTime(StartDate);
            a.EndDate = Convert.ToDateTime(EndDate);
            a.CreatedAt = DateTime.Now;
            a.CreatedBy = (Session["AdminAccount"] as Employee).DisplayName;
            a.UpdateAt = DateTime.Now;
            a.UpdateBy = (Session["AdminAccount"] as Employee).DisplayName;
            data.Discounts.InsertOnSubmit(a);
            data.SubmitChanges();
            return RedirectToAction("DiscountManagement");
        }
        public ActionResult Edit(string id)
        {
            if (Session["AdminAccount"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var edit_Dis = data.Discounts.SingleOrDefault(n => n.DiscountID == id);
            return Json(new { edit_Dis = edit_Dis, StartDate = edit_Dis.StartDate.Value.Date.ToString("dd/MM/yyyy"), EndDate = edit_Dis.EndDate.Value.Date.ToString("dd/MM/yyyy") }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Edit_Form(Discount a)
        {
            var p = data.Discounts.Where(t => t.DiscountID == a.DiscountID).FirstOrDefault();
            var DisplayName = a.DisplayName;
            var DiscountPersent = a.DiscountPersent;
            var NumberOfUse =a.NumberOfUse;
            var StartDate = a.StartDate;
            var EndDate = a.EndDate;
            ViewBag.IDError = CheckID(a.DiscountID);
            if (string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(DiscountPersent.ToString()) || string.IsNullOrEmpty(StartDate.ToString()) || string.IsNullOrEmpty(EndDate.ToString()))
            {
                return Json(new { Message = "X Vui lòng nhập đầy đủ thông tin!" });
            }
            p.DisplayName = DisplayName;
            p.DiscountPersent = Convert.ToInt32(DiscountPersent);
            p.NumberOfUse = Convert.ToInt32(NumberOfUse);
            p.StartDate = Convert.ToDateTime(StartDate);
            p.EndDate = Convert.ToDateTime(EndDate);
            p.UpdateAt = DateTime.Now;
            p.UpdateBy = (Session["AdminAccount"] as Employee).DisplayName;
            UpdateModel(p);
            data.SubmitChanges();
            return Json(new { url = Url.Action("DiscountManagement", "DiscountManagement") });
        }
        //checkid
        public ActionResult CheckID(string id)
        {
            var check = data.Discounts.SingleOrDefault(n => n.DiscountID == id);
            if (check != null)
            {
                return Content("<script>alert('Mã giảm giá đã tồn tại!');</script>");
            }
            return null;
        }
    }
    
}