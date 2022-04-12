using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SK_Stanicni_Racuni.ViewModels
{
    public class SrFakturaViewModel
    {
        public string Stanica { get; set; }
        public int? Blagajna { get; set; }
        public string BlagajnaTip { get; set; }
        public string TekuciRacun { get; set; }
        public string FakturaBroj { get; set; }
        public int FakturaGodina { get; set; }
        public int? OtpBroj { get; set; }
        public DateTime? OtpDatum { get; set; }
        public int? PrBroj { get; set; }
        public DateTime? PrDatum { get; set; }
        public DateTime? FakturaDatum { get; set; }
        public DateTime? DatumIzdavanja { get; set; }
        public string Primalac { get; set; }
        public string PrimalacUg { get; set; }
        public string PrimalacAdresa { get; set; }
        public string PrimalacZemlja { get; set; }
        public string PrimalacTelefon { get; set; }
        public string PrimalacTr { get; set; }
        public string PrimalacMb { get; set; }
        public string PrimalacPib { get; set; }
        public string VrstaUslugaSifra { get; set; }
        public string VrstaUslugaOpis { get; set; }
        public DateTime? FakturaDatumPromet { get; set; }
        public DateTime? FakturaDatumP { get; set; }
        public string Fjedinica { get; set; }
        public string Fkolicina { get; set; }
        public decimal? Fjcena { get; set; }
        public decimal? FakturaOsnovica { get; set; }
        public decimal? FakturaPdv { get; set; }
        public decimal? FakturaTotal { get; set; }
        public string Fopdv { get; set; }
        public string Fopdvcst { get; set; }
        public string FakturaTekst { get; set; }
        public string FpozivNaBroj { get; set; }
        public decimal? Kurs { get; set; }
        public string Fpath { get; set; }
        public string Blagajnik { get; set; }
        public int Id { get; set; }
        public string Realizovano { get; set; }
        public DateTime? DatumR { get; set; }

        //******** DODATO

        public string NazivStanice { get; set; }

        //za upload
        public IFormFile UploadFile { get; set; }
    }
}
