using BulkyBook.DataAcces.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAcces.Repository
{
    public class OrderDetails : Repository<OrderDetailsModel> , IOrderDetails
    {
        private readonly ApplicationDbContext _db;

        public OrderDetails(ApplicationDbContext db) : base(db)
        {
            _db = db;   
        }
    }
}
