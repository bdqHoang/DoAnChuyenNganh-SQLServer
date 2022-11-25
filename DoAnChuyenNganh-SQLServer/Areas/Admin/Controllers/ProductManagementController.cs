using DoAnChuyenNganh_SQLServer.Areas.Admin.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnChuyenNganh_SQLServer.Areas.Admin.Controllers
{
    public class ProductManagementController : Controller
    {
        GearShopAdminDataContext data = new GearShopAdminDataContext();
        // GET: Admin/ProductManagement
        public ActionResult ProductManagement()
        {
            if (Session["AdminAccount"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.Categorize = new SelectList(data.Categorizes, "CategorizeID", "DisplayName");
            ViewBag.Supplier = new SelectList(data.Suppliers, "SupplierID", "DisplayName");
            var all_Pro = from tt in data.Products select tt;
            return View(all_Pro);
        }
        public ActionResult Create()
        {
            if (Session["AdminAccount"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.Categorize = new SelectList(data.Categorizes, "CategorizeID", "DisplayName");
            ViewBag.Supplier = new SelectList(data.Suppliers, "SupplierID", "DisplayName");
            return View();
        }
        [HttpPost]
        public ActionResult Create(Product s, HttpPostedFileBase upFile, FormCollection collection)
        {
            if (Session["AdminAccount"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.IDError = CheckID(collection["ProductID"]);
            var ProductID = collection["ProductID"];
            var CategorizeID = string.IsNullOrEmpty(collection["CategorizeID"]) ? "KB01" : collection["CategorizeID"];
            var SupplierID = collection["SupplierID"];
            var DisplayName = collection["DisplayName"];
            var ImportPrice = decimal.Parse(string.IsNullOrEmpty(collection["ImportPrice"]) ? "0" : collection["ImportPrice"]);
            var SellPrice = decimal.Parse(string.IsNullOrEmpty(collection["SellPrice"]) ? "0" : collection["SellPrice"]);
            var Description = collection["Description"];
            var Warranty = int.Parse(string.IsNullOrEmpty(collection["Warranty"]) ? "0" : collection["Warranty"]);
            var Weight = double.Parse(collection["Weight"]);
            var Material = collection["Material"];
            ViewBag.Categorize = new SelectList(data.Categorizes, "CategorizeID", "DisplayName");
            ViewBag.Supplier = new SelectList(data.Suppliers, "SupplierID", "DisplayName");
            if (string.IsNullOrEmpty(ProductID) || string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(Weight.ToString()))
            {
                ViewData["Error"] = "x Vui lòng nhập đầy đủ thông tin!";
            }
            if (ViewBag.IDError != null)
            {
                return View();
            }
            s.ProductID = ProductID;
            s.DisplayName = DisplayName;
            s.SupplierID = SupplierID == null ? null : SupplierID;
            s.CategorizeID = CategorizeID;
            s.ImportPrice = ImportPrice;
            s.SellPrice = SellPrice;
            s.Description = Description == null ? null : SupplierID;
            s.Warranty = Warranty;
            s.Material = Material;
            s.Weight = Weight;
            s.CreatedAt = DateTime.Now;
            s.UpdateAt = DateTime.Now;
            s.UpdateBy = (Session["AdminAccount"] as Employee).DisplayName;
            s.CreatedBy = (Session["AdminAccount"] as Employee).DisplayName;
            s.Status = true;
            data.Products.InsertOnSubmit(s);
            data.SubmitChanges();
            return RedirectToAction("ProductManagement");
        }
        public ActionResult Edit(string id)
        {
            if (Session["AdminAccount"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var e = data.Products.Where(t => t.ProductID == id).FirstOrDefault();
            ViewBag.Categorize = new SelectList(data.Categorizes, "CategorizeID", "DisplayName");
            ViewBag.Supplier = new SelectList(data.Suppliers, "SupplierID", "DisplayName");
            return View(e);
        }
        [HttpPost]
        public ActionResult Edit(FormCollection collection, string id)
        {

            var u = data.Products.Where(t => t.ProductID == id).FirstOrDefault();
            ViewBag.Categorize = new SelectList(data.Categorizes, "CategorizeID", "DisplayName");
            ViewBag.Supplier = new SelectList(data.Suppliers, "SupplierID", "DisplayName");
            var CategorizeID = collection["CategorizeID"];
            var SupplierID = collection["SupplierID"];
            var DisplayName = collection["DisplayName"];
            var ImportPrice = decimal.Parse(string.IsNullOrEmpty(collection["ImportPrice"]) ? "0" : collection["ImportPrice"]);
            var SellPrice = decimal.Parse(string.IsNullOrEmpty(collection["SellPrice"]) ? "0" : collection["SellPrice"]);
            var Description = collection["Description"];
            var Warranty = int.Parse(string.IsNullOrEmpty(collection["Warranty"]) ? "0" : collection["Warranty"]);
            var Weight = double.Parse(string.IsNullOrEmpty(collection["Weight"]) ? "0" : collection["Weight"]);
            var Material = collection["Material"];
            if (string.IsNullOrEmpty(DisplayName))
            {
                ModelState.AddModelError(string.Empty, "X Vui lòng nhập đầy đủ thông tin!");
                return View();
            }
            u.DisplayName = DisplayName;
            u.SupplierID = SupplierID;
            u.CategorizeID = CategorizeID;
            u.ImportPrice = ImportPrice;
            u.SellPrice = SellPrice;
            u.Description = Description;
            u.Warranty = Warranty;
            u.Material = Material;
            u.Weight = Weight;
            u.UpdateAt = DateTime.Now;
            u.UpdateBy = (Session["AdminAccount"] as Employee).DisplayName;
            u.Status = true;
            UpdateModel(u);
            data.SubmitChanges();
            return RedirectToAction("ProductManagement");
        }
        public ActionResult ActiveProduct(string id)
        {
            var e = data.Products.Where(t => t.ProductID == id).FirstOrDefault();
            if (e.Status == true)
            {
                e.Status = false;
            }
            else
            {
                e.Status = true;
            }
            data.SubmitChanges();
            return RedirectToAction("ProductManagement");
        }
        public byte[] converImagetoByte(string url)
        {
            var path = Server.MapPath(url);
            FileStream fs;
            fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] picbyte = new byte[fs.Length];
            fs.Read(picbyte, 0, System.Convert.ToInt32(fs.Length));
            fs.Close();
            return picbyte;
        }
        public string CheckID(string ProductID)
        {
            string error = null;
            if (data.Products.Where(x => x.ProductID == ProductID).Count() > 0)
            {
                error = "× Mã sản phẩm đã tồn tại!";
            }
            return error;
        }
        
    }
}