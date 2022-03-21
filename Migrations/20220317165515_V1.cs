using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebProjekat.Migrations
{
    public partial class V1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agencija",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GradOsnivanja = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    BrTelefona = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agencija", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Destinacija",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Drzava = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Grad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Smestaj = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Slika = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinacija", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Korisnik",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    JMBG = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Grad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrTelefona = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnik", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Ponuda",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DestinacijaID = table.Column<int>(type: "int", nullable: true),
                    Cena = table.Column<int>(type: "int", nullable: false),
                    DatumPolaska = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumPovratka = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxBrojOsoba = table.Column<int>(type: "int", nullable: false),
                    AgencijaID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ponuda", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Ponuda_Agencija_AgencijaID",
                        column: x => x.AgencijaID,
                        principalTable: "Agencija",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ponuda_Destinacija_DestinacijaID",
                        column: x => x.DestinacijaID,
                        principalTable: "Destinacija",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rezervacija",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrojOsoba = table.Column<int>(type: "int", nullable: false),
                    KorisnikID = table.Column<int>(type: "int", nullable: true),
                    PonudaID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezervacija", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Rezervacija_Korisnik_KorisnikID",
                        column: x => x.KorisnikID,
                        principalTable: "Korisnik",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rezervacija_Ponuda_PonudaID",
                        column: x => x.PonudaID,
                        principalTable: "Ponuda",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ponuda_AgencijaID",
                table: "Ponuda",
                column: "AgencijaID");

            migrationBuilder.CreateIndex(
                name: "IX_Ponuda_DestinacijaID",
                table: "Ponuda",
                column: "DestinacijaID");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacija_KorisnikID",
                table: "Rezervacija",
                column: "KorisnikID");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacija_PonudaID",
                table: "Rezervacija",
                column: "PonudaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rezervacija");

            migrationBuilder.DropTable(
                name: "Korisnik");

            migrationBuilder.DropTable(
                name: "Ponuda");

            migrationBuilder.DropTable(
                name: "Agencija");

            migrationBuilder.DropTable(
                name: "Destinacija");
        }
    }
}
