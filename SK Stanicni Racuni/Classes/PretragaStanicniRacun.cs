using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SK_Stanicni_Racuni.Classes
{
    public class PretragaStanicniRacun
    {
        public string Stanica { get; }
        public string RacunBr { get; }
        public int FakturaGod { get; }
        public bool SveFakture { get; }
        public bool SamoNfakture { get; }
        public PretragaStanicniRacun(string stanica, string racunBr, int fakturaGod, bool sveFakture, bool samoNfakture)
        {
            Stanica = stanica;
            RacunBr = racunBr;
            FakturaGod = fakturaGod;
            SveFakture = sveFakture;
            SamoNfakture = samoNfakture;
        }



    
    }
}
