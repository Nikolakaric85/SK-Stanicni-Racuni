using System;
using System.Collections.Generic;

#nullable disable

namespace SK_Stanicni_Racuni.Models
{
    public partial class SlogKola
    {
        public SlogKola()
        {
            SlogRobas = new HashSet<SlogRoba>();
        }

        public int RecId { get; set; }
        public string Stanica { get; set; }
        public int KolaStavka { get; set; }
        public string OtpUprava { get; set; }
        public string OtpStanica { get; set; }
        public int OtpBroj { get; set; }
        public DateTime OtpDatum { get; set; }
        public string Ibk { get; set; }
        public string Kontrola { get; set; }
        public string Uprava { get; set; }
        public string Vlasnik { get; set; }
        public string Serija { get; set; }
        public int? Osovine { get; set; }
        public int? Tara { get; set; }
        public int? GranTov { get; set; }
        public string Stitna { get; set; }
        public string TipKola { get; set; }
        public decimal? Prevoznina { get; set; }
        public decimal? Naknada { get; set; }
        public string Icf { get; set; }

        public virtual SlogKalk SlogKalk { get; set; }
        public virtual ICollection<SlogRoba> SlogRobas { get; set; }
    }
}
