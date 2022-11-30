using DoAnChuyenNganh_SQLServer.Interface;
using DoAnChuyenNganh_SQLServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnChuyenNganh_SQLServer.Service
{
    public class UserService : IUser
    {
        private GearShopDataContext db = new GearShopDataContext();
        public object GetInformation(string id)
        {
            var address = db.CustomerAddresses.Where(s => s.CustomerID == id);
            var data = db.Customers.Where(x => x.CustomerID == id).Select(x => new
            {
                x.CustomerID,
                x.DisplayName,
                x.Email,
                x.Phone,
                Address = address.Select(s => new {s.AddressID, s.AddressDetail})
            }).FirstOrDefault();
            return data;
        }

        public object ratting(FeedBack feb)
        {
            var data = db.FeedBacks.Where(s => s.ProductID == feb.ProductID && s.CustomerID == feb.CustomerID).FirstOrDefault();
            if(data == null)
            {
                db.FeedBacks.InsertOnSubmit(feb);
                db.SubmitChanges();
                return feb;
            }
            else
            {
                data.Content = feb.Content;
                data.Rating = feb.Rating;
                db.SubmitChanges();
                return data;
            }
            
        }
    }
}