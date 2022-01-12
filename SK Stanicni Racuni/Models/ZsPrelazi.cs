using System;
using System.Collections.Generic;

#nullable disable

namespace SK_Stanicni_Racuni.Models
{
    public partial class ZsPrelazi
    {
        public string SifraPrelaza { get; set; }
        public string Naziv { get; set; }
        public string SifraPrelaza4 { get; set; }
        public string GranicnaUprava { get; set; }
        public string SifraCarina { get; set; }
        public int? Carina { get; set; }
    }
}
