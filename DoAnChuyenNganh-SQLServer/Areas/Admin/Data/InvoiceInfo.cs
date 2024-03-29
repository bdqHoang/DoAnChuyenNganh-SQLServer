﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnChuyenNganh_SQLServer.Areas.Admin.Data
{
    public class InvoiceInfo
    {
        public String InvoiceId { get; set; }
        public String CustomerName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public String CreatedBy { get; set; }
        public Double? TotalPayment { get; set; }
        public String OrderID { get; set; }
        public String ProductName { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
    }
}