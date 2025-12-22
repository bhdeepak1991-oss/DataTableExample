namespace DataTableExample.Models
{
    public class EmployeePagingModel : PagingRequest
    {
        public int IdSearch { get; set; }
        public string FirstNameSearch { get; set; }
        public string MiddleNameSearch { get; set; }
        public string LastNameSearch { get; set; }
        public string EmailIdSearch { get; set; }
    }
}
