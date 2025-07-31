using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Domain.Extensions
{
    public static class ObjectExtensions
    {
        
        public static object Select<T>(this T type, Func<T, object> func) =>
               func.Invoke(type);
        
    }
}
