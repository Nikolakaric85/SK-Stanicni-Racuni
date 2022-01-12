using System;
using System.Collections.Generic;

#nullable disable

namespace SK_Stanicni_Racuni.Models
{
    public partial class ZsTarifa
    {
        public string SifraTarife { get; set; }
        public string TarifaStavka { get; set; }
        public int SifraVs { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public DateTime? VaziOd { get; set; }
        public DateTime? VaziDo { get; set; }
    }
}
