using System;
using System.Collections.Generic;

#nullable disable

namespace SK_Stanicni_Racuni.Models
{
    public partial class SrK121a
    {
        public string Stanica { get; set; }
        public int? Blagajna { get; set; }
        public int? Broj { get; set; }
        public string Pošiljalac { get; set; }
        public decimal? Iznos { get; set; }
        public int? OtpBroj { get; set; }
        public DateTime? OtpDatum { get; set; }
        public string PrStanica { get; set; }
        public string Primalac { get; set; }
        public string PrimalacAdresa { get; set; }
        public string PrimalacZemlja { get; set; }
        public DateTime? Datum { get; set; }
        public string Blagajnik { get; set; }
    }
}
