using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class AgenciesContext: DbContext
    {
        public DbSet<Agencija> Agencije { get; set; }

        public DbSet<Korisnik> Korisnici { get; set; }

        public DbSet<Destinacija> Destinacije { get; set; }

        public DbSet<Ponuda> Ponude { get; set; }

        public DbSet<Rezervacija> Rezervacije { get; set; }

        public AgenciesContext(DbContextOptions options) : base(options){
            
        }
    }
}