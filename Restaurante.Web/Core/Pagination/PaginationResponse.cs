using Restaurante.Web.Core.Pagination.Abstractions;

namespace Restaurante.Web.Core.Pagination
{
    public class PaginationResponse<T> : IPagination
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int RecordsPerPage { get; set; }
        public string? Filter { get; set; }
        public int TotalCount { get; set; }
        public PagedList<T> List { get; set; } = new();

        public int VisiblePages { get; set; } = 5;

        public List<int> Pages
        {
            get
            {
                List<int> pages = new List<int>();

                int half = VisiblePages / 2;
                int start = CurrentPage - half + 1 - (VisiblePages % 2);
                int end = CurrentPage + half;

                int visiblePages = VisiblePages;

                if (visiblePages > TotalPages)
                {
                    visiblePages = TotalPages;
                }

                if (start <= 0)
                {
                    start = 1;
                    end = visiblePages;
                }

                if (end > TotalPages)
                {
                    start = TotalPages - visiblePages + 1;
                    end = TotalPages;
                }

                for (int page = start; page <= end; page++)
                {
                    pages.Add(page);
                }

                return pages;
            }
        }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
    }
}