using Microsoft.AspNetCore.Mvc;
using SK_Stanicni_Racuni.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SK_Stanicni_Racuni.Controllers
{
    public class ListaStanicaController : Controller
    {
        private readonly AppDbContext context;

        public ListaStanicaController(AppDbContext context)
        {
            this.context = context;
        }



        public IActionResult ListaMedjunarodnihStanica()
        {
            var stanice = (from s in context.UicStanices
                           select s.SifraStanice).ToList().Take(10);

            return Json(stanice);
        }

        public ActionResult ListaUnutrasnjihStanica(string data)
        {
            var stanice = (from s in context.ZsStanices
                           where s.Naziv.Contains(data)
                           select s.Naziv).ToList().Take(10);

            return Json(stanice);
        }

        public ActionResult ListaUnutrasnjihStanicaStanicniRacun(string data)
        {
            var stanice = (from s in context.ZsStanices
                           where s.Naziv.Contains(data) && s.Prikaz == "D"
                           select s.Naziv).ToList().Take(10);

            return Json(stanice);
        }


        public ActionResult KomintentIUgovor(string data)
        {
            var query = from k in context.Komitents
                        join u in context.Ugovoris on k.Sifra equals u.SifraKorisnika
                        where u.VaziDo >= DateTime.Now
                        && k.Naziv.Contains(data)
                        select new
                        {
                            Naziv = k.Naziv.Trim(),
                            Mesto = k.Mesto.Trim(), 
                            Zemlja = k.Zemlja.Trim(),
                            Adresa = k.Adresa.Trim(),
                            Telefon = k.Telefon.Trim(),
                            Pib = k.Pib.Trim(), 
                            Mb = k.Mb.Trim(),
                            Tr = k.Tr.Trim(),
                            BrojUgovora = u.BrojUgovora.Trim()
                        };

            var res = from element in query.ToList()
                      group element by element.Naziv
                      into groups
                      select groups.OrderBy(p => p.BrojUgovora).First();

            return Json(res);
        }
    }
}
