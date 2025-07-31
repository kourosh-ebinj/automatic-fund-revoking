using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Core.Constants;
using Core.Exceptions;
using Core.Extensions;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string orderByText)
        {
            if (string.IsNullOrWhiteSpace(orderByText)) return source;

            var isTextValid = orderByText.TryGetSqlInjectionFreeText(out (string ColumnName, bool IsAscending) orderByValues);
            if (!isTextValid) return source;

            ParameterExpression parameter = Expression.Parameter(source.ElementType, "");

            MemberExpression property = Expression.Property(parameter, orderByValues.ColumnName);

            if (property == null)
                throw new CustomException(Core.Enums.ExceptionCodeEnum.BadRequest, customMessage: "Invalid sort column name");

            LambdaExpression lambda = Expression.Lambda(property, parameter);

            string methodName = orderByValues.IsAscending ? "OrderBy" : "OrderByDescending";

            Expression methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                                  new Type[] { source.ElementType, property.Type },
                                  source.Expression, Expression.Quote(lambda));

            return source.Provider.CreateQuery<T>(methodCallExpression);
        }

        //public static async Task<PaginatedList<IEnumerable<T>>> ToPaginatedList<T>(this IQueryable<T> source,
        //                                                            int? pageSize = null, int? pageNumber = null, string orderBy = "",
        //                                                            CancellationToken cancellationToken = default) =>
        //    await ToPaginatedListQueryable(source, pageSize, pageNumber, orderBy).ToListAsync(cancellationToken);

        public static async Task<PaginatedList<T>> ToPaginatedList<T>(this IQueryable<T> query,
                                                                int pageSize = GlobalConstants.DefaultPageSize, int pageNumber = 1, string orderBy = "",
                                                                CancellationToken cancellationToken = default)
        {
            var totalItemsCountTask = await query.CountAsync();
            var totalItems = totalItemsCountTask;

            query = query.ToPaginatedListQueryable(pageSize, pageNumber, orderBy, cancellationToken);

            var items = await query.ToListAsync();
            return new PaginatedList<T>()
            {
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
        }

            public static IQueryable<T> ToPaginatedListQueryable<T>(this IQueryable<T> query,
                                                                int pageSize = GlobalConstants.DefaultPageSize, int pageNumber = 1, string orderBy = "",
                                                                CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrWhiteSpace(orderBy))
                query = query.OrderBy(orderBy);
            if (pageNumber > 1)
                query = query.Skip((pageNumber - 1) * pageSize);
            if (pageSize > 0)
                query = query.Take(pageSize);


            return query;
        }

    }
}
