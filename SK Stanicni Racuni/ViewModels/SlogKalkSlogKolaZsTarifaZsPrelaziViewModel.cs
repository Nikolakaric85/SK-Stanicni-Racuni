using SK_Stanicni_Racuni.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SK_Stanicni_Racuni.ViewModels
{
    public class SlogKalkSlogKolaZsTarifaZsPrelaziViewModel
    {
        [NotMapped]
        public SlogKalk SlogKalk { get; set; }
        [NotMapped]
        public SlogKola SlogKola { get; set; }
        [NotMapped]
        public ZsTarifa ZsTarifa { get; set; }
        [NotMapped]
        public ZsPrelazi ZsPrelazi { get; set; }
    }
}
