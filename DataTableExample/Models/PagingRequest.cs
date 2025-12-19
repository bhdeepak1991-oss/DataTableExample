namespace DataTableExample.Models
{
    public class PagingRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public string SearchText { get; set; }
    }
}