using System;
using System.Collections.Generic;

#nullable disable

namespace SK_Stanicni_Racuni.Models
{
    public partial class ZsStanice
    {
        public string SifraStanice { get; set; }
        public string Naziv { get; set; }
        public int? X { get; set; }
        public int? Y { get; set; }
        public string NadzornaStanicaSifra { get; set; }
        public string NadzornaStanicaNaziv { get; set; }
        public int? Region { get; set; }
        public int? Ztp { get; set; }
        public string KolskaVaga { get; set; }
        public int? SifraPruge { get; set; }
        public string Carina { get; set; }
        public int? SifraSekcije { get; set; }
        public int? SifraDeonice { get; set; }
        public string Kolsko { get; set; }
        public string Putnicka { get; set; }
        public string SkupPodskup { get; set; }
        public int? Blagajna { get; set; }
        public string Ekspres { get; set; }
        public string Dencano { get; set; }
        public string ZiveZivotinje { get; set; }
        public string Magacin { get; set; }
        public string ObicnaRampa { get; set; }
        public string CeonaRampa { get; set; }
        public string Dizalica { get; set; }
        public string TovarniProfil { get; set; }
        public string Prtljag { get; set; }
        public string SifraUprave { get; set; }
        public string SifraStanice1 { get; set; }
        public string Vrsta { get; set; }
        public string Prikaz { get; set; }
        public string Mesto { get; set; }

        public virtual UicStanice SifraStaniceNavigation { get; set; }
    }
}
