using E_commerceAPI.Services.Data;
using E_commerceAPI.Services.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_commerceAPI.Services.Repositories.Services
{
    public class CurdRepository<T> : ICurdRepository<T> where T : class
    {
        private readonly Context _context;
        public CurdRepository(Context _context)
        {
            this._context = _context;
        }
        public async Task<int> Add(T Model)
        {
            await _context.Set<T>().AddAsync(Model);
            return await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            var Data =await _context.Set<T>().ToListAsync();        
            return Data;          
        }
        public async Task<IEnumerable<T>> GetAllInclude(params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var data = await query.ToListAsync();
            return data;
        }
        public async Task<T>? GetByID(int id)
        {
            var Entity = await _context.Set<T>().FindAsync(id);
                return Entity;
        }
        public async Task<T>? GetByName(Expression<Func<T, bool>> expression)
        {
            var Entity = await _context.Set<T>().Where(expression).FirstOrDefaultAsync();
            return Entity;
        }
        public async Task<int> Update(T model, int id)
        {
            T entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Entry(entity).CurrentValues.SetValues(model);
                _context.Entry(entity).State = EntityState.Modified;
                return await _context.SaveChangesAsync();
            }
            return 0;
        }
        public async Task<int> Delete(int id)
        {
            try
            {
                T Entity = await _context.Set<T>().FindAsync(id);
                _context.Set<T>().Remove(Entity);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return await _context.SaveChangesAsync();
            }
        }

        public async Task<T> IsExist(Expression<Func<T,bool>> expression)
        {
            var Entity = await _context.Set<T>().Where(expression).FirstOrDefaultAsync();         
            return Entity;
        }

    }
}
