import { ListaPonuda } from './ListaPonuda.js'
import { Ponuda } from './Ponuda.js'

export class Agencija
{
    constructor(idAgencije){
        this.idAgencije = idAgencije;
    }

    ucitajAgenciju = () =>{
    
        let container = document.createElement("div");
        let divPonude = document.createElement("div");
        let rezForma = document.createElement("div");
        rezForma.classList.add("rezForma");
    
        divPonude.classList.add("ponude");
    
        let filtriranje = document.createElement("div");
        filtriranje.classList.add("filter");
        filtriranje.classList.add("border");
        
        let agencijaIme= document.createElement("h4");
        agencijaIme.classList.add("imeAgencije");
    
        let email = document.createElement("p");
        email.classList.add("krajForme");
        let brFona = document.createElement("p");
        brFona.classList.add("krajForme");
    
        fetch("https://localhost:5001/Agencija/VratiAgenciju?idAgencije="+this.idAgencije)
        .then(p => p.json())
        .then(p => {
            agencijaIme.innerText = p.naziv;
            email.innerHTML = "Email: " + p.email;
            brFona.innerHTML = "Kontakt telefon: "+ p.brTelefona;
        })
    
        filtriranje.appendChild(agencijaIme);
    
        let drzavaLabel = document.createElement("label");
        drzavaLabel.innerHTML = "Država:";
        filtriranje.appendChild(drzavaLabel);
    
        let drzava = document.createElement("select");
        let temp = document.createElement("option");
        temp.innerText = "-";
        drzava.appendChild(temp);
    
        drzava.classList.add("drzavaIzbor");
    
        fetch("https://localhost:5001/Destinacija/VratiSveDrzave")
        .then(p => p.json())
        .then(p => p.forEach(el => {
            let opcija = document.createElement("option");
            opcija.innerHTML = el;
            opcija.value = el;
           drzava.appendChild(opcija);
        }));
    
        filtriranje.appendChild(drzava);
    
        let gradLabel = document.createElement("label");
        gradLabel.innerHTML = "Grad:";
        filtriranje.appendChild(gradLabel);
    
        let grad = document.createElement("select");
        grad.classList.add("gradIzbor");
        filtriranje.appendChild(grad);
        grad.innerHTML = '';
    
        document.body.appendChild(rezForma);
        drzava.onchange = () =>{
    
            grad.innerHTML = '';
        
            fetch("https://localhost:5001/Destinacija/VratiGradoveOdDrzave/"+ drzava.value)
                .then(p => p.json())
                .then(p => p.forEach(el => {
                    let opcija = document.createElement("option");
                    opcija.innerHTML = el;
                    opcija.value = el;
                    grad.appendChild(opcija);
                }))
            
        }
        
        let datumPolInput = document.createElement("input");
        datumPolInput.setAttribute("type","date");
        datumPolInput.setAttribute("name","datum");
        datumPolInput.classList.add("datum");

        let datumPovrInput = document.createElement("input");
        datumPovrInput.setAttribute("type","date");
        datumPovrInput.setAttribute("name","datum");
        datumPovrInput.classList.add("datum");
        
     
    
        var today = new Date();
    
          var dd = today.getDate();
          var mm = today.getMonth()+1;
          var yyyy = today.getFullYear();
    
          if(dd<10) {dd = '0'+dd} 
          if(mm<10) {mm = '0'+mm} 
        
          today = yyyy + '-' + mm + '-' + dd;
    
        let datPolLbl = document.createElement("label");
        datPolLbl.innerText = "Datum Polaska:";
    
        let datPovrLbl = document.createElement("label");
        datPovrLbl.innerText = "Datum Povratka:";
    
        datumPovrInput.value = today;
        datumPolInput.value = today;

        filtriranje.appendChild(datPolLbl);
        filtriranje.appendChild(datumPolInput);
        filtriranje.appendChild(datPovrLbl);
        filtriranje.appendChild(datumPovrInput);
    
        let minLabel = document.createElement("label");
        minLabel.innerHTML = "Minimalna cena";
        filtriranje.appendChild(minLabel);
    
        let minCInput = document.createElement("input");
        minCInput.setAttribute("type","number");
        minCInput.setAttribute("value","1");
        minCInput.setAttribute("name","minCena");
        minCInput.classList.add("minCena");
        minCInput.min = 1;
        filtriranje.appendChild(minCInput);
        
        let maxLabel = document.createElement("label");
        maxLabel.innerHTML = "Maksimalna cena";
        filtriranje.appendChild(maxLabel);
    
        let maxCInput = document.createElement("input");
        maxCInput.setAttribute("type","number");
        maxCInput.setAttribute("value","1");
        maxCInput.setAttribute("name","maxCena");
        maxCInput.classList.add("maxCena");
        maxCInput.min = 1;
        filtriranje.appendChild(maxCInput);

        let brCovekaLbl = document.createElement("label")
        brCovekaLbl.innerHTML = "Broj ljudi:"
        filtriranje.appendChild(brCovekaLbl);
        let brCovekaInput = document.createElement("input")
        brCovekaInput.type = "number";
        brCovekaInput.value = 1;
        brCovekaInput.min = 1;
        brCovekaInput.classList.add("brCoveka");
        filtriranje.appendChild(brCovekaInput)
    
        let klik = document.createElement("button");
        klik.classList.add("klikSelect");
    
        klik.onclick = () =>{
            divPonude.innerHTML = '';
            let dat = document.querySelector(".datum").value;
            let minC = document.querySelector(".minCena").value;
            let maxC = document.querySelector(".maxCena").value;
            let grad = document.querySelector(".gradIzbor").value;
            let drzava = document.querySelector(".drzavaIzbor").value
            let brLjudi = document.querySelector(".brCoveka").value

            console.log(this.idAgencije,grad,minC,maxC,datumPolInput.value,datumPovrInput.value, brLjudi)
            console.log(grad)

            if(grad == "")
            {
                alert("Molimo izaberite jedan od ponudjenih drzava i gradova!")
                return
            }
            if(datumPolInput.value > datumPovrInput.value)
            {
                alert("Datum povratka mora biti nakon datuma polaska!")
                return
            }

            if(brLjudi < 1)
            {
                alert("Molimo unesite validan broj ljudi")
                return
            }
            if(minC < 1)
            {
                alert("Molimo unesite validnu min cenu")
                return
            }

            if(maxC < 1)
            {
                alert("Molimo unesite validnu max cenu")
                return
            }
        
            let listaPonuda = new ListaPonuda(this.idAgencije,grad,minC,maxC,datumPolInput.value,datumPovrInput.value, brLjudi);
    
            listaPonuda.draw(divPonude);
        }
        klik.innerHTML = "Pretraži !";
        filtriranje.appendChild(klik);
    
        filtriranje.appendChild(email);    
        filtriranje.appendChild(brFona); 
    
        container.appendChild(filtriranje);
        container.appendChild(divPonude);
        container.classList.add("container");
        
    
        document.body.appendChild(container);
        
    }
}