using System.Collections.Generic;
using System;
using System.Linq;

namespace Util
{
    public class PagingList<T>
    {
        public List<T> List { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPageNumber { get; set; }
        public int TotalPageCount { get; set; }
        public int PageSize { get; set; }
    }

    public static class QueryableExtensions
    {
        public static PagingList<T> ToPagedQuery<T>(this IQueryable<T> query, int pageNumber = 1, int pageSize = 10)
        {

            var totalCount = query.Count();
            var list = new PagingList<T>()
            {
                List = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList(),
                CurrentPageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPageCount = (int)Math.Ceiling((double)totalCount / pageSize)

            };

            return list;
        }
        public static PagingList<T> ToPagedQuery<T>(this IEnumerable<T> query, int pageNumber = 1, int pageSize = 10)
        {

            var totalCount = query.Count();
            var list = new PagingList<T>()
            {
                List = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList(),
                CurrentPageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPageCount = (int)Math.Ceiling((double)totalCount / pageSize)

            };

            return list;
        }

    }
}
