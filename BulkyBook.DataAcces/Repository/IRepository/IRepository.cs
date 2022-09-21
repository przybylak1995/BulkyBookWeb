using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAcces.Repository.IRepository
{

    // DIT IS EEN GENERIC REPOSITORY !!!
    public interface IRepository<T> where T : class // Maakt de interface generic zodat niet uitmaakt wat voor class we gaan toevoegen 
    {

        T GetFirstOreDefault(Expression<Func<T, bool>> filter, string? icludeProperties = null);

        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter=null,  string? icludeProperties = null); // T - category

        void Add(T entity); // Add methode T is de class en enitity is een parameter die word toegevoed.

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entity);

    }
}
