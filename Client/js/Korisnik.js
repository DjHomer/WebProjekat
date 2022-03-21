export class Korisnik{

    constructor(id="",ime="",prezime="",jmbg="",grad="",adresa="",telefon=""){
        this.id = id;
        this.ime = ime;
        this.prezime = prezime;
        this.jmbg = jmbg;
        this.grad = grad;
        this.adresa = adresa;
        this.telefon = telefon;
    }

    async getFromDatabaseAsync(jmbg){


        fetch("https://localhost:5001/Korisnik/VratiKorisnika/" + jmbg)
        .then(k => {//
            k.json().then(korisnik =>
                {
                    let kor = new Korisnik(korisnik.id, korisnik.ime, korisnik.prezime, korisnik.jmbg, korisnik.grad, korisnik.email, korisnik.brTelefona)
                    return kor

                }).catch(err =>
                    {
                        console.log("Greska oko serijalizacije verovatno, zanemarljiva " + err)
                    })
        }).catch(err2 =>
            {
                console.log("Ili spolja mzd " + err2)
            })
    }



    async addToDatabaseAsync()
    {
        console.log("POZIVANJE ADDTODATABASE KORISNIKA")
        let data = {
            "ime": this.ime,
            "prezime": this.prezime,
            "jmbg": this.jmbg,
            "grad": this.grad,
            "email": this.adresa,
            "brTelefona": this.telefon
        }
        console.log(data)

        fetch("https://localhost:5001/Korisnik/KreirajKorisnika", {
            method: "POST",
            headers: {'Content-Type': 'application/json'}, 
            body: JSON.stringify(data)
          }).then(res => {
              if(res.status == 200){
                console.log(res);
              }else{
                alert("Neispravno uneti podaci o korisniku !");
              }
          });
    }

    async updateDatabaseAsync()
    {
        let data = {
            "ime": this.ime,
            "prezime": this.prezime,
            "jmbg": this.jmbg,
            "grad": this.grad,
            "email": this.adresa,
            "brTelefona": this.telefon
        }

        fetch("https://localhost:5001/Korisnik/IzmeniKorisnika/" + this.jmbg,
        {
            method: "PUT",
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify(data)
        }).then(res =>
            {
                if(res.status == 200)
                {
                    console.log(res)
                }
            }).catch(err =>
                {
                    console.log(err)
                })

    }

    removeFromDatabase(){
        fetch("https://localhost:5001/Korisnik/ObirisiKorisnika?ID="+this.id,{method:'DELETE'})
        .then(res => console.log(res));
    }
}