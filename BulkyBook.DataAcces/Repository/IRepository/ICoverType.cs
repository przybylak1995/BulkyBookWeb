using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAcces.Repository.IRepository
{
    public interface ICoverType : IRepository<CoverType>
    {
        void Update(CoverType obj);
    }
}
