using System;
using System.Collections.Generic;

#nullable disable

namespace SK_Stanicni_Racuni.Models
{
    public partial class UserTab
    {
        public string UserId { get; set; }
        public string Lozinka { get; set; }
        public string Naziv { get; set; }
        public string Grupa { get; set; }
        public string Stanica { get; set; }
    }
}
