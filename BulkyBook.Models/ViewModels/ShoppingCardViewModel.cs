﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models.ViewModels
{
    public class ShoppingCardViewModel
    {
        public IEnumerable<ShoppingCart> ListCart { get; set; }
        public double CartTotal { get; set; }

        public OrderHeaderModel OrderHeader { get; set; }
    }
}
