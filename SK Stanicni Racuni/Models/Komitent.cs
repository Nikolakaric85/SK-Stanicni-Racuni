using System;
using System.Collections.Generic;

#nullable disable

namespace SK_Stanicni_Racuni.Models
{
    public partial class Komitent
    {
        public int Sifra { get; set; }
        public string Naziv { get; set; }
        public string Mesto { get; set; }
        public string Zemlja { get; set; }
        public string Land { get; set; }
        public string Adresa { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string Pib { get; set; }
        public string Mb { get; set; }
        public string Tr { get; set; }
        public string Osoba { get; set; }
        public int? Sapsifra { get; set; }
    }
}
