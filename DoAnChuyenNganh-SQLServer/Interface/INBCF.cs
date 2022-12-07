using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnChuyenNganh_SQLServer.Interface
{
    public interface INBCF
    {
        string[,] GetData();
        List<string> RecommentForYou(string customerID);
    }
}