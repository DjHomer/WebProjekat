using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Agencija")]
    public class Agencija
    {
        
        [Key]
        public int ID { get; set; }

        [MaxLength(50)]
        [Required] 
        public string Naziv { get; set; }

        [MaxLength(30)]
        [Required]
        public string GradOsnivanja { get; set; }

        [Required]
        [MaxLength(30)]
        public string BrTelefona { get; set; }

        [Required]
        public string Email { get; set; }

    }
}