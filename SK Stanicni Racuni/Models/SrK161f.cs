using System;
using System.Collections.Generic;

#nullable disable

namespace SK_Stanicni_Racuni.Models
{
    public partial class SrK161f
    {
        public string Stanica { get; set; }
        public int? Blagajna { get; set; }
        public string BlagajnaTip { get; set; }
        public string FakturaBroj { get; set; }
        public DateTime? FakturaDatum { get; set; }
        public decimal? FakturaOsnovica { get; set; }
        public decimal? FakturaPdv { get; set; }
        public string Primalac { get; set; }
        public string PrimalacAdresa { get; set; }
        public string PrimalacZemlja { get; set; }
        public string PrimalacTelefon { get; set; }
        public string PrimalacTr { get; set; }
        public string PrimalacMb { get; set; }
        public string PrimalacPib { get; set; }
        public string VrstaUslugaSifra { get; set; }
        public string VrstaUslugaOpis { get; set; }
        public string Blagajnik { get; set; }
    }
}
