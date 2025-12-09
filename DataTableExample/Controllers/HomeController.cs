using DataTableExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;
using System.Linq.Dynamic.Core;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataTableExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataTableContext _context;
        public HomeController(ILogger<HomeController> logger, DataTableContext context)
        {
            _logger = logger;
            _context = context;
        }


        public async Task<IActionResult> Index()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadEmployees()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Convert.ToInt32(Request.Form["start"].FirstOrDefault());
                var length = Convert.ToInt32(Request.Form["length"].FirstOrDefault());
                var sortColumn = Request.Form[$"columns[{Request.Form["order[0][column]"]}][name]"].FirstOrDefault();
                var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                var firstNameSearch = Request.Form["columns[1][search][value]"].FirstOrDefault();
                var middleNameSearch = Request.Form["columns[2][search][value]"].FirstOrDefault();
                var lastNameSearch = Request.Form["columns[3][search][value]"].FirstOrDefault();
                var emailSearch = Request.Form["columns[4][search][value]"].FirstOrDefault();


                #region EF CORE Code for Filter
                //var query = _context.DataTableModels.AsQueryable();

                //// Searching
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    query = query.Where(x =>
                //        x.FirstName.Contains(searchValue) ||
                //        x.LastName.Contains(searchValue) ||
                //        x.EmailId.Contains(searchValue));
                //}

                //// Sorting using Dynamic LINQ
                //if (!string.IsNullOrEmpty(sortColumn))
                //{
                //    query = query.OrderBy($"{sortColumn} {sortDirection}");
                //}

                //// Paging
                //var recordsTotal = await query.CountAsync();
                //var data = await query.Skip(start).Take(length).ToListAsync();

                #endregion

                var conn = _context.Database.GetDbConnection();
                await conn.OpenAsync();

                var cmd = conn.CreateCommand();
                cmd.CommandText = "SP_GetEmployees_DataTable";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@SearchValue", searchValue ?? ""));
                cmd.Parameters.Add(new SqlParameter("@FirstNameSearch", firstNameSearch ?? ""));
                cmd.Parameters.Add(new SqlParameter("@MiddleNameSearch", middleNameSearch ?? ""));
                cmd.Parameters.Add(new SqlParameter("@LastNameSearch", lastNameSearch ?? ""));
                cmd.Parameters.Add(new SqlParameter("@EmailIdSearch", emailSearch ?? ""));
                cmd.Parameters.Add(new SqlParameter("@SortColumn", sortColumn ?? "Id"));
                cmd.Parameters.Add(new SqlParameter("@SortDirection", sortDirection ?? "asc"));
                cmd.Parameters.Add(new SqlParameter("@Start", start));
                cmd.Parameters.Add(new SqlParameter("@Length", length));

                List<DataTableModel> data = new List<DataTableModel>();

                var reader = await cmd.ExecuteReaderAsync();

                int recordsTotal = 0;
                int filteredRecordCount = 0;

                while (await reader.ReadAsync())
                {
                    data.Add(new DataTableModel
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FirstName = reader["FirstName"].ToString() ?? string.Empty,
                        MiddleName = reader["MiddleName"].ToString() ?? string.Empty,
                        LastName = reader["LastName"].ToString() ?? string.Empty,
                        EmailId = reader["EmailId"].ToString() ?? string.Empty,

                    });

                    recordsTotal = Convert.ToInt32(reader["TotalCount"]);
                    filteredRecordCount = Convert.ToInt32(reader["FilteredCount"]);
                }

                await conn.CloseAsync();


                return Json(new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = data
                });
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                throw;
            }
           
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
