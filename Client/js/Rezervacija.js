
export class Rezervacija{
    constructor(id,korisnik,ponuda,brojOsoba, sifraRez){
        this.id = id;
        this.korisnik = korisnik;
        this.ponuda = ponuda;
        this.brojOsoba = brojOsoba;
        this.sifraRez = sifraRez;
    }

    getFromDatabase(id){
        fetch("https://localhost:5001/api/Rezervacija?ID="+id)
        .then(res => res.json())
        .then(data => {
            this.id = data.id;
            this.korisnik = data.korisnik;
            this.ponuda = data.ponuda;
            this.brojOsoba = data.brojOsoba;
        })
        .catch(err => console.log(err));

        
    }

    draw(container){
        console.log("Pozvan draw iz klase rezervacija")

        let row = document.createElement("div");
        row.classList.add("pregled");
        row.classList.add("border");
        let polje1 = document.createElement("h5");
        polje1.classList.add("podaci");
        let polje2 = document.createElement("h5");
        polje2.classList.add("podaci");
        let polje3 = document.createElement("h5");
        polje3.classList.add("podaci");
        let polje4 = document.createElement("h5");
        polje4.classList.add("podaci");
        let polje5 = document.createElement("h5")
        polje5.classList.add("podaci")

        polje1.innerHTML = "Država: " + this.ponuda.destinacija.drzava;
        polje2.innerHTML = "Grad: " + this.ponuda.destinacija.grad;
        polje3.innerHTML = "Hotel: " + this.ponuda.destinacija.smestaj;
        polje4.innerHTML = "Cena: " + this.ponuda.cena;
        polje5.innerHTML = "Broj osoba: " + this.brojOsoba

        let os = document.createElement("label");
        os.innerText = "Izmeni broj osoba:";
        os.classList.add("podaci");

        let numBrOS = document.createElement("input");
        numBrOS.setAttribute("disabled","true");
        numBrOS.type = "number";
        numBrOS.value = this.brojOsoba;
        numBrOS.max = this.ponuda.maxBrojOsoba;
        numBrOS.onchange = () =>
        {
            if(numBrOS.value < 1)
            {
                alert("Jel se salis? Min broj osoba je 1")
                numBrOS.value = 1;
                return
            }
            if(numBrOS.value > this.ponuda.maxBrojOsoba)
            {
                alert("Broj unetih osoba ne sme biti visi od " + this.ponuda.maxBrojOsoba)
                numBrOS.value = this.maxBrojOsoba;
            }
        }


        
        let izmeni = document.createElement("button");
        izmeni.innerText = "Izmeni";
        izmeni.classList.add("izmeniBtn");

        izmeni.onclick = () =>{
            if(izmeni.innerText == "Izmeni"){
                izmeni.innerText = "Potrvrdi";
                numBrOS.disabled = false;
            }else{
                fetch("https://localhost:5001/Rezervacija/IzmeniRezervaciju/?ID="+this.id+"&brojOsoba="+numBrOS.value,
                {method:'PUT'})
                .then(res =>{
                    if(res.status == 200){
                        alert("Uspešno izmenjena rezervacija !");
                    }else{
                        alert("Greška pri izmeni rezervacije !");
                    }
                    numBrOS.disabled = true;
                    numBrDana.disabled = true;
                    izmeni.innerText = "Izmeni";
                })
                .catch(err => console.log(err));
            }
        }

        let obrisi = document.createElement("button");
        obrisi.innerText = "Obriši";
        obrisi.classList.add("obrisiBtn");

        obrisi.onclick =() =>{
            fetch("https://localhost:5001/Rezervacija/ObrisiRezervaciju/" + this.id,
            {
                method:'DELETE'
            })
                .then(res =>{
                    if(res.status == 200){
                        alert("Uspešno obrisana rezervacija !");
                    }else{
                        alert("Greška pri brisanju rezervacije !");
                    }
                    
                })
                container.removeChild(row);
        }

        row.appendChild(polje1);
        row.appendChild(polje2);
        row.appendChild(polje3);
        row.appendChild(polje5)
        row.appendChild(polje4);
        row.appendChild(os);
        row.appendChild(numBrOS);
        row.appendChild(izmeni);
        row.appendChild(obrisi);

        container.appendChild(row);
    }

    addToDatabase(){
        fetch("https://localhost:5001/Rezervacija/KreirajRezervaciju?JMBG="+this.korisnik.jmbg+"&IDponude="+this.ponuda.id+"&brojOsoba="+this.brojOsoba, 
        {method: 'POST'})
            .then(res => {
                if(res.status == 200){
                    res.json().then(sifra =>
                    {
                        alert("Uspesno ste rezervisali putovanje, vas kod je "+ sifra);
                    })
                   
                }else{
                    alert("Greska pri rezervaciji !");
                }
                
            })
            .catch(err => console.log(err));        
    }
}