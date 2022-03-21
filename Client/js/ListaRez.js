import { Rezervacija } from "./Rezervacija.js";

export class ListaRezervacija
{

    rezervacije = [];
    
    constructor(jmbgOrCode, idAgencije){
        this.jmbgOrCode = jmbgOrCode;
        this.idAgencije = idAgencije
    }

    loadDataAndDraw(container){

        let rez = document.createElement("div");
        rez.classList.add("ponude");
        container.appendChild(rez);
        let paramInterpol = ""

        if(this.jmbgOrCode.length == 13)
        {
            paramInterpol = "&JMBG="
        }
        else if(this.jmbgOrCode.length == 8)
        {
            paramInterpol = "&sifraRez="
            

        }

        fetch("https://localhost:5001/Rezervacija/VratiRezervacije?idAgencije="+ this.idAgencije+ paramInterpol +this.jmbgOrCode)
        .then(res => res.json())
        .then(res => {
            res.forEach(el =>
                {
                    let r = new Rezervacija(el.id,el.korisnik,el.ponuda,el.brojOsoba, el.sifraRez)
                    this.rezervacije.push(r);
                    r.draw(rez)
                })
        })
        .catch(err =>
            {
                alert("Nije pronadjena nijedna rezervacija")
            })

        console.log(this.rezervacije);
    }

    draw(container){
    
        let rez = document.createElement("div");
        rez.classList.add("ponude");
        container.appendChild(rez);

        this.rezervacije.forEach(el => {
            el.draw(rez);
        });
    }
}