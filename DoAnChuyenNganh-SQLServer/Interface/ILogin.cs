using DoAnChuyenNganh_SQLServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnChuyenNganh_SQLServer.Interface
{
    public interface ILogin
    {
        Customer Login(string email, string password);
        bool SignUp(Customer cu);
        bool SentPassword(string email);
    }
}