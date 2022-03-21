using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Korisnik")]
    public class Korisnik
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(30)]
        [Required]
        public string Ime { get; set; }

        [MaxLength(30)]
        [Required]
        public string Prezime { get; set; }

        [Required]
        [RegularExpression("^[0-9]*$")]
        public string JMBG { get; set; }

        [Required]
        public string Grad { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^[0-9]*$")]
        public string BrTelefona { get; set; }
    
    }
}