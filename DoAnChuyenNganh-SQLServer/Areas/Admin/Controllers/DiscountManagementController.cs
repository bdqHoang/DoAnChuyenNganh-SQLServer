using DoAnChuyenNganh_SQLServer.Areas.Admin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                ModelState.AddModelError(string.Empty, "X Vui lòng nhập đầy đủ thông tin!");
                return RedirectToAction("Create");
            }
            if (ViewBag.IDError != null)
            {
                return RedirectToAction("Create");
            }
            a.DiscountID = DiscountID;
            a.DisplayName = DisplayName;
            a.DiscountPersent = Convert.ToInt32(DiscountPersent);
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
            return Json(edit_Dis, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Edit(FormCollection collection, Discount a)
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
                ModelState.AddModelError(string.Empty, "X Vui lòng nhập đầy đủ thông tin!");
                return RedirectToAction("Edit");
            }
            if (ViewBag.IDError != null)
            {
                return RedirectToAction("Edit");
            }
            a.DiscountID = DiscountID;
            a.DisplayName = DisplayName;
            a.DiscountPersent = Convert.ToInt32(DiscountPersent);
            a.StartDate = Convert.ToDateTime(StartDate);
            a.EndDate = Convert.ToDateTime(EndDate);
            a.UpdateAt = DateTime.Now;
            a.UpdateBy = (Session["AdminAccount"] as Employee).DisplayName;
            UpdateModel(a);
            data.SubmitChanges();
            return RedirectToAction("DiscountManagement");
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