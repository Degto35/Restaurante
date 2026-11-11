namespace Restaurante.Web.Core.Pagination
{
    public class PaginationRequest
    {
        private int _page = 1;
        private int _recordsPerPage = 15;
        private const int MAX_RECORDS_PER_PAGE = 50;

        public string? Filter { get; set; }

        public int Page
        {
            get => _page;
            set => _page = value > 0 ? value : 1;
        }

        public int RecordsPerPage
        {
            get => _recordsPerPage;

            set => _recordsPerPage = value switch
            {
                <= 0 => 15,
                > MAX_RECORDS_PER_PAGE => MAX_RECORDS_PER_PAGE,
                _ => value
            };
        }

        public static PaginationRequest Default => new PaginationRequest { Page = 1, RecordsPerPage = 15 };
    }
}