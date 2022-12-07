using DoAnChuyenNganh_SQLServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnChuyenNganh_SQLServer.Interface
{
    public interface IUser
    {
        //get information
        object GetInformation(string id);

        object ratting(FeedBack feb);
    }
}
