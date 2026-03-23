using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("customer")]
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string? CNA { get; set; } 
        public string? CTEL { get; set; } 
        public string? CTEL1 { get; set; } 
        public string? CSKIND { get; set; } 
        public string? CADDR { get; set; } 
        public int? CPOSTNO { get; set; }
        public string? CCO { get; set; } 
        public string? CMEMO { get; set; } 
    }
}
