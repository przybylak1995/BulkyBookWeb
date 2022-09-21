using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAcces.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public ICategoryRepository Category { get; }
        public ICoverType CoverType { get; }
        public IProductRepository Product { get; }
        public ICompany Company { get; }
        public IShoppingCardRepository ShoppingCard { get; }
        public IApplicationUserRepository ApplicationUser { get; }
        public IOrderHeader OrderHeader { get; }
        public IOrderDetails OrderDetails { get; }

        void Save();
    }
}
