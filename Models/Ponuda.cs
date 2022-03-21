using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Ponuda")]
    public class Ponuda
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public Destinacija Destinacija { get; set; }

        [Required]
        public int Cena { get; set; }

        [Required]
        public DateTime DatumPolaska { get; set; }

        [Required]
        public DateTime DatumPovratka { get; set; }

        [Required]
        public int MaxBrojOsoba {get;set;}

        [Required]
        public Agencija Agencija {get;set;}

    }
}