using DoAnChuyenNganh_SQLServer.Areas.Admin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnChuyenNganh_SQLServer.Areas.Admin.Controllers
{
    public class ProductImageController : Controller
    {
        GearShopAdminDataContext data = new GearShopAdminDataContext();
        // GET: Admin/ProductImage
        public ActionResult ProductImage()
        {
            if (Session["AdminAccount"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var all_Pro = from tt in data.Images select tt;
            return View(all_Pro);
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
        public ActionResult Create(FormCollection collection, Image a)
        {
            var ImageID = collection["ImageID"];
            var ProductID = collection["ProductID"];
            var ImagePath = collection["ImagePath"];
            if (string.IsNullOrEmpty(ImageID) || string.IsNullOrEmpty(ProductID) || string.IsNullOrEmpty(ImagePath))
            {
                return Json(new { Message = "X Vui lòng nhập đầy đủ thông tin!" });
            }
            if (ViewBag.IDError != null)
            {
                return Json(new { Message = "X Mã đã tồn tại !" });
            }
            a.ImageID = ImageID;
            a.ProductID = ProductID;
            //a.ImagePath = ImagePath;
            a.CreatedAt = DateTime.Now;
            a.CreatedBy = (Session["AdminAccount"] as Employee).DisplayName;
            a.UpdateAt = DateTime.Now;
            a.UpdateBy = (Session["AdminAccount"] as Employee).DisplayName;
            data.Images.InsertOnSubmit(a);
            data.SubmitChanges();
            return RedirectToAction("ProductImage");
        }
        public ActionResult Edit(string id)
        {
            if (Session["AdminAccount"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var edit_Pro = data.Images.SingleOrDefault(n => n.ImageID == id);
            return View(edit_Pro);
        }
        [HttpPost]
        public ActionResult Edit(FormCollection collection, Image a)
        {
            var ImageID = collection["ImageID"];
            var ProductID = collection["ProductID"];
            var ImagePath = collection["ImagePath"];
            if (string.IsNullOrEmpty(ImageID) || string.IsNullOrEmpty(ProductID) || string.IsNullOrEmpty(ImagePath))
            {
                return Json(new { Message = "X Vui lòng nhập đầy đủ thông tin!" });
            }
            a.ImageID = CreateID(11);
            a.ProductID = ProductID;
            //a.Image1 = ImagePath;
            a.UpdateAt = DateTime.Now;
            a.UpdateBy = (Session["AdminAccount"] as Employee).DisplayName;
            UpdateModel(a);
            data.SubmitChanges();
            return View();
        }
        private static Random random = new Random();

        public static string CreateID(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}