using DoAnChuyenNganh_SQLServer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DoAnChuyenNganh.Services
{
    public class RepositoryService<T> : IRepository<T> where T : class
    {
        public DbSet<T> entities;
        private readonly DbContext _context;
        public RepositoryService(GearShopDataContext context)
        {
            //_context = context;
            //entities = context.Set<T>();
        }

        public DbSet<T> _dbSet => entities;

        public async Task<T> Delete(string id)
        {
            var entity = await entities.FindAsync(id);
            if(entities == null)
            {
                throw new NotImplementedException();
            }
            entities.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<T>> GetAll()
        {
            var ListData = await entities.ToListAsync();
            return ListData;
        }

        public async Task<T> Insert(T entity)
        {
            if(entities == null)
            {
                throw new NotImplementedException();
            }
            entities.Add(entity);
            await _context.SaveChangesAsync(); 
            return entity;
        }

        public async Task<T> Update(T obj)
        {
            if(obj == null)
            throw new NotImplementedException();
            else
            {
                await _context.SaveChangesAsync();
                return obj;
            }
        }
    }
}