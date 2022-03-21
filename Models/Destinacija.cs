using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Destinacija")]
    public class Destinacija
    {

        [Key]
        public int ID { get; set; }

        [MaxLength(50)]
        [Required]
        public string Drzava { get; set; }

        [MaxLength(50)]
        [Required]
        public string Grad { get; set; }

        [MaxLength(50)]
        [Required]
        public string Smestaj { get; set; }

        [Required]
        public string Slika { get; set; }
        
        
    }
}