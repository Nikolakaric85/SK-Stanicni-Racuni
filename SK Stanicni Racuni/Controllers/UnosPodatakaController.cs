using AspNetCore.Reporting;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SK_Stanicni_Racuni.Models;
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

        public IActionResult PrintK165a(string id, string stanica, string blagajna, DateTime DatumDo)
        {
            var sifraStanice = context.ZsStanices.Where(x => x.Naziv == stanica).Select(x => x.SifraStanice).FirstOrDefault(); //primer 7213670
            var nazivStanice = context.ZsStanices.Where(x => x.Naziv == stanica).Select(x => x.Naziv).FirstOrDefault(); //primer 7213670

            var query = from ZsStanice in context.ZsStanices
                        join SlogKalk in context.SlogKalks
                        on ZsStanice.SifraStanice equals SlogKalk.PrStanica
                        where ((SlogKalk.Saobracaj == "1") || (SlogKalk.Saobracaj == "2"))
                        && SlogKalk.K165a == 'D'
                        && SlogKalk.Stanica == sifraStanice
                        && SlogKalk.K165a_datum == DatumDo
                        select new
                        {
                            PrStanica = SlogKalk.PrStanica,
                            Saobracaj = SlogKalk.Saobracaj,
                            Naziv = ZsStanice.Naziv,
                            PrBroj = SlogKalk.PrBroj,
                            K165a_iznos = SlogKalk.K165a_iznos,
                            PrRbb = SlogKalk.PrRbb,
                        };

            var dt = new DataTable();

            dt.Columns.Add("PrBroj");
            dt.Columns.Add("UnutrasnjiIznos");
            dt.Columns.Add("UnutrasnjiIznos_pare");
            dt.Columns.Add("MedjunarodniIznos");
            dt.Columns.Add("MedjunarodniIznos_pare");
            // Treba isproveravati ovo da li je ok sa parama 
            DataRow row;

            foreach (var item in query)
            {
                row = dt.NewRow();
                row["PrBroj"] = item.PrBroj;

                var TlSuma = item.K165a_iznos.ToString();
                string[] array = TlSuma.Split('.');

                row["UnutrasnjiIznos"] = item.Saobracaj == "1" ? array[0] : null;
                row["UnutrasnjiIznos_pare"] = item.Saobracaj == "1" ? array[1] : null;
                row["MedjunarodniIznos"] = item.Saobracaj == "2" ? array[0] : null;
                row["MedjunarodniIznos_pare"] = item.Saobracaj == "2" ? array[1] : null;

                dt.Rows.Add(row);
            }

            string mimtype = "";
            int extension = 1;

            Dictionary<string, string> paramtars = new Dictionary<string, string>();

            paramtars.Add("SifraStanice", sifraStanice);
            paramtars.Add("NazivStanice", nazivStanice);
            paramtars.Add("Blagajna", blagajna);
            paramtars.Add("DatumDo", DatumDo.ToString());

            var path = $"{this.webHostEnvironment.WebRootPath}\\Reports\\K165a.rdlc";
            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("K165a", dt);
            extension = (int)(DateTime.Now.Ticks >> 10);
            ReportResult result = localReport.Execute(RenderType.Pdf, extension, paramtars, mimtype);
            return File(result.MainStream, "application/pdf");

        }

        public IActionResult K161f()
        {
            var UserId = HttpContext.User.Identity.Name; // daje UserId
            var user = context.UserTabs.Where(x => x.UserId == UserId).FirstOrDefault();
            if (user != null)
            {
                ViewBag.Stanica = context.ZsStanices.Where(x => x.SifraStanice1 == user.Stanica).FirstOrDefault().Naziv;
            } else
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
