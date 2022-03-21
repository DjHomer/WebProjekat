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
    public class AgencijaController : ControllerBase
    {
        public AgenciesContext Context { get; set;}

        public AgencijaController(AgenciesContext context)
        {
            Context = context;
        }

        [Route("VratiAgenciju")]
        [HttpGet]
        public async Task<ActionResult> vratiAgenciju(int idAgencije = 0) //PROVERI SA FRONTOM OVO KASNIJE
        {
            if(idAgencije < 0)
                return BadRequest("Nevalidan ID");

            if(idAgencije == 0){
                var data = await Context.Agencije.ToListAsync();
                return Ok(data);

            }else{
                var data = await Context.Agencije.FindAsync(idAgencije);

                if(data == null) return NotFound("Ne postoji agencija sa tim ID-jem");            
                return Ok(data);
            }
        }

        [Route("DodajAgenciju")]
        [HttpPost]
        public async Task<ActionResult> dodajAgenciju(Agencija a)
        {
            Context.Agencije.Add(a);
            try{
                await Context.SaveChangesAsync();
                return Ok("Sve je u redu !");  
            }catch{
                return BadRequest("Greska pri dodavanju");
            }
                      
        }

        
        [Route("IzmeniAgenciju/{ID}")]
        [HttpPut]
        public async Task<ActionResult> menjajAgenciju(int ID,Agencija temp)
        {
            var AgencijaZaMenjati = Context.Agencije.Find(ID);
            if(AgencijaZaMenjati == null) return NotFound("Podatak nije nadjen !");

            AgencijaZaMenjati.Naziv = temp.Naziv;
            AgencijaZaMenjati.GradOsnivanja = temp.GradOsnivanja;
            AgencijaZaMenjati.BrTelefona = temp.BrTelefona;
            AgencijaZaMenjati.Email = temp.Email;
            
            await Context.SaveChangesAsync();
            return Ok("Podatak izmenjen !");
             
        }

        
        [Route("ObrisiAgenciju")] 
        [HttpDelete]
        public async Task<ActionResult> obrisiAgenciju(int ID)
        {
            if(ID < 0){
                return BadRequest("Neispravan ID");
            }

            try{
                var pon = Context.Agencije.Find(ID);
                if(pon != null)
                {
                    Context.Agencije.Remove(pon);

                    await Context.SaveChangesAsync();
                    return Ok("Sve je u redu.");
                }else
                    return NotFound("Ne postoji agencija sa ID:" + ID.ToString());
            }catch(Exception e){
                return BadRequest(e.Message);
            }
        }
    }
}