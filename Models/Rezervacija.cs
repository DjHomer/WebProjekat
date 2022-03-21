using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Rezervacija")]
    public class Rezervacija
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int BrojOsoba {get;set;}

        [Required]
        public string SifraRez { get; set; }

        [Required]
        public Korisnik Korisnik { get; set; }

        [Required]
        public Ponuda Ponuda { get; set; }
    }
}