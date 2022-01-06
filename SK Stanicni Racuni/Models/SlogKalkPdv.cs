using System;
using System.Collections.Generic;

#nullable disable

namespace SK_Stanicni_Racuni.Models
{
    public partial class SlogKalkPdv
    {
        public int RecId { get; set; }
        public string Stanica { get; set; }
        public decimal? Pdv1 { get; set; }
        public decimal? Pdv2 { get; set; }
    }
}
