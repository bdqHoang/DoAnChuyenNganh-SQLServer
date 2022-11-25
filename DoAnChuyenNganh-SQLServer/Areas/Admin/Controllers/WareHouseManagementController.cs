using DoAnChuyenNganh_SQLServer.Areas.Admin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnChuyenNganh_SQLServer.Areas.Admin.Controllers
{
    public class WareHouseManagementController : Controller
    {
        GearShopAdminDataContext data = new GearShopAdminDataContext();
        // GET: Admin/WareHouseManagement
        public ActionResult WareHouseManagement()
        {
            if (Session["AdminAccount"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var wareHouse = from tt in data.WareHouses select tt;
            return View(wareHouse);
        }
        public ActionResult Create()
        {
            if (Session["AdminAccount"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.Product = new SelectList(data.Products.Where(x => x.Status == true), "ProductID", "DisplayName");
            ViewBag.Color = new SelectList(data.Colors, "ColorID", "DisplayName");
            ViewBag.Option = new SelectList(data.Options, "OptionID", "DisplayName");
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, WareHouse a)
        {
            var ProductID = collection["ProductID"];
            var ColorID = collection["ColorID"];
            var OptionID = collection["OptionID"];
            var Quantity = int.Parse(collection["Quantity"]);
            ViewBag.IDError = CheckID(ProductID, ColorID, OptionID);
            ViewBag.Product = new SelectList(data.Products, "ProductID", "DisplayName");
            ViewBag.Color = new SelectList(data.Colors, "ColorID", "DisplayName");
            ViewBag.Option = new SelectList(data.Options, "OptionID", "DisplayName");
            if (string.IsNullOrEmpty(Quantity.ToString()))
            {
                ModelState.AddModelError(string.Empty, "X Vui lòng nhập đầy đủ thông tin!");
                return View();
            }
            if (ViewBag.IDError != null)
            {
                return View();
            }
            a.ProductID = ProductID;
            a.ColorID = ColorID;
            a.OptionID = OptionID;
            a.quantity = Quantity;
            data.WareHouses.InsertOnSubmit(a);
            data.SubmitChanges();
            return RedirectToAction("WareHouseManagement");
        }
        public ActionResult Edit(string id, string colorID, string optionID)
        {
            if (Session["AdminAccount"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var e = data.WareHouses.Where(t => t.ProductID == id && t.ColorID == colorID && t.OptionID == optionID).FirstOrDefault();
            return View(e);
        }
        [HttpPost]
        public ActionResult Edit(FormCollection collection, string id, string colorID, string optionID)
        {
            var u = data.WareHouses.Where(t => t.ProductID == id && t.ColorID == colorID && t.OptionID == optionID).FirstOrDefault();
            var Quantity = int.Parse(collection["Quantity"]);
            if (string.IsNullOrEmpty(Quantity.ToString()))
            {
                ModelState.AddModelError(string.Empty, "X Vui lòng nhập đầy đủ thông tin!");
                return View();
            }
            u.quantity = Quantity;
            UpdateModel(u);
            data.SubmitChanges();
            return RedirectToAction("SwitchManagement");
        }
        public string CheckID(string ProductID, string ColorID, string OptionID)
        {
            string error = null;
            if (data.WareHouses.Where(x => x.ProductID == ProductID).Count() > 0 && data.WareHouses.Where(x => x.ColorID == ColorID).Count() > 0 && data.WareHouses.Where(x => x.OptionID == OptionID).Count() > 0)
            {
                error = "× Sản phẩm đã có trong kho!";
            }
            return error;
        }
    }
}