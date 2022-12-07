using DoAnChuyenNganh_SQLServer.Areas.Admin.Data;
using Microsoft.Ajax.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DoAnChuyenNganh_SQLServer.Areas.Admin.Controllers
{
    public class InvoiceManagementController : Controller
    {
        GearShopAdminDataContext data = new GearShopAdminDataContext();
        // GET: Admin/InvoiceManagement
        public ActionResult InvoiceManagement()
        {
            if (Session["AdminAccount"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var invoice = from ss in data.Invoices select ss;
            return View(invoice);
        }
        public ActionResult UpdateInvoice(string id)
        {
            var invoice = data.Invoices.Where(x => x.InvoiceID == id).First();
            if (invoice.Status == true) invoice.Status = false;
            else invoice.Status = true;
            invoice.UpdateAt = DateTime.Now;
            invoice.UpdateBy = (Session["AdminAccount"] as Employee).DisplayName;
            UpdateModel(invoice);
            data.SubmitChanges();
            return RedirectToAction("InvoiceManagement/InvoiceManagement");
        }
        [HttpGet]
        public JsonResult InvoiceDetail(string id)
        {
            var invoiceDetail = data.Invoices.Include(i => i.Order).ThenInclude(i => i.Customer).Include(s => s.Order).ThenInclude(s => s.OrderDetails).ThenInclude(s => s.WareHouse).ThenInclude(s => s.Product).Select(s => new InvoiceInfo
            {

                InvoiceId = s.InvoiceID,
                CustomerName = s.Order.Customer.DisplayName,
                CreatedAt = s.CreatedAt,
                CreatedBy = s.CreatedBy,
                TotalPayment = s.TotalPayment,
                OrderID = s.OrderID,
                ProductName = s.Order.OrderDetails.First().WareHouse.Product.DisplayName,
                Quantity = s.Order.OrderDetails.First().Quantity,
                Price = s.Order.OrderDetails.First().WareHouse.Product.SellPrice
            }).ToList();
            return Json(invoiceDetail, JsonRequestBehavior.AllowGet);
        }
    }
}