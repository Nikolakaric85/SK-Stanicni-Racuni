using System;
using System.Collections.Generic;

#nullable disable

namespace SK_Stanicni_Racuni.Models
{
    public partial class SlogKalk
    {
        public SlogKalk()
        {
            SlogKolas = new HashSet<SlogKola>();
        }

        public int RecId { get; set; }
        public string Stanica { get; set; }
        public string OtpUprava { get; set; }
        public string OtpStanica { get; set; }
        public int? OtpRbb { get; set; }
        public int OtpBroj { get; set; }
        public DateTime OtpDatum { get; set; }
        public string ObrGodina { get; set; }
        public string ObrMesec { get; set; }
        public string ZsUlPrelaz { get; set; }
        public int? UlEtiketa { get; set; }
        public DateTime? DatumUlaza { get; set; }
        public string ZsIzPrelaz { get; set; }
        public int? IzEtiketa { get; set; }
        public DateTime? DatumIzlaza { get; set; }
        public string PrUprava { get; set; }
        public string PrStanica { get; set; }
        public int? PrRbb { get; set; }
        public int? PrBroj { get; set; }
        public DateTime? PrDatum { get; set; }
        public int? BrojVoza { get; set; }
        public string SatVoza { get; set; }
        public string SatVoza2 { get; set; }
        public int? BrojVoza2 { get; set; }
        public string Najava { get; set; }
        public int? NajavaKola { get; set; }
        public string Najava2 { get; set; }
        public string SifraTarife { get; set; }
        public string Ugovor { get; set; }
        public int? Posiljalac { get; set; }
        public int? PlatilacFrr { get; set; }
        public int? Primalac { get; set; }
        public int? PlatilacNfr { get; set; }
        public string VrPos { get; set; }
        public string Prevoz { get; set; }
        public string Saobracaj { get; set; }
        public int? VrSao { get; set; }
        public string VrPrevoza { get; set; }
        public string NarPos { get; set; }
        public int? UkupnoKola { get; set; }
        public int? TkmZs { get; set; }
        public int? SkmZs { get; set; }
        public int? TkmR { get; set; }
        public int? SkmR { get; set; }
        public string StanicaRee { get; set; }
        public int? SifraIzjave { get; set; }
        public string FrNaknade { get; set; }
        public string FrDoPrelaza { get; set; }
        public string Incoterms { get; set; }
        public string IzjavaVal { get; set; }
        public decimal? IzjavaIznos { get; set; }
        public decimal? OstatakDin { get; set; }
        public string IsporukaVal { get; set; }
        public decimal? Isporuka { get; set; }
        public string PouzeceVal { get; set; }
        public decimal? Pouzece { get; set; }
        public string VrednostRobeVal { get; set; }
        public decimal? VrednostRobe { get; set; }
        public string FrRacun { get; set; }
        public int? ProcenatTarifa { get; set; }
        public int? ProcenatUg { get; set; }
        public decimal? DodPrev { get; set; }
        public string TlValuta { get; set; }
        public decimal? TlSumaFr { get; set; }
        public decimal? TlSumaUp { get; set; }
        public decimal? TlPrevFr { get; set; }
        public decimal? TlPrevUp { get; set; }
        public decimal? TlNakFr { get; set; }
        public decimal? TlNakUp { get; set; }
        public decimal? TlSumaFrDin { get; set; }
        public decimal? TlSumaUpDin { get; set; }
        public decimal? TlPrevFrDin { get; set; }
        public decimal? TlPrevUpDin { get; set; }
        public decimal? TlNakFrDin { get; set; }
        public decimal? TlNakUpDin { get; set; }
        public decimal? TlNakCo82 { get; set; }
        public decimal? TlNakCo { get; set; }
        public decimal? RSumaFr { get; set; }
        public decimal? RSumaUp { get; set; }
        public decimal? RPrevFr { get; set; }
        public decimal? RPrevUp { get; set; }
        public decimal? RNakFr { get; set; }
        public decimal? RNakUp { get; set; }
        public decimal? RSumaFrDin { get; set; }
        public decimal? RSumaUpDin { get; set; }
        public decimal? RPrevFrDin { get; set; }
        public decimal? RPrevUpDin { get; set; }
        public decimal? RNakFrDin { get; set; }
        public decimal? RNakUpDin { get; set; }
        public decimal? K165aIznos { get; set; }
        public DateTime? K165aDatum { get; set; }
        public string K165a { get; set; }
        public string TlRealizovan { get; set; }
        public string SifraK211 { get; set; }
        public string Obrada { get; set; }
        public string Referent1 { get; set; }
        public string Referent2 { get; set; }
        public DateTime? DatumObrade { get; set; }
        public string CarJci { get; set; }
        public string CarStanica { get; set; }
        public string CarStanica2 { get; set; }
        public string CarPrimalacPib { get; set; }
        public string CarPrimalacNaziv { get; set; }
        public string CarPrimalacSediste { get; set; }
        public string CarPrimalacZemlja { get; set; }
        public string CarValuta { get; set; }
        public decimal? CarFaktura { get; set; }
        public string CarBrojFakture { get; set; }
        public string CarDokumenti { get; set; }
        public string CarKnjiga { get; set; }
        public DateTime? CarDatum { get; set; }
        public DateTime? CarRokCarinjenja { get; set; }
        public string CarPostupakEnd { get; set; }
        public string CarAgent { get; set; }
        public string CarXml { get; set; }
        public string Server { get; set; }

        public virtual ICollection<SlogKola> SlogKolas { get; set; }
    }
}
