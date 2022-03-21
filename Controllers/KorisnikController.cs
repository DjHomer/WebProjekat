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
    public class KorisnikController : ControllerBase
    {
        
        public AgenciesContext Context { get; set;}

        public KorisnikController(AgenciesContext context)
        {
            Context = context;
        }

        [Route("VratiKorisnika/{JMBG}")]
        [HttpGet]
        public async Task<ActionResult> vratiKorisnika(string JMBG)
        {   
            var retEmpty = new Object();
            
            if(string.IsNullOrWhiteSpace(JMBG) || JMBG.Length != 13)
            {
                return BadRequest("Lose unet JMBG.");
            }
            else{
                var data = await Context.Korisnici
                        .Where(p => p.JMBG == JMBG)
                        .FirstOrDefaultAsync();
                if(data != null) //data.any()
                    return Ok(data);
                else
                    return Ok(retEmpty);
            }
        }

        [Route("KreirajKorisnika")]
        [HttpPost]
        public async Task<ActionResult> kreirajKorisnika([FromBody] Korisnik k)
        {

            if(string.IsNullOrWhiteSpace(k.Ime)){ 
                return BadRequest("Lose uneto ime korisnika.");
            }

            if(string.IsNullOrWhiteSpace(k.Prezime)){
                return BadRequest("Lose uneto prezime korisnika.");
            }

            if(string.IsNullOrWhiteSpace(k.JMBG) || k.JMBG.Length != 13){
                return BadRequest("Lose unet JMBG.");
            }

             if(string.IsNullOrWhiteSpace(k.Grad) || k.Grad.Length > 50){
                return BadRequest("Lose uneto ime grada.");
            }

             if(string.IsNullOrWhiteSpace(k.Email) || k.Email.Length > 50){
                return BadRequest("Lose uneta adresa.");
            }

             if(string.IsNullOrWhiteSpace(k.BrTelefona) || k.BrTelefona.Length < 9){
                return BadRequest("Lose unet broj telefona.");
            }

            try{
                Context.Korisnici.Add(k);
                await Context.SaveChangesAsync();
                return Ok("sve je u redu");
            }catch(Exception e){
                return BadRequest(e.Message);
            }
        }


        [Route("IzmeniKorisnika/{jmbg}")]
        [HttpPut]
        public async Task<ActionResult> izmeniKorisnika(string jmbg, [FromBody] Korisnik k)
        {
            if(string.IsNullOrWhiteSpace(k.JMBG) || k.JMBG.Length != 13){
                return BadRequest("Lose unet JMBG.");
            }

            
            try
            {
                var korisnikZaIzmenu =  await Context.Korisnici.Where(p => p.JMBG == jmbg).FirstOrDefaultAsync();
                if(korisnikZaIzmenu != null)
                {

                    if(string.IsNullOrWhiteSpace(k.Ime)){ 
                        return BadRequest("Lose uneto ime korisnika.");
                    }

                    if(string.IsNullOrWhiteSpace(k.Prezime)){
                        return BadRequest("Lose uneto prezime korisnika.");
                    }

                    if(string.IsNullOrWhiteSpace(k.JMBG) || k.JMBG.Length != 13){
                        return BadRequest("Lose unet JMBG.");
                    }

                     if(string.IsNullOrWhiteSpace(k.Grad) || k.Grad.Length > 50){
                        return BadRequest("Lose uneto ime grada.");
                    }

                     if(string.IsNullOrWhiteSpace(k.Email) || k.Email.Length > 50){
                        return BadRequest("Lose uneta adresa.");
                    }

                     if(string.IsNullOrWhiteSpace(k.BrTelefona) || k.BrTelefona.Length < 9){
                        return BadRequest("Lose unet broj telefona.");
                    }


                    korisnikZaIzmenu.Ime = k.Ime;
                    korisnikZaIzmenu.Prezime = k.Prezime;
                    korisnikZaIzmenu.JMBG = k.JMBG;
                    korisnikZaIzmenu.Email = k.Email;
                    korisnikZaIzmenu.BrTelefona = k.BrTelefona;
                    korisnikZaIzmenu.Grad = k.Grad;

                    await Context.SaveChangesAsync();
                    return Ok("Uspesno promenjen korisnik!");
                }

                else
                {
                    return BadRequest("Korisnik nije pronadjen!");
                }
                

            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [Route("ObrisiKorisnika/{ID}")]
        [HttpDelete]
        public async Task<ActionResult> obrisiKorisnika(int ID){

            if(ID < 0)
                return BadRequest("Neispravan ID.");
            
            try{
                var korisn = Context.Korisnici.Find(ID);
                if(korisn != null)
                {
                    Context.Korisnici.Remove(korisn);

                    await Context.SaveChangesAsync();
                    return Ok("Uspesno obrisan korisnik!");
                }else
                    return NotFound("Ne postoji korisnik sa ID:" + ID.ToString());
            }catch(Exception e){
                return BadRequest(e.Message);
            }
        }

    }
}