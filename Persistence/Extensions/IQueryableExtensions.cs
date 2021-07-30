using System;
using System.Linq;
using System.Threading.Tasks;
using Jaricardodev.Paginator.Model.Capabilities;
using Microsoft.EntityFrameworkCore;

namespace Jaricardodev.Paginator.Persistence.Extensions
{
    public static class IQueryableExtensions
    {
        public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int page, int itemsPerPage)
        {
            var totalCount = source.Count();

            var pagedSource = (PaginatedList<T>)await source
                .Skip(itemsPerPage * (page - 1))
                .Take(itemsPerPage)
                .ToListAsync();

            pagedSource.TotalItemsCount = totalCount;
            pagedSource.TotalPageCount = (int)Math.Ceiling((double)totalCount / itemsPerPage);

            return pagedSource;
        }
    }
}
