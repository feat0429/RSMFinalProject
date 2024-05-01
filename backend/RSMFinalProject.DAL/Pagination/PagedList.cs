namespace RSMFinalProject.DAL.Pagination
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PagedList<T>
    {
        public IEnumerable<T> Items { get; }
        public int PageSize { get; private set; }
        public int CurrentPage { get; private set; }
        public int TotalItemCount { get; private set; }
        public int TotalPageCount { get; private set; }
        public bool HasPrevious => (CurrentPage > 1);
        public bool HasNext => (CurrentPage < TotalPageCount);

        private PagedList(List<T> items, int pageSize, int currentPage, int totalItemCount)
        {
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalItemCount = totalItemCount;
            TotalPageCount = (int)Math.Ceiling(TotalItemCount / (double)PageSize);
            Items = items;
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> query, int currentPage, int pageSize)
        {
            var count = await query.CountAsync();

            var pagedItems = await query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedList<T>(pagedItems, pageSize, currentPage, count);
        }
    }
}
