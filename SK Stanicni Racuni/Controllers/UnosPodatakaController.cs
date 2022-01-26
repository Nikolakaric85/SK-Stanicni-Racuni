﻿using AspNetCore.Reporting;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SK_Stanicni_Racuni.CustomModelBinding.Datumi;
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

            List<string> k165aList = new List<string>() { " ", "Ne iskupljen", "Iskupljen" };

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
                    notyf.Information("Uneti broj prispeća ne postoji u bazi.",3);
                    ViewBag.K165a = false;
                }

            }
            catch (Exception)
            {

                ViewBag.K165a = false;
            }

            return View("K165a", viewModel);
        }

        public IActionResult K165a_save(UnosK165aViewModel viewModel, [ModelBinder(typeof(DatumDoModelBinder))] DateTime datumDo, string k165a )
        {
            var model = context.SlogKalks.Where(x => x.PrBroj == viewModel.PrBroj && x.PrStanica == viewModel.PrStanica).FirstOrDefault();
            var newModel = new SlogKalk();

            newModel = model;

            if (k165a == null)
            {
                newModel.K165a = ' ';
            } else if (k165a == "Iskupljen")
            {
                newModel.K165a = 'D';
            } else if(k165a == "Ne iskupljen")
            {
                newModel.K165a = 'N';
            }

            newModel.K165a_iznos = viewModel.K165a_iznos;
            _ = datumDo.ToString() != "1/1/0001 12:00:00 AM" ? newModel.K165a_datum = datumDo : newModel.K165a_datum = model.K165a_datum;
            
            try
            {
                var update = context.SlogKalks.Attach(newModel);
                update.State = Microsoft.EntityFrameworkCore.EntityState.Modified;                                      //*********************  PRIMERY KEY koji   *********************///
                context.SaveChanges();
                notyf.Success("Uspeša izmena podataka.",3);
            }
            catch (Exception)
            {
                notyf.Error("Greška prilikom izmene podataka.", 3);
            }

            return RedirectToAction("K165a");
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
        public IActionResult K161F_save([ModelBinder(typeof(DatumOdModelBinder))] DateTime DatumOd, [ModelBinder(typeof(DatumDoModelBinder))] DateTime DatumDo, 
            SrK161f model, char naplacenoCheckBox = 'N')
        {
            var newModel = new SrK161f();

            var sifraStanice = context.ZsStanices.Where(x => x.Naziv == model.Stanica).FirstOrDefault();
            _ = sifraStanice != null ? newModel.Stanica = sifraStanice.SifraStanice : model.Stanica = null;
            newModel.Blagajna = model.Blagajna;
            newModel.FakturaBroj = model.FakturaBroj;

            if (DatumOd.ToString() != "1/1/0001 12:00:00 AM")
            {
                newModel.FakturaDatum = DatumOd;
            }
            if (DatumDo.ToString() != "1/1/0001 12:00:00 AM")
            {
                newModel.FakturaDatumP = DatumDo;
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

            return RedirectToAction ("K161f");
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

        public IActionResult K121a_save(SrK121a model, 
            [ModelBinder(typeof(DatumOdModelBinder))] DateTime DatumOd, [ModelBinder(typeof(DatumDoModelBinder))] DateTime DatumDo, string stanica_)      //otpDatum je DatumOd, datum je DatumDo
        {
            var newModel = new SrK121a();

            newModel.Broj = model.Broj;
            newModel.Pošiljalac = model.Pošiljalac;
            newModel.Iznos = model.Iznos;
            newModel.OtpBroj = model.OtpBroj;

            if (DatumOd.ToString() != "1/1/0001 12:00:00 AM")
            {
                newModel.OtpDatum = DatumOd;
            }

            if (!string.IsNullOrEmpty(stanica_))
            {
                newModel.PrStanica = context.ZsStanices.Where(x => x.Naziv == stanica_).Select(x => x.SifraStanice).FirstOrDefault();
            }

            if (DatumDo.ToString() != "1/1/0001 12:00:00 AM")
            {
                newModel.Datum = DatumDo;
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
