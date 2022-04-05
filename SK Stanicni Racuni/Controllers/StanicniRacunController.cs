using Microsoft.AspNetCore.Mvc;
using SK_Stanicni_Racuni.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SK_Stanicni_Racuni.Controllers
{
    public class StanicniRacunController : Controller
    {
        private readonly AppDbContext context;
        private string blagajnik = string.Empty;

        public StanicniRacunController(AppDbContext context)
        {
            this.context = context;
            
        }

        public IActionResult StanicniRacun()
        {

            var UserId = HttpContext.User.Identity.Name; // daje UserId
            var user = context.UserTabs.Where(x => x.UserId == UserId).FirstOrDefault();
            

            if (user != null)
            {
                ViewBag.blagajnik = user.Naziv;

                if (user.Stanica.StartsWith("000"))
                {
                    ViewBag.Admin = true;
                }
                else
                {
                    ViewBag.Admin = false;
                    ViewBag.Stanica = context.ZsStanices.Where(x => x.SifraStanice1 == user.Stanica).FirstOrDefault().Naziv;
                    ViewBag.SifraStanice = user.Stanica;
                }
            }
            //else
            //{
            //    return RedirectToAction("Login", "Account");
            //}

         //   ViewBag.Admin = false; // OVO OBRISATI, SETOVANO SAMO DA NE BI MORAO DA SE LOGUJEM SVAKI PUT

            ViewBag.stanicniRacuni = Enumerable.Empty<SrFaktura>();
            return View();
        }

        public IActionResult Prikazi(string stanica, int racunBr)
        {
            var sifraStanice = context.ZsStanices.Where(x => x.Naziv == stanica).Select(x => x.SifraStanice).FirstOrDefault();

            ViewBag.blagajnik = blagajnik;

            if (!string.IsNullOrEmpty(stanica) && racunBr != 0)
            {
                //ovde ide PDF izvestaj
            }
            else if (!string.IsNullOrEmpty(stanica) && racunBr == 0)
            {

                var query = context.SrFakturas.Where(x => x.Stanica == sifraStanice).Select(x => new { x.Stanica, x.FakturaBroj}).AsEnumerable();
              //  ViewBag.stanicniRacuni = query;
                return View("StanicniRacun");

                // ovde ide popunjavanje GRID-a faktura broj je racun broj
            }

            return RedirectToAction("StanicniRacun");
        }


        public IActionResult Save(SrFaktura model)
        {

            context.SrFakturas.Add(model);
            //context.SaveChanges();

            return RedirectToAction("StanicniRacun");
        }


    }
}
