using System;
using System.Collections.Generic;

#nullable disable

namespace SK_Stanicni_Racuni.Models
{
    public partial class Ugovori
    {
        public string BrojUgovora { get; set; }
        public string TipUgovora { get; set; }
        public int SifraKorisnika { get; set; }
        public string VrstaObracuna { get; set; }
        public string VrstaKursaF { get; set; }
        public string VrstaKursaT { get; set; }
        public DateTime? VaziOd { get; set; }
        public DateTime? VaziDo { get; set; }
        public string Saobracaj { get; set; }
        public DateTime? UgPath { get; set; }
        public string UserId { get; set; }
        public DateTime? DatumUnosa { get; set; }
        public string BrojUgovoraStari { get; set; }
    }
}
