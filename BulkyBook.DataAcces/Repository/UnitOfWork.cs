using BulkyBook.DataAcces.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAcces.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            CoverType = new CoverTypeRepository(_db);
            Product = new ProductRepository(_db);
            Company = new CompanyRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            ShoppingCard = new ShoppingCardRepository(_db);
            OrderHeader = new OrderHeader(_db);
            OrderDetails = new OrderDetails(_db);
           
        }

        public ICategoryRepository Category { get; private set; }
        public ICoverType CoverType { get; private set; }
        public IProductRepository Product { get; private set; }
        public ICompany Company { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IShoppingCardRepository ShoppingCard { get; private set; }
        public IOrderDetails OrderDetails { get; private set; }
        public IOrderHeader  OrderHeader { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
