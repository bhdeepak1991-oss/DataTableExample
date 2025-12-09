using Microsoft.EntityFrameworkCore;

namespace DataTableExample.Models
{
    public class DataTableContext : DbContext
    {
        public DataTableContext(DbContextOptions<DataTableContext> options)
       : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataTableModel>().ToTable("Employee");
        }

        public virtual DbSet<DataTableModel> DataTableModels { get; set; }
    }
}
