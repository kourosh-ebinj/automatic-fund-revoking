using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Extensions
{
    public static class ListExtensions
    {

        /// <summary>
        /// This function is created to overcome sending max 2100 parameter to query db with "where clause" contain IN(@params)
        /// </summary>
        /// <typeparam name="T">Input Type</typeparam>
        /// <typeparam name="T2">Output type </typeparam>
        /// <param name="action">Action that should have one input param of List<T2></param>
        /// <param name="items">a list of data that should use in where clause IN(@params). </param>
        /// <param name="size">capacity of each batch that will send to db. Default value is 2000 that means this function will split items to list of list with max size item. </param>
        /// <returns></returns>
        public static async Task<List<T>> DoItSplitted<T, T2>(this Func<IEnumerable<T2>, Task<IEnumerable<T>>> action, IEnumerable<T2> items, int size = 2000)
        {
            var ret = new List<T>();
            foreach (var item in items.SplitList(size))
            {
                ret.AddRange(await action(item));
            }

            return ret;
        }

        private static List<List<T2>> SplitList<T2>(this IEnumerable<T2> input, int size)
        {
            var ret = new List<List<T2>>();
            for (var i = 0; i < input.Count() / size + 1; i++)
            {
                ret.Add(input.Skip(i * size).Take(size).ToList());
            }

            return ret;
        }
    }
}
