using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Domain.Extensions
{
    public static class OrderExtensions
    {
        public static object Select(this Order type, Func<Order, object> func)
        {
            return ObjectExtensions.Select<Order>(type, func);
        }
    }
}
