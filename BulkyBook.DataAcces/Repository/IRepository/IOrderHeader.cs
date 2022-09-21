﻿using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAcces.Repository.IRepository
{
    public interface IOrderHeader : IRepository<OrderHeaderModel>
    {
        void Update(OrderHeaderModel obj);
        void UpdateStatus(int id, string orderStatus, string paymentstatus);
    }
}
