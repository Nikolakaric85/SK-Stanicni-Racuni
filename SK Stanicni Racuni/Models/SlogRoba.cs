using System;
using System.Collections.Generic;

#nullable disable

namespace SK_Stanicni_Racuni.Models
{
    public partial class SlogRoba
    {
        public int RecId { get; set; }
        public string Stanica { get; set; }
        public int KolaStavka { get; set; }
        public int RobaStavka { get; set; }
        public string OtpUprava { get; set; }
        public string OtpStanica { get; set; }
        public int OtpBroj { get; set; }
        public DateTime OtpDatum { get; set; }
        public string Nhm { get; set; }
        public string Nhmrazred { get; set; }
        public decimal? SmasaDec { get; set; }
        public int? Smasa { get; set; }
        public int? Rmasa { get; set; }
        public string Rid { get; set; }
        public string Ridklasa { get; set; }
        public string Ridrazred { get; set; }
        public decimal? VozStav { get; set; }
        public string VozStavSifra { get; set; }
        public int? PaleteR { get; set; }
        public int? PaleteBox { get; set; }
        public int? UtiTip { get; set; }
        public string UtiIb { get; set; }
        public int? UtiTara { get; set; }
        public decimal? UtiRaster { get; set; }
        public decimal? UtiNaknada { get; set; }
        public string UtiNhm { get; set; }
        public string UtiPredajniList { get; set; }
        public string UtiBrPlombe { get; set; }

        public virtual SlogKola SlogKola { get; set; }
    }
}
