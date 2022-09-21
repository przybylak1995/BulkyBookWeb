using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAcces.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category> // interface voor methodes die speciaal voor deze controller gebruikt worden  dit is meestal de Update methode
    {

        void Update(Category obj);
    }
}
