using BulkyBook.DataAcces.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAcces.Repository
{

    // Hier maak implemnteer je de interface of zo generic methodes te maken die je in meerdere controllers kan gebruiken!!!!
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet; // deze moet generic zijn omdat je verschillende DBSets kan meegeven deze worden gemaakt in de map dat onder ApplicationDbContext

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            _db.Products.Include(u => u.Category);
            this.dbSet = _db.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter=null ,  string? icludeProperties = null)
        {
            IQueryable<T> query = dbSet;
          
            if(filter != null)
            {
                query = query.Where(filter);
            }

            if(icludeProperties != null)
            {
                foreach (var includeProp in icludeProperties.Split(new char[] {',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }

        public T GetFirstOreDefault(Expression<Func<T, bool>> filter, string? icludeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (icludeProperties != null)
            {
                foreach (var includeProp in icludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            query = query.Where(filter);

            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
