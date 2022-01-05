using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SK_Stanicni_Racuni.Controllers
{
    public class RacuniUnutrasnjiSaobracajController : Controller
    {

        public ActionResult ListaStanica(string stanica)
        {
            List<string> lista = new List<string> { "Smederevo", "Beograd", "Niš", "Kragujevac", "Novi Sad", "Kraljevo" };

            return Json(lista);
        }


        public IActionResult RacuniUnutrasnjiSaobracaj()
        {
            return View();
        }
    }
}
