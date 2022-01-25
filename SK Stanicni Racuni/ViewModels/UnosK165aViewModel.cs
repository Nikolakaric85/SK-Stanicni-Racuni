using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SK_Stanicni_Racuni.ViewModels
{
    public class UnosK165aViewModel
    {
        public DateTime? K165a_datum { get; set; }
        public char K165a { get; set; }
        public decimal K165a_iznos { get; set; }
        public int? PrBroj { get; set; }
        public string PrStanica { get; set; }
    }
}
