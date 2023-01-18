using Microsoft.EntityFrameworkCore;
using StokKontrolProject.Entities.Entities;
using StokKontrolProject.Repositories.Abstract;
using StokKontrolProject.Repositories.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace StokKontrolProject.Repositories.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StokKontrolContext _context;

        public GenericRepository(StokKontrolContext context)
        {
            _context = context;
        }
              
        public bool Add(T item)
        {
            try
            {
                item.AddedDate = DateTime.Now;
                _context.Set<T>().Add(item);
                return Save() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Add(List<T> items)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    foreach (T item in items)
                    {
                        item.AddedDate = DateTime.Now;
                        _context.Set<T>().Add(item);
                    }
                    ts.Complete();
                    return Save() > 0;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Any(Expression<Func<T, bool>> exp) => _context.Set<T>().Any(exp);


        public bool DeleteAll(Expression<Func<T, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public List<T> GetActive() => _context.Set<T>().Where(x => x.IsActive == true).ToList();
        public IQueryable<T> GetActive(params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().Where(x => x.IsActive == true);
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }

        public List<T> GetAll() => _context.Set<T>().ToList();

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>().AsQueryable();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }


        public T GetByDefault(Expression<Func<T, bool>> exp) => _context.Set<T>().FirstOrDefault(exp);


        public List<T> GetDefault(Expression<Func<T, bool>> exp) => _context.Set<T>().Where(exp).ToList();


        public bool Remove(T item)
        {
            item.IsActive = false;
            return Update(item); ;
        }

        public bool Remove(int id)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    T item = GetByID(id);
                    item.IsActive = false;
                    ts.Complete();
                    return Update(item);
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public bool RemoveAll(Expression<Func<T, bool>> exp)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    var collection = GetDefault(exp);
                    int count = 0;
                    foreach (var item in collection)
                    {
                        item.IsActive = false; ;
                        bool operationResult = Update(item);
                        if (operationResult) count += 1;

                    }

                    if (collection.Count == count) ts.Complete();
                    else return false;
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public bool Update(T item)
        {
            try
            {
                //item.IsActive = true;
                item.ModifiedDate = DateTime.Now;
                _context.Set<T>().Update(item);
                return Save() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Activate(int id)
        {
            T item = GetByID(id);
            item.IsActive = true;
            return Update(item);
        }                

        public T GetByID(int id) => _context.Set<T>().Find(id);

        public void DetachEntity(T item)
        {
            _context.Entry<T>(item).State = EntityState.Detached; // Bir entry tkip etmeyi bırakmak için kulllanılır
        }
    }
}
