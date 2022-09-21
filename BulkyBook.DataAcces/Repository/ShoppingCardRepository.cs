using BulkyBook.DataAcces.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAcces.Repository
{
    public class ShoppingCardRepository : Repository<ShoppingCart> , IShoppingCardRepository
    {
        private readonly ApplicationDbContext _db;
        public ShoppingCardRepository(ApplicationDbContext db) : base (db)
        {
            _db = db;
        }

        public int DecrementCount(ShoppingCart cart, int count)
        {
            cart.Count -= count;
            return cart.Count;
        }

        public int IncrementCount(ShoppingCart cart, int count)
        {
            cart.Count += count;
            return cart.Count;
        }
    }
}
