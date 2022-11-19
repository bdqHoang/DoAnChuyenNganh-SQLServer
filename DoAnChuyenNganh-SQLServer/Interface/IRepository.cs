using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DoAnChuyenNganh
{
    public interface IRepository<T> where T : class
    {
        // show data
        Task<List<T>> GetAll();
        // insert data
        Task<T> Insert(T entity);
        // delete
        Task<T> Delete(string id);
        // update
        Task<T> Update(T obj);
        // set db context
        DbSet<T> _dbSet { get; }
    }
}
