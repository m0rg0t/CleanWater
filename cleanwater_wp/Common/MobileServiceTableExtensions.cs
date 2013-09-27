using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace cleanwater_wp.Common
{
    public static class MobileServiceTableExtensions
    {
        public static async Task<IEnumerable<T>> GetAllAsync<T>(this IMobileServiceTable<T> mobileServiceTable, Int32 count = 50)
        {
            return await GetAll(mobileServiceTable, count);
        }

        public static async Task<IEnumerable<T>> WhereAsync<T>(this IMobileServiceTable<T> mobileServiceTable, Expression<Func<T, bool>> predicate, Int32 count = 50)
        {
            return await Where(mobileServiceTable, predicate, count);
        }

        private static async Task<IEnumerable<T>> GetAll<T>(this IMobileServiceTable<T> mobileServiceTable, Int32 count)
        {
            var list = new List<T>();
            var items = await mobileServiceTable.Take(count).IncludeTotalCount().ToEnumerableAsync();
            list.AddRange(items);
            var countProvider = items as ITotalCountProvider;
            if (countProvider != null)
            {
                var current = count;
                while (current < countProvider.TotalCount)
                {
                    items = await mobileServiceTable.Skip(current).ToEnumerableAsync();
                    list.AddRange(items);
                    current += count;
                }
            }
            return list;
        }

        private static async Task<IEnumerable<T>> Where<T>(this IMobileServiceTable<T> mobileServiceTable, Expression<Func<T, bool>> predicate, Int32 count)
        {
            var list = new List<T>();
            var items = await mobileServiceTable.Where(predicate).Take(count).IncludeTotalCount().ToEnumerableAsync();
            list.AddRange(items);
            var countProvider = items as ITotalCountProvider;
            if (countProvider != null)
            {
                var current = count;
                while (current < countProvider.TotalCount)
                {
                    items = await mobileServiceTable.Where(predicate).Skip(current).ToEnumerableAsync();
                    list.AddRange(items);
                    current += count;
                }
            }
            return list;
        }
    }
}
