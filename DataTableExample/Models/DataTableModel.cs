using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTableExample.Models
{
    [Table("Employee")]
    public class DataTableModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(200)]
        public string FirstName { get; set; }

        [MaxLength(200)]
        public string MiddleName { get; set; }

        [MaxLength(200)]
        public string LastName { get; set; }

        [MaxLength(500)]
        public string EmailId { get; set; }
    }
}
