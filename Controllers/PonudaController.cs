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
    public class PonudaController : ControllerBase
    {

        public AgenciesContext Context { get; set;}

        public PonudaController(AgenciesContext context)
        {
            Context = context;
        }


        [HttpGet]
        [Route("VratiPonudu")]
        public async Task<ActionResult> PrikazPonude(int ID = -1)
        {
            if(ID == -1)
            {
                var ponude = await Context.Ponude
                .Include(p => p.Destinacija)
                .ToListAsync();
                return Ok(ponude);
            }
            else
            {
                
                var ponude = await Context.Ponude
                .Where(p => p.ID == ID)
                .Include(p => p.Destinacija)
                .ToListAsync();
                if(ponude.Any())
                    return Ok(ponude);
                else
                    return NotFound();
            }
        }


        [Route("FiltrirajPonude")]
        [HttpGet]
        public async Task<ActionResult> filtrirajPonude(int idAgencije ,string grad,DateTime datumPol,DateTime datumPovr, int maxOsoba, int minCena = 0,int maxCena = 0)
        {
            
            if(maxCena == 0){


                if(string.IsNullOrWhiteSpace(grad))
                {
                    var filtriranePonude = await Context.Ponude
                    .Where(p =>  p.Cena > minCena && p.DatumPolaska.Date == datumPol.Date 
                            && p.DatumPovratka.Date == datumPovr.Date && p.MaxBrojOsoba >= maxOsoba && p.Agencija.ID == idAgencije)
                    .Include(p => p.Destinacija)
                    .Include(p => p.Agencija)
                    .ToListAsync();
                    if(filtriranePonude.Any() == true)
                        return Ok(filtriranePonude);
                    else
                        return NotFound();
                }

                else
                {
                    var filtriranePonude = await Context.Ponude
                    .Where(p => p.Destinacija.Grad == grad && 
                        p.Cena > minCena && 
                        p.DatumPolaska.Date == datumPol.Date && 
                        p.DatumPovratka.Date == datumPovr.Date &&
                        p.MaxBrojOsoba >= maxOsoba && 
                        p.Agencija.ID == idAgencije
                    )
                    .Include(p => p.Destinacija)
                    .Include(p => p.Agencija)
                    .ToListAsync();
                    if(filtriranePonude.Any() == true)
                        return Ok(filtriranePonude);
                    else
                        return NotFound();
                }
            }
            else
            {
                if(string.IsNullOrWhiteSpace(grad))
                {
                     var filtrMinMaxPonude = await Context.Ponude
                    .Where(p => p.Cena > minCena && p.Cena < maxCena && 
                            p.DatumPolaska.Date == datumPol.Date && p.DatumPovratka.Date == datumPovr.Date && p.MaxBrojOsoba >= maxOsoba)
                    .Include(p => p.Destinacija)
                    .Include(p => p.Agencija)
                    .ToListAsync();
                    if(filtrMinMaxPonude.Any() == true)
                        return Ok(filtrMinMaxPonude);
                    else
                        return NotFound();

                }
                else
                {

                    var filtrMinMaxPonude = await Context.Ponude
                    .Where(p => p.Destinacija.Grad == grad && p.Cena > minCena && p.Cena < maxCena && 
                            p.DatumPolaska.Date == datumPol.Date && p.DatumPovratka.Date == datumPovr.Date && p.MaxBrojOsoba >= maxOsoba)
                    .Include(p => p.Destinacija)
                    .Include(p => p.Agencija)
                    .ToListAsync();
                    if(filtrMinMaxPonude.Any() == true)
                        return Ok(filtrMinMaxPonude);
                    else
                        return NotFound();

                }
 
            }
            
        }

        [HttpPost]
        [Route("DodajPonudu")]
        public async Task<ActionResult> Dodaj(int IDDst,int IDAg,int cena,DateTime datumPol, DateTime datumPovr, int maxBrojOsoba)
        {

            if(IDDst < 0){
                return BadRequest("Neispravan ID");
            }

            if(cena <= 0){
                return BadRequest("Neispravna cena.");
            }

            try{
                Ponuda pon = new Ponuda();
                Destinacija dst = Context.Destinacije.Find(IDDst);
                Agencija ag = Context.Agencije.Find(IDAg);

                pon.Destinacija = dst;
                pon.Cena = cena;
                pon.DatumPolaska = datumPol;
                pon.DatumPovratka = datumPovr;
                pon.Agencija = ag;
                pon.MaxBrojOsoba = maxBrojOsoba;

                Context.Ponude.Add(pon);
                await Context.SaveChangesAsync();

                return Ok("Sve je u redu.");
            }catch(Exception e){
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("ObrisiPonudu/{ID}")]
        public async Task<ActionResult> ObrisiPonudu(int ID)
        {
            if(ID < 0)
            {
                return BadRequest("Neispravan ID");
            }

            try
            {
                var pon = Context.Ponude.Find(ID);
                if(pon != null)
                {
                    Context.Ponude.Remove(pon);

                    await Context.SaveChangesAsync();
                    return Ok("Sve je u redu.");
                }else
                    return NotFound("Ne postoji ponuda sa ID:" + ID.ToString());
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}