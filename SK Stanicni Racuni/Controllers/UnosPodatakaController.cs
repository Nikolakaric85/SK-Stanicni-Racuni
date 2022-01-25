using AspNetCore.Reporting;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SK_Stanicni_Racuni.Models;
using SK_Stanicni_Racuni.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SK_Stanicni_Racuni.Controllers
{
    public class UnosPodatakaController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly INotyfService notyf;

        public UnosPodatakaController(AppDbContext context, IWebHostEnvironment webHostEnvironment, INotyfService notyf)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
            this.notyf = notyf;
        }

        public IActionResult K165a()
        {
            var UserId = HttpContext.User.Identity.Name; // daje UserId
            var user = context.UserTabs.Where(x => x.UserId == UserId).FirstOrDefault();

            if (user != null)
            {
                if (user.Stanica.StartsWith("000"))
                {
                    ViewBag.Admin = true;
                }
                else
                {
                    ViewBag.Admin = false;
                    ViewBag.Stanica = context.ZsStanices.Where(x => x.SifraStanice1 == user.Stanica).FirstOrDefault().Naziv;
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        public IActionResult Pretraga(string stanica, string prBroj)
        {
            var UserId = HttpContext.User.Identity.Name; // daje UserId
            var user = context.UserTabs.Where(x => x.UserId == UserId).FirstOrDefault();
            var prStanica = context.ZsStanices.Where(x => x.SifraStanice1 == user.Stanica).FirstOrDefault().SifraStanice;
            ViewBag.Stanica = stanica;

            var query = from sk in context.SlogKalks
                        where (sk.Saobracaj == "1" || sk.Saobracaj == "2") &&
                        sk.PrBroj == Int32.Parse(prBroj) &&
                        sk.PrStanica == prStanica

                        select new { sk.K165a, sk.K165a_iznos, sk.K165a_datum };

            var viewModel = new UnosK165aViewModel();

            List<string> k165aList = new List<string>() { " ", "N", "D" };

            try
            {
                if (query.Any())
                {
                    ViewBag.K165a = true;
                    foreach (var item in query)
                    {

                        if (item.K165a == '\0')
                        {
                            ViewBag.k165aList = new SelectList(k165aList, k165aList[0]);
                        }
                        else if (item.K165a == 'N')
                        {
                            ViewBag.k165aList = new SelectList(k165aList, k165aList[1]);
                        }
                        else if (item.K165a == 'D')
                        {
                            ViewBag.k165aList = new SelectList(k165aList, k165aList[2]);
                        }

                        viewModel.K165a_iznos = item.K165a_iznos;
                        viewModel.K165a_datum = item.K165a_datum;
                        viewModel.PrBroj = Int32.Parse(prBroj);
                        viewModel.PrStanica = prStanica;
                    }
                }
                else
                {
                    ViewBag.K165a = false;
                }

            }
            catch (Exception)
            {

                ViewBag.K165a = false;
            }

            return View("K165a", viewModel);
        }

        public IActionResult K165a_save(UnosK165aViewModel viewModel, string k165aDatum)
        {
            var model = context.SlogKalks.Where(x => x.PrBroj == viewModel.PrBroj && x.PrStanica == viewModel.PrStanica).FirstOrDefault();

            var newModel = new SlogKalk();

            newModel = model;
            newModel.K165a = viewModel.K165a;
            newModel.K165a_iznos = viewModel.K165a_iznos;
            _ = k165aDatum != null ? newModel.K165a_datum = DateTime.Parse(k165aDatum) : newModel.K165a_datum = model.K165a_datum;
            
            try
            {
                var update = context.SlogKalks.Attach(newModel);
                update.State = Microsoft.EntityFrameworkCore.EntityState.Modified;                                      //*********************PRIMERY KEY *********************///
                context.SaveChanges();
                notyf.Success("Uspeša izmena podataka.",3);
            }
            catch (Exception)
            {
                notyf.Error("Greška prilikom izmene podataka.", 3);
            }

            return View("K165a");
        }

    

        public IActionResult K161f()
        {
            var UserId = HttpContext.User.Identity.Name; // daje UserId
            var user = context.UserTabs.Where(x => x.UserId == UserId).FirstOrDefault();
            if (user != null)
            {
                ViewBag.Stanica = context.ZsStanices.Where(x => x.SifraStanice1 == user.Stanica).FirstOrDefault().Naziv;
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            ViewBag.k161f = context.SrK161fs.Where(x => x.FakturaDatum >= firstDayOfMonth && x.FakturaDatum <= lastDayOfMonth).AsEnumerable();

            return View();
        }

        //***********************  IMA JOS POSLA DA SE OGRANICI UNOS PODATAKA NA FORMI *******************************************************************************
        public IActionResult K161F_save(SrK161f model, string datumOd, string datumDo, char naplacenoCheckBox = 'N')
        {
            var newModel = new SrK161f();
            newModel.Stanica = model.Stanica;
            newModel.Blagajna = model.Blagajna;
            newModel.FakturaBroj = model.FakturaBroj;

            if (datumOd != null)
            {
                newModel.FakturaDatum = DateTime.Parse(datumOd);
            }
            if (datumDo != null)
            {
                newModel.FakturaDatumP = DateTime.Parse(datumDo);
            }

            newModel.VrstaUslugaSifra = model.VrstaUslugaSifra;
            newModel.VrstaUslugaOpis = model.VrstaUslugaOpis;
            newModel.Kurs = model.Kurs;
            newModel.FakturaOsnovica = model.FakturaOsnovica;
            newModel.FakturaPdv = model.FakturaPdv;
            newModel.NaplacenoNB = naplacenoCheckBox;
            newModel.Primalac = model.Primalac;
            newModel.PrimalacAdresa = model.PrimalacAdresa;
            newModel.PrimalacZemlja = model.PrimalacZemlja;
            newModel.PrimalacTr = model.PrimalacTr;
            newModel.PrimalacMb = model.PrimalacMb;
            newModel.PrimalacPib = model.PrimalacPib;

            try
            {
                context.SrK161fs.Add(newModel);
                context.SaveChanges();
                notyf.Success("Uspešan unos podataka", 3);
            }
            catch (Exception)
            {
                notyf.Error("Greška prilikom upisa u bazu.", 3);
            }

            return View("K161f");
        }

        public IActionResult K121a()
        {
            var UserId = HttpContext.User.Identity.Name; // daje UserId
            var user = context.UserTabs.Where(x => x.UserId == UserId).FirstOrDefault();
            if (user != null)
            {
                ViewBag.UserId = UserId;
                ViewBag.Stanica = context.ZsStanices.Where(x => x.SifraStanice1 == user.Stanica).FirstOrDefault().SifraStanice;
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            ViewBag.k121a = context.SrK121as.Where(x => x.Datum >= firstDayOfMonth && x.Datum <= lastDayOfMonth).AsEnumerable();


            return View();
        }

        public IActionResult K121a_save(SrK121a model, string otpDatum, string datum, string stanica_)
        {
            var newModel = new SrK121a();

            newModel.Broj = model.Broj;
            newModel.Pošiljalac = model.Pošiljalac;
            newModel.Iznos = model.Iznos;
            newModel.OtpBroj = model.OtpBroj;

            if (otpDatum != null)
            {
                newModel.OtpDatum = DateTime.Parse(otpDatum);
            }

            if (!string.IsNullOrEmpty(stanica_))
            {
                newModel.PrStanica = context.ZsStanices.Where(x => x.Naziv == stanica_).Select(x => x.SifraStanice).FirstOrDefault();
            }

            if (datum != null)
            {
                newModel.Datum = DateTime.Parse(datum);
            }

            newModel.Primalac = model.Primalac;
            newModel.PrimalacAdresa = model.PrimalacAdresa;
            newModel.PrimalacZemlja = model.PrimalacZemlja;
            newModel.Blagajnik = model.Blagajnik;
            newModel.Stanica = model.Stanica;

            try
            {
                context.SrK121as.Add(newModel);
                context.SaveChanges();
                notyf.Success("Uspešan unos podataka", 3);
            }
            catch (Exception)
            {
                notyf.Error("Greška prilikom upisa u bazu.", 3);
            }

            return RedirectToAction("K121a");
        }


    }
}
