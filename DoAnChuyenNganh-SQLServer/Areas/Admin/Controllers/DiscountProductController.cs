using DoAnChuyenNganh_SQLServer.Areas.Admin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnChuyenNganh_SQLServer.Areas.Admin.Controllers
{
    public class DiscountProductController : Controller
    {
        GearShopAdminDataContext data = new GearShopAdminDataContext();
        // GET: Admin/DiscountProduct
        public ActionResult DiscountProduct()
        {
            if (Session["AdminAccount"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var all_Prodis = from tt in data.ProductDiscounts select tt;
            return View(all_Prodis);
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
        public ActionResult Create(FormCollection collection, ProductDiscount a)
        {
            var ProductDiscountID = collection["ProductDiscountID"];
            var DisplayName = collection["DisplayName"];
            var DiscountPercent = collection["DiscountPercent"];
            var StartDate = collection["StartDate"];
            var EndDate = collection["EndDate"];
            ViewBag.IDError = CheckID(collection["ProductDiscountID"]);
            if (string.IsNullOrEmpty(ProductDiscountID) || string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(DiscountPercent) || string.IsNullOrEmpty(StartDate) || string.IsNullOrEmpty(EndDate))
            {
                ModelState.AddModelError(string.Empty, "X Vui lòng nhập đầy đủ thông tin!");
                return RedirectToAction("Create");
            }
            if (ViewBag.IDError != null)
            {
                return RedirectToAction("Create");
            }
            a.ProductDiscountID = ProductDiscountID;
            a.DisplayName = DisplayName;
            a.DiscountPercent = Convert.ToInt32(DiscountPercent);
            a.StartDate = Convert.ToDateTime(StartDate);
            a.EndDate = Convert.ToDateTime(EndDate);
            a.CreatedAt = DateTime.Now;
            a.CreatedBy = (Session["AdminAccount"] as Employee).DisplayName;
            a.UpdateAt = DateTime.Now;
            a.UpdateBy = (Session["AdminAccount"] as Employee).DisplayName;
            data.ProductDiscounts.InsertOnSubmit(a);
            data.SubmitChanges();
            return RedirectToAction("DiscountProduct");
        }
        public ActionResult Edit(string id)
        {
            if (Session["AdminAccount"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var edit_Dis = data.ProductDiscounts.SingleOrDefault(n => n.ProductDiscountID == id);
            return Json(new { edit_Dis = edit_Dis, StartDate = edit_Dis.StartDate.Value.Date.ToString("dd/MM/yyyy"), EndDate = edit_Dis.EndDate.Value.Date.ToString("dd/MM/yyyy") }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Edit_Form(ProductDiscount a)
        {
            var p = data.ProductDiscounts.Where(t => t.ProductDiscountID == a.ProductDiscountID).FirstOrDefault();
            var DisplayName = a.DisplayName;
            var DiscountPercent = a.DiscountPercent;
            var StartDate = a.StartDate;
            var EndDate = a.EndDate;
            ViewBag.IDError = CheckID(a.ProductDiscountID);
            if (string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(DiscountPercent.ToString()) || string.IsNullOrEmpty(StartDate.ToString()) || string.IsNullOrEmpty(EndDate.ToString()))
            {
                return Json(new { Message = "X Vui lòng nhập đầy đủ thông tin!" });
            }
            p.DisplayName = DisplayName;
            p.DiscountPercent = Convert.ToInt32(DiscountPercent);
            p.StartDate = Convert.ToDateTime(StartDate);
            p.EndDate = Convert.ToDateTime(EndDate);
            p.UpdateAt = DateTime.Now;
            p.UpdateBy = (Session["AdminAccount"] as Employee).DisplayName;
            UpdateModel(p);
            data.SubmitChanges();
            return Json(new { url = Url.Action("DiscountProduct", "DiscountProduct") });
        }
        //checkid
        public ActionResult CheckID(string id)
        {
            var check = data.ProductDiscounts.SingleOrDefault(n => n.ProductDiscountID == id);
            if (check != null)
            {
                return Content("<script>alert('Mã giảm giá đã tồn tại!');</script>");
            }
            return null;
        }
    }
}