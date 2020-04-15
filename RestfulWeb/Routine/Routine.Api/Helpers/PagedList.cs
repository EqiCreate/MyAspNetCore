using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }

        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPrevious => this.CurrentPage > 1;
        public bool HasNext => this.CurrentPage < TotalPages;
        public PagedList(List<T> items, int count, int pagenumber, int pagesize)
        {
            this.TotalCount = count;
            this.PageSize = pagenumber;
            this.CurrentPage = pagenumber;
            this.TotalPages = (int)Math.Ceiling((double)count / pagesize);
            AddRange(items);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pagenumber, int pagesize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip(pagesize * (pagenumber - 1)).Take(pagesize).ToListAsync();
            return new PagedList<T>(items, count, pagenumber, pagesize);
        }
    }


}
