using BulkyBook.DataAcces.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAcces.Repository
{
    public class OrderHeader : Repository<OrderHeaderModel>, IOrderHeader
    {
        private readonly ApplicationDbContext _db;
        public OrderHeader(ApplicationDbContext db) : base(db)
        {
            _db = db;

        }

        public void Update(OrderHeaderModel obj)
        {
            _db.OrderHeaders.Update(obj);
        }

        public void UpdateStatus(int id, string orderStatus, string paymentstatus)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(x => x.Id == id);
            if(orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;

                if(paymentstatus != null)
                {
                    orderFromDb.PaymentStatus = paymentstatus;
                }
            }
        }
    }
}
