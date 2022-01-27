using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace SK_Stanicni_Racuni.Models
{
    public partial class SrK121a
    {
        [Key]
        public int Id { get; set; }
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
        public int RedniBroj { get; set; }
        public char? Saobracaj { get; set; }
        public DateTime? DatumVracanjaFR { get; set; }
        public decimal? ObracunFR { get; set; }
        [ScaffoldColumn(true)]
        [StringLength(4, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string BlagajnikFR { get; set; }

    }
}
