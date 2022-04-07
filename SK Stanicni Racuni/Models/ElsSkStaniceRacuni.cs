using System;
using System.Collections.Generic;

#nullable disable

namespace SK_Stanicni_Racuni.Models
{
    public partial class ElsSkStaniceRacuni
    {
        public int? Sekcija { get; set; }
        public string Sifra { get; set; }
        public string Naziv { get; set; }
        public string Status { get; set; }
        public string NadzornaStanicaSifra { get; set; }
        public string NadzornaStanicaNaziv { get; set; }
        public int? Blagajna { get; set; }
        public int? SifraBlagajne { get; set; }
        public int? K140Od { get; set; }
        public int? K140Do { get; set; }
        public int? K165Od { get; set; }
        public int? K165Do { get; set; }
        public int? K140mOd { get; set; }
        public int? K140mDo { get; set; }
        public int? K165mOd { get; set; }
        public int? K165mDo { get; set; }
        public int? K161fOd { get; set; }
        public int? K161fDo { get; set; }
        public int? K121aOd { get; set; }
        public int? K121aDo { get; set; }
    }
}
