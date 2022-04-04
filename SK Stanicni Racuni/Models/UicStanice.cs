using System;
using System.Collections.Generic;

#nullable disable

namespace SK_Stanicni_Racuni.Models
{
    public partial class UicStanice
    {
        public string SifraStanice { get; set; }
        public string Naziv { get; set; }
        public string Kb { get; set; }
        public string SifraUprave { get; set; }
        public string SifraStanice1 { get; set; }

        public virtual ZsStanice ZsStanice { get; set; }
    }
}
