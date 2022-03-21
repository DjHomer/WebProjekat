import { Agencija } from './Agencija.js';
import { ListaRezervacija } from './ListaRez.js';


let ucitajProveru = (jmbgOrCode, idAgencije) =>{
    console.log(idAgencije)
    document.body.innerHTML = '';
    mainMenu();

    let container = document.createElement("div");
    container.classList.add("container");
    let listaRez = new ListaRezervacija(jmbgOrCode, idAgencije);
    listaRez.loadDataAndDraw(container);
    console.log(listaRez)
    listaRez.draw(container);

    document.body.appendChild(container);
}

let mainMenu = () =>{

    let selectAgenciju = document.createElement("select");
    selectAgenciju.classList.add("select");

    let agencijaBtn = document.createElement("button");
    agencijaBtn.classList.add("klikSelect");
    agencijaBtn.innerText = "Izaberi Agenciju!";

    let agencijaDiv = document.createElement("div");
    agencijaDiv.classList.add("agencijaDiv");

    let containerAgencija = document.createElement("div");
    

    let proveraDiv = document.createElement("div");
    proveraDiv.classList.add("proveraDiv");

    let proveraInput = document.createElement("input");
    proveraInput.type = "text";
    proveraInput.placeholder = "Unesi sifru rez ili jmbg za detalje rezervacije!";
    proveraInput.classList.add("pInput");

    let proveraButton = document.createElement("button");
    proveraButton.innerText = "Proveri !";
    proveraButton.classList.add("klikSelect");


    fetch("https://localhost:5001/Agencija/VratiAgenciju")
    .then(res => res.json())
    .then(agencije =>
        {
            console.log(agencije); 
            agencije.forEach(el => 
            {
                let ag = document.createElement("option");
                ag.id = el.id;
                ag.value = el.id;
                ag.innerHTML = el.naziv;
                selectAgenciju.appendChild(ag);
            })
        });

    agencijaBtn.onclick = () => 
    { 
        document.body.innerHTML = '';
        mainMenu();
        let agencija = new Agencija(selectAgenciju.value);
        agencija.ucitajAgenciju();
    }

    proveraButton.onclick = () => 
    {
        let agencijaTMP = selectAgenciju.value
        console.log(proveraInput.value.length) 
        ucitajProveru(proveraInput.value, agencijaTMP);
    }

    containerAgencija.classList.add("container");
    agencijaDiv.appendChild(selectAgenciju);
    agencijaDiv.appendChild(agencijaBtn);
    containerAgencija.appendChild(agencijaDiv);

    proveraDiv.appendChild(proveraInput);
    proveraDiv.appendChild(proveraButton);
    containerAgencija.appendChild(proveraDiv); // mozda prebaci to dole

    document.body.appendChild(containerAgencija);
}

document.body.onload
{
    mainMenu();
}