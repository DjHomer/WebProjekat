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
    public class DestinacijaController : ControllerBase
    {
        
        public AgenciesContext Context { get; set;}


          public DestinacijaController(AgenciesContext context)
        {
            Context = context;
        }

        [Route("PrikaziDestinaciju")]
        [HttpGet]
        public async Task <ActionResult> PrikaziDestinaciju(int ID = -1){
            if(ID == -1){
                var data = await Context.Destinacije.ToListAsync();
                return Ok(data);
            }
            else{
                var data =await Context.Destinacije
                .Where(p => p.ID == ID)
                .ToListAsync();
                if(data.Any())
                    return Ok(data);
                else
                    return NotFound();
            }
        }


        [Route("DodatiDestinaciju")]
        [HttpPost]
        public async Task<ActionResult> DodajDestinaciju([FromBody] Destinacija dst){
            
            if(string.IsNullOrWhiteSpace(dst.Drzava)){
                return BadRequest("Lose uneto ime drzave.");
            }

            if(string.IsNullOrWhiteSpace(dst.Grad)){
                return BadRequest("Lose uneto ime grada.");
            }

            if(string.IsNullOrWhiteSpace(dst.Smestaj)){
                return BadRequest("Lose uneto ime hotela.");
            }

            try{
                Context.Destinacije.Add(dst);
                await Context.SaveChangesAsync();
                return Ok("sve je u redu");
            }catch(Exception e){
                return BadRequest(e.Message);
            }
            
            
        }

        [Route("VratiSveDrzave")]
        [HttpGet]
        public ActionResult vratiSveDrzave(){
            var sveDestinacije = Context.Destinacije;
            var drzaveStr = new List<string>();
            foreach (var dst in sveDestinacije)
            {
                if(drzaveStr.Contains(dst.Drzava) == false)
                    drzaveStr.Add(dst.Drzava);
            }
            return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(drzaveStr)); //!!!
        }

        [HttpGet]
        [Route("VratiGradoveOdDrzave/{drzava}")]
        public ActionResult vratiGradove(string drzava){
            var sveDestinacije = Context.Destinacije
            .Where(p => p.Drzava == drzava)
            .ToList();

            var gradoviStr = new List<string>();
            foreach (var dst in sveDestinacije)
            {
                if(gradoviStr.Contains(dst.Grad) == false)
                    gradoviStr.Add(dst.Grad);
            }
            return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(gradoviStr));
        }

        [HttpDelete]
        [Route("ObrisiDestinaciju/{ID}")]
        public async Task<ActionResult> IzbrisiDestinaciju(int ID){
            if(ID < 0){
                return BadRequest("Neispravan ID.");
            }

            try{

                var lok = Context.Destinacije.Find(ID);
                Context.Destinacije.Remove(lok);

                await Context.SaveChangesAsync();

                return Ok($"Izbrisana destinacija sa ID: {lok.ID}");
            }catch(Exception e){
                return BadRequest(e.Message);
            }
        }


    }
}
