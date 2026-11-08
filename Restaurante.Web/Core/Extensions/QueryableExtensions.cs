using Restaurante.Web.Core.Pagination;

namespace Restaurante.Web.Core.Extensions
{
    public static class QueryableExtensions
    {
      
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationRequest request)
        {
            return queryable.Skip((request.Page - 1) * request.RecordsPerPage)
                            .Take(request.RecordsPerPage);
        }
    }
}