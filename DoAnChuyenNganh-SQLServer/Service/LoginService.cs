using DoAnChuyenNganh_SQLServer.Models;
using DoAnChuyenNganh_SQLServer.Interface;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DoAnChuyenNganh_SQLServer.Service
{
    public class LoginService : ILogin
    {
        private readonly GearShopDataContext db = new GearShopDataContext();
        public Customer Login(string email, string password)
        {
            password = MD5Hash(Base64Encode(password));
            var user = db.Customers.Where(x => x.Email == email && x.Password == password && x.Status == true).FirstOrDefault();
            if(user == null)
            {
                return null;
            }
            return user;
        }

        public bool SignUp(Customer cu)
        {
            try
            {
                if(db.Customers.Where(s=>s.Email== cu.Email).FirstOrDefault() == null)
                {
                    cu.CustomerID = CreateCustomerID(11);
                    cu.CreatedAt = DateTime.Now;
                    cu.UpdateAt = DateTime.Now;
                    cu.Status = true;
                    cu.Password = MD5Hash(Base64Encode(cu.Password));
                    db.Customers.InsertOnSubmit(cu);
                    db.SubmitChanges();
                    return true;
                }
                return false;
            }catch(Exception ex)
            {
                return false;
            }
        }









        //encrypt password
        //Encode password
        static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
        //auto generate customerid
        private static Random random = new Random();

        public static string CreateCustomerID(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public bool SentPassword(string email)
        {
            var data = db.Customers.Where(x => x.Email == email).FirstOrDefault();
            if(data == null)
            {
                return false;
            }
            else
            {
                string emailTo = email;
                string subject = "PHỤC HỒI MẬT KHẨU - TEAM-3TL";
                string newPassword = CreateCustomerID(10);
                string content = "Mật khẩu mới của bạn là: " + newPassword;
                string body = string.Format("Bạn vừa nhận được liên hệ từ: <b>TEAM-3TL</b><br/>Email: {0}<br/>Nội dung:<br/>{1}<br/>Mọi thắc mắc xin liên hệ qua link facebook: <a href= \" https://www.facebook.com/groups/4959672280730594 \">https://www.facebook.com/groups/4959672280730594</a><br/>", "Team3TL@outlook.com", content);
                bool result = EmailService.Send(emailTo, subject, body);
                if (result)
                {
                    data.Password = MD5Hash(Base64Encode(newPassword));
                    //UpdateModel(data);
                    db.SubmitChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
