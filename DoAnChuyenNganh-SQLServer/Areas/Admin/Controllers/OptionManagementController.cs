using DoAnChuyenNganh_SQLServer.Areas.Admin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnChuyenNganh_SQLServer.Areas.Admin.Controllers
{
    public class OptionManagementController : Controller
    {
        GearShopAdminDataContext data = new GearShopAdminDataContext();
        // GET: Admin/OptionManagement
        public ActionResult OptionManagement()
        {
            if (Session["AdminAccount"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var option = from tt in data.Options select tt;
            return View(option);
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
        public ActionResult Create(FormCollection collection, Option a)
        {
            var OptionID = collection["OptionID"];
            var DisplayName = collection["DisplayName"];
            ViewBag.IDError = CheckID(collection["OptionID"]);
            if (string.IsNullOrEmpty(OptionID) || string.IsNullOrEmpty(DisplayName))
            {
                ModelState.AddModelError(string.Empty, "X Vui lòng nhập đầy đủ thông tin!");
                return RedirectToAction("Create");
            }
            if (ViewBag.IDError != null)
            {
                return RedirectToAction("Create");
            }
            a.OptionID = OptionID;
            a.DisplayName = DisplayName;
            a.CreatedAt = DateTime.Now;
            a.CreatedBy = (Session["AdminAccount"] as Employee).DisplayName;
            a.UpdateAt = DateTime.Now;
            a.UpdateBy = (Session["AdminAccount"] as Employee).DisplayName;
            a.Status = true;
            data.Options.InsertOnSubmit(a);
            data.SubmitChanges();
            return RedirectToAction("OptionManagement");
        }
        public ActionResult Edit(string id)
        {
            if (Session["AdminAccount"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var option = data.Options.Where(n => n.OptionID == id).Select(s => new { s.OptionID, s.DisplayName}).FirstOrDefault();
            return Json(option, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Form_Edit(FormCollection collection, String id)
        {
            var u = data.Options.Where(t => t.OptionID == id).FirstOrDefault();
            var DisplayName = collection["DisplayName"];
            ViewBag.IDError = CheckID(collection["OptionID"]);
            if ( string.IsNullOrEmpty(DisplayName))
            {
                ModelState.AddModelError(string.Empty, "X Vui lòng nhập đầy đủ thông tin!");
                return RedirectToAction("Edit");
            }
            if (ViewBag.IDError != null)
            {
                return RedirectToAction("Edit");
            }
            u.DisplayName = DisplayName;
            u.UpdateAt = DateTime.Now;
            u.UpdateBy = (Session["AdminAccount"] as Employee).DisplayName;
            u.Status = true;
            UpdateModel(u);
            data.SubmitChanges();
            return RedirectToAction("OptionManagement");
        }
        //checkid
        public ActionResult CheckID(string id)
        {
            var option = data.Options.SingleOrDefault(n => n.OptionID == id);
            if (option != null)
            {
                return Content("<p style='color:red'>X ID đã tồn tại!</p>");
            }
            return null;
        }
    }
       
}