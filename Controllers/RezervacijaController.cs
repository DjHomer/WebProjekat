using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Microsoft.EntityFrameworkCore;

namespace WebProjekat.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RezervacijaController : ControllerBase
    {
        
        public AgenciesContext Context { get; set;}

         public RezervacijaController(AgenciesContext context)
        {
            Context = context;
        }

        [Route("VratiRezervacije")]
        [HttpGet]
        public async Task<ActionResult> vratiRez(int idAgencije, string sifraRez = "none", string JMBG = "none"){
            if(sifraRez == "none" && JMBG == "none")
            {
                /*var data = await Context.rezervacija
                .Include(p => p.klijent)
                .Include(p => p.ponuda)
                .Include(p => p.ponuda.lokacija)
                .ToListAsync();*/
                return BadRequest("Molimo unesite vas kod rezervacije ili jmbg");
            }else if(JMBG != "none"){
                var data = await Context.Rezervacije
                .Where(p => p.Korisnik.JMBG == JMBG && p.Ponuda.Agencija.ID == idAgencije)
                .Include(p => p.Korisnik)
                .Include(p => p.Ponuda)
                .ThenInclude(p => p.Destinacija)
                .Include(p => p.Ponuda)
                .ThenInclude(p => p.Agencija)
                //.Where(p => p.ID == idAgencije)
                .ToListAsync();
                return Ok(data);
            }else{
                
                var data = await Context.Rezervacije
                .Where(p => p.SifraRez == sifraRez)
                .Include(p => p.Korisnik)
                .Include(p => p.Ponuda)
                .Include(p => p.Ponuda.Destinacija)
                .ToListAsync();
                if(data.Any())
                    return Ok(data);
                else
                    return NotFound();
            }
        }
        
        [Route("KreirajRezervaciju")]
        [HttpPost]
        public async Task<ActionResult> Rezervisi(string JMBG, int IDponude,int brojOsoba){

            if(string.IsNullOrWhiteSpace(JMBG))
                return BadRequest("Neispravan JMBG");
            if(IDponude < 0)
                return BadRequest("Neispravan ID ponude.");

            try{
                Rezervacija rez = new Rezervacija();
                Ponuda po = Context.Ponude.Find(IDponude);

                if(po.MaxBrojOsoba < brojOsoba)
                    return BadRequest("Broj osoba je premasen max broju osoba! Maksimalan broj osoba za ovu ponudu je: " + po.MaxBrojOsoba);
                
                Korisnik kor = Context.Korisnici
                                .Where(p => p.JMBG == JMBG) 
                                .First();
                if(kor != null)
                    rez.Korisnik = kor;
                else
                    return BadRequest("Nevalidni JMBG.");

                rez.Ponuda = po;
                rez.BrojOsoba = brojOsoba;
                
                Boolean postoji = true;
                string potencKodRez = "";
                
                while(postoji)
                {
                    
                    string options = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
                    for(int i = 0; i < 8; i++)
                    {
                        Random nasumican = new Random();   
                        int num = nasumican.Next(1,62);
                        potencKodRez += options[num]; 
                    }
                    var isItInRez = await Context.Rezervacije
                                    .Where(p => p.SifraRez == potencKodRez)
                                    .ToListAsync();

                    if(!isItInRez.Any())
                    {
                        postoji = false;
                    }
                    else
                    {
                        potencKodRez = "";
                    }
                }

                rez.SifraRez = potencKodRez;  
                Context.Rezervacije.Add(rez);
                await Context.SaveChangesAsync();
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(rez.SifraRez));
            }catch(Exception e){
                return BadRequest(e.Message);
            }
        } 

        [Route("IzmeniRezervaciju")]
        [HttpPut]
        public async Task<ActionResult> MenjajRez(int ID, int brojOsoba){
            if(ID < 0)
                return BadRequest("Nevalidan JMBG!");
            
            var rezervacija = await Context.Rezervacije.Where(p => p.ID == ID)
            .Include(p => p.Ponuda)
            .ToListAsync();

            if(!rezervacija.Any())
                return NotFound("Ne postoji rezervacija sa ID-jem "+ID);

            else if(rezervacija[0].Ponuda.MaxBrojOsoba < brojOsoba)
            {
                return BadRequest("Maksimalan broj osoba za ovu ponudu je: " + rezervacija[0].Ponuda.MaxBrojOsoba);
            }

            rezervacija[0].BrojOsoba = brojOsoba;
                        
            await Context.SaveChangesAsync();
            return Ok("Uspesno azurirana rezervacija");
        }


        [Route("ObrisiRezervaciju/{ID}")]
        [HttpDelete]
        public async Task<ActionResult> ObrisiRez(int ID){
            if(ID < 0){
                return BadRequest("Neispravan ID");
            }

            try{
                var rez = Context.Rezervacije.Find(ID);
                if(rez != null)
                {
                    Context.Rezervacije.Remove(rez);

                    await Context.SaveChangesAsync();
                    return Ok("Uspesno obrisana rezervacija!");
                }else
                    return NotFound("Ne postoji rezervacija sa ID:" + ID.ToString());
            }catch(Exception e){
                return BadRequest(e.Message);
            }
        }

    }
}