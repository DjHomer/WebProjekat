import { Ponuda } from "./Ponuda.js";

export class ListaPonuda{

    constructor(idAgencije,grad, minimumCena,maximumCena,datumPol,datumPovr, brLjudi){
        this.idAgencije = idAgencije;
        this.grad = grad;
        this.minimumCena = minimumCena;
        this.maximumCena = maximumCena;
        this.datumPol = datumPol;
        this.datumPovr = datumPovr;
        this.brLjudi = brLjudi;
    }

    draw(container){

        console.log(this.idAgencije, this.grad, this.minimumCena, this.maximumCena, this.datumPol, this.datumPovr, this.brLjudi);
       
        fetch("https://localhost:5001/Ponuda/FiltrirajPonude?"+"idAgencije="+this.idAgencije+"&grad="+this.grad+"&datumPol="+
            this.datumPol + "&datumPovr="+ this.datumPovr +"&maxOsoba=" + this.brLjudi+ "&minCena="+this.minimumCena+"&maxCena="+this.maximumCena)
        .then(p => p.json())
        .then(p => {
            p.forEach(el => {
                let p = new Ponuda(el.id,el.destinacija,el.cena,el.datumPolaska, el.datumPovratka, el.destinacija.slika, el.maxBrojOsoba, this.brLjudi);
                p.draw(container); 
            })
        })
        .catch(err => {
            alert("Nemamo tu ponudu trenutno u agenciji!");
            console.error(err);
        });
    }
}