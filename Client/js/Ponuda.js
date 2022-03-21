import { Korisnik } from "./Korisnik.js";
//import { destinacija } from "./destinacija.js";
import { Rezervacija } from "./Rezervacija.js";

export class Ponuda {
    
    constructor(id,destinacija,cena,datumPolaska, datumPovratka, slika, maxBrojOsoba, brOsobaPrethodnaForma){
        this.id = id;
        this.destinacija = destinacija;
        this.cena = cena;
        this.datumPolaska = datumPolaska;
        this.datumPovratka = datumPovratka;
        this.slika = slika;
        this.maxBrojOsoba = maxBrojOsoba;
        this.brOsobaPrethodnaForma = brOsobaPrethodnaForma;
    }

    async getFromDatabaseAsync(id){
        await fetch("https://localhost:5001/Ponuda?ID="+id)
        .then(res => res.json())
        .then(res => {
            this.id = res.id;
            this.destinacija = res.destinacija;
            this.cena = res.cena;
            this.datumPolaska = res.datumPolaska;
            this.datumPovratka = res.datumPovratka;
        })
        .catch(err => alert(err));
    }

    draw(container)
    {
            console.log(this.maxBrojOsoba)
            console.log(this.brOsobaPrethodnaForma)
            let row = document.createElement("div");
            row.classList.add("box");
            row.classList.add("border");
            let slika = document.createElement("img");
            let polje1 = document.createElement("h4");
            let polje2 = document.createElement("h5");
            let polje3 = document.createElement("h5");
            let polje4 = document.createElement("h5");

            slika.src = this.slika; 
            slika.alt = "Nema slike !";
            polje1.innerText = "Destinacija : " +this.destinacija.drzava + " - " + this.destinacija.grad;
            polje3.innerText = "Smestaj : "+ this.destinacija.smestaj;
            polje2.innerText = "Cena : " + this.cena;
            polje4.innerText = "Br kreveta : " + this.maxBrojOsoba; 
            
            
            let buttonRez = document.createElement("button");

            buttonRez.innerText = "Rezerviši";

            buttonRez.onclick = () =>
            {
                let container = document.querySelectorAll(".container").item(1);
                container.innerHTML='';
                let rezForma = document.querySelector(".rezForma");
                rezForma.classList.add("border"); 
                rezForma.style = "background-color: whitesmoke;"
                rezForma.innerHTML = '';


                let noviLabel = document.createElement("label");    
                noviLabel.innerHTML = "Unesite vase podatke: ";

                let cenaLabel = document.createElement("h5")
                cenaLabel.classList.add("dinari")
                cenaLabel.innerText = "Cena : " + this.cena + "din."

                

                let novKorisnik = document.createElement("button");

                let ime = document.createElement("input");
                ime.type="text";
                ime.placeholder="Ime";

                let prezime = document.createElement("input");
                prezime.type="text";
                prezime.placeholder = "Prezime";

                let grad = document.createElement("input");
                grad.type="text";
                grad.placeholder ="Grad stanovanja";

                let adresa = document.createElement("input");
                adresa.type="text";
                adresa.placeholder = "Email adresa";

                let telefon = document.createElement("input");
                telefon.type="text";
                telefon.placeholder = "Broj Telefona";

                let jmbg = document.createElement("input");
                jmbg.type="text";
                jmbg.placeholder = "JMBG";
                

                let brojOsobaN = document.createElement("input");
                brojOsobaN.type = "number";
                brojOsobaN.min = 1;
                brojOsobaN.max = this.maxBrojOsoba
                brojOsobaN.onchange = () =>
                {
                    console.log(brojOsobaN.value)
                    console.log(this.maxBrojOsoba)
                    if(brojOsobaN.value < 1)
                    {
                        alert("Jel se salis? Min broj osoba je 1")
                        brojOsobaN.value = 1;
                        return
                    }
                    if(brojOsobaN.value > this.maxBrojOsoba)
                    {
                        alert("Broj unetih osoba ne sme biti visi od " + this.maxBrojOsoba)
                        brojOsobaN.value = this.maxBrojOsoba;
                    }
                }

                brojOsobaN.placeholder = "Unesite broj osoba";

                novKorisnik.innerText = "Rezerviši!";   

                novKorisnik.onclick = async () =>
                {
                    if(!ime.value)
                    {
                        alert("Unesite ime molim vas!");
                        return

                    }
                    if(!prezime.value)
                    {
                        alert("Unesite prezime molim vas!")
                    }
                    if(!grad.value)
                    {
                        alert("Unesite vase mesto stanovanja")
                    }
                    if(!(telefon.value && this.isItAllNumbers(telefon.value)))
                    {
                        alert("Ostavili ste prazno polje za telefon ili ste uneli neki drugi karakter pored cifre!")
                    }
                    if(!adresa.value)
                    {
                        alert("Unesite vas Email!")
                    }

                    if (! (this.isItAllNumbers(jmbg.value) && jmbg.value.length == 13))
                    {
                        alert("Neispravno ukucan JMBG! Proverite da li ima 13 cifara ili da niste ukucali neki karakter")
                        return
                    }
                    else
                    {
                        let korisnikIzmenaMyb = new Korisnik(0,ime.value,prezime.value,jmbg.value, grad.value,adresa.value,telefon.value);

                        fetch("https://localhost:5001/Korisnik/VratiKorisnika/" + jmbg.value)
                        .then(k => 
                        {
                            console.log(k)
                            k.json().then(korisnik =>
                                {
                                    let kor = new Korisnik(korisnik.id, korisnik.ime, korisnik.prezime, korisnik.jmbg, korisnik.grad, korisnik.email, korisnik.brTelefona)
                                    if(kor.id != "")
                                    {
                                        korisnikIzmenaMyb.updateDatabaseAsync();
                                        let rez = new Rezervacija(0,korisnikIzmenaMyb,this,brojOsobaN.value);
                                        rez.addToDatabase(korisnikIzmenaMyb.jmbg, this.id);
                                        
                                        
                                    }
                                    else
                                    {
                                        

                                        let k = new Korisnik(0,ime.value,prezime.value,jmbg.value,grad.value,adresa.value,telefon.value);
                                        let data = {
                                            "ime": ime.value,
                                            "prezime": prezime.value,
                                            "jmbg": jmbg.value,
                                            "grad": grad.value,
                                            "email": adresa.value,
                                            "brTelefona": telefon.value
                                        }
                                    
                                        fetch("https://localhost:5001/Korisnik/KreirajKorisnika", {
                                            method: "POST",
                                            headers: {'Content-Type': 'application/json'}, 
                                            body: JSON.stringify(data)
                                          }).then(res => {
                                              if(res.status == 200){
                                                let rez = new Rezervacija(0,k,this,brojOsobaN.value);
                                                rez.addToDatabase();

                                              }else{
                                                alert("Neispravno uneti podaci o korisniku !");
                                              }
                                          });
                                    }


                                    document.body.removeChild(rezForma);

                
                                }).catch(err =>
                                    {
                                        console.log("Greska oko serijalizacije verovatno, zanemarljiva " + err)
                                    })
                        }).catch(err2 =>
                            {
                                console.log("Ili spolja mzd " + err2)
                            })
                       

                    }
                    
                }

                rezForma.appendChild(noviLabel);
                rezForma.appendChild(ime);
                rezForma.appendChild(prezime);
                rezForma.appendChild(grad);
                rezForma.appendChild(adresa);
                rezForma.appendChild(jmbg);
                rezForma.appendChild(telefon);
                rezForma.appendChild(brojOsobaN);
                rezForma.appendChild(cenaLabel)
                rezForma.appendChild(novKorisnik);
            }

            row.appendChild(slika);
            row.appendChild(polje1);
            row.appendChild(polje3);
            row.appendChild(polje4);
            row.appendChild(polje2);
            row.appendChild(buttonRez);
            
            container.appendChild(row);
    }

    isItAllNumbers(jmbg)
    {
        for(let i = 0; i < jmbg.length; i++)
        {
            let karakter = jmbg[i]
            if(karakter < '0' || karakter > '9')
            {
                return false
            }
        }
        return true
    }

}