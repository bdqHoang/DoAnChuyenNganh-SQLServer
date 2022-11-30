using DoAnChuyenNganh_SQLServer.Areas.Admin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnChuyenNganh_SQLServer.Areas.Admin.Controllers
{
    public class CategorizeManagementController : Controller
    {
        GearShopAdminDataContext data = new GearShopAdminDataContext();
        // GET: Admin/CategorizeManagement
        public ActionResult CategorizeManagement(string id)
        {
            if (Session["AdminAccount"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var all_Cat = from tt in data.Categorizes select tt;
            return View(all_Cat);
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
        public ActionResult Create(FormCollection collection, Categorize a)
        {

            var CategorizeID = collection["CategorizeID"];
            var DisplayName = collection["DisplayName"];
            ViewBag.IDError = CheckID(collection["CategorizeID"]);
            if (string.IsNullOrEmpty(CategorizeID) || string.IsNullOrEmpty(DisplayName))
            {
                return Json(new { Message = "X Vui lòng nhập đầy đủ thông tin!" });
            }
            if (ViewBag.IDError != null)
            {
                return Json(new { Message = "X Mã đã tồn tại !" });
            }
            a.CategorizeID = CategorizeID;
            a.DisplayName = DisplayName;
            a.CreatedAt = DateTime.Now;
            a.CreatedBy = (Session["AdminAccount"] as Employee).DisplayName;
            a.UpdateAt = DateTime.Now;
            a.UpdateBy = (Session["AdminAccount"] as Employee).DisplayName;
            a.Status = true;
            data.Categorizes.InsertOnSubmit(a);
            data.SubmitChanges();
            return RedirectToAction("CategorizeManagement");
        }
        public ActionResult Edit(string id)
        {
            if (Session["AdminAccount"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var e = data.Categorizes.Where(t => t.CategorizeID == id).FirstOrDefault();
            return Json(e, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Form_Edit(FormCollection collection, string id)
        {
            var u = data.Categorizes.Where(t => t.CategorizeID == id).FirstOrDefault();
            var DisplayName = collection["DisplayName"];
            if (string.IsNullOrEmpty(DisplayName))
            {
                ModelState.AddModelError(string.Empty, "X Vui lòng nhập đầy đủ thông tin!");
                return View();
            }
            u.DisplayName = DisplayName;
            u.UpdateAt = DateTime.Now;
            u.UpdateBy = (Session["AdminAccount"] as Employee).DisplayName;
            UpdateModel(u);
            data.SubmitChanges();
            return RedirectToAction("CategorizeManagement");
        }
        public ActionResult ActiveCategorize(string id)
        {
            var e = data.Categorizes.Where(t => t.CategorizeID == id).FirstOrDefault();
            if (e.Status == true)
            {
                e.Status = false;
            }
            else
            {
                e.Status = true;
            }
            data.SubmitChanges();
            return RedirectToAction("CategorizeManagement");
        }
        public string CheckID(string categorizeID)
        {
            string error = null;
            if (data.Categorizes.Where(x => x.CategorizeID == categorizeID).Count() > 0)
            {
                error = "× Mã danh mục đã tồn tại!";
            }
            return error;
        }
    }
}