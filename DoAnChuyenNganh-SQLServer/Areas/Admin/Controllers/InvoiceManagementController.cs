using DoAnChuyenNganh_SQLServer.Areas.Admin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return RedirectToAction("Index");
        }

        public ActionResult InvoiceDetail(string id)
        {
            var detailInvoice = data.Invoices.Where(x => x.InvoiceID == id);
            return View(detailInvoice);
        }
    }
}