using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using SK_Stanicni_Racuni.CustomModelBinding.Datumi;
using SK_Stanicni_Racuni.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SK_Stanicni_Racuni.Controllers
{
    public class MagacinskaKnjigaController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;
        // private ReportResult TopResult; // za pdf izvestaj

        public MagacinskaKnjigaController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        public ActionResult ListaStanica()
        {
            var stanice = context.ZsStanices.Select(x => x.Naziv);
            return Json(stanice);
        }

        public IActionResult MagacinskaKnjiga(string id)
        {
            ViewBag.Id = id;
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



        /***************** PRINT PDF DOKUMENT ******************/

        public IActionResult Print(string id, string stanica, [ModelBinder(typeof(DatumOdModelBinder))] DateTime DatumOd, [ModelBinder(typeof(DatumDoModelBinder))] DateTime DatumDo)
        {

            var sifraStanice = context.ZsStanices.Where(x => x.Naziv == stanica).Select(x => x.SifraStanice).FirstOrDefault();

            if (id == "K117")
            {
                var query = from kalk in context.SlogKalks
                            join kola in context.SlogKolas on kalk.RecId equals kola.RecId
                            join roba in context.SlogRobas on new { RecId = kola.RecId, KolaStavka = kola.KolaStavka } equals
                                                              new { RecId = roba.RecId, KolaStavka = roba.KolaStavka }
                            where (kalk.OtpDatum >= DatumOd && kalk.OtpDatum <= DatumDo)
                            && kalk.OtpStanica == sifraStanice
                            && (kalk.Saobracaj == "1" || kalk.Saobracaj == "3")
                            select new
                            {
                                Broj_tovarnog_lista = kalk.OtpBroj,
                                Predato_dana = kalk.OtpDatum,
                                Uputna_stanica = kalk.PrStanica,
                                Broj_Kola = kalk.UkupnoKola,
                                Naznačenje_vrste_robe = roba.Nhm,
                                Navedena_u_tovarnom_listu = roba.Smasa,
                                IBK = kola.Ibk
                            };

                var dt = new DataTable();

                dt.Columns.Add("BrojTovarnogLista");
                dt.Columns.Add("PredatoDana");
                dt.Columns.Add("UputnaStanica");
                dt.Columns.Add("BrojKola");
                dt.Columns.Add("NaznacenjeVrsteRobe");
                dt.Columns.Add("NavedenoUtovarnomListu");
                dt.Columns.Add("IBK");

                DataRow row;

                foreach (var item in query)
                {
                    row = dt.NewRow();
                    row["BrojTovarnogLista"] = item.Broj_tovarnog_lista;
                    row["PredatoDana"] = item.Predato_dana.ToString("dd.MM.yyyy");
                    row["UputnaStanica"] = item.Uputna_stanica;
                    row["BrojKola"] = item.Broj_Kola;
                    row["NaznacenjeVrsteRobe"] = item.Naznačenje_vrste_robe;
                    row["NavedenoUtovarnomListu"] = item.Navedena_u_tovarnom_listu;
                    row["IBK"] = item.IBK;
                    dt.Rows.Add(row);
                }

                string renderFormat = "PDF";
                string mimtype = "application/pdf";

                var localReport = new LocalReport();
                localReport.ReportPath = $"{this.webHostEnvironment.WebRootPath}\\Reports\\K117.rdlc";
                localReport.DataSources.Add(new ReportDataSource("K117", dt));
                var parametars = new[]
                {
                    new ReportParameter("Stanica",sifraStanice),
                    new ReportParameter("DatumOd",DatumOd.ToString()),
                    new ReportParameter("DatumDo", DatumDo.ToString())
                };

                localReport.SetParameters(parametars);
                var pdf = localReport.Render(renderFormat);
                return File(pdf, mimtype);


            }
            else if (id == "K254")
            {
                var query = from kalk in context.SlogKalks
                            join kola in context.SlogKolas on kalk.RecId equals kola.RecId
                            join roba in context.SlogRobas on new { RecId = kola.RecId, KolaStavka = kola.KolaStavka } equals
                                                              new { RecId = roba.RecId, KolaStavka = roba.KolaStavka }
                            where (kalk.OtpDatum >= DatumOd && kalk.OtpDatum <= DatumDo)
                            && kalk.OtpStanica == sifraStanice
                            && (kalk.Saobracaj == "1" || kalk.Saobracaj == "2")
                            select new
                            {
                                Broj_Prispeca = kalk.PrBroj,
                                Otpravna_stanica = kalk.OtpStanica,
                                Broj_tovarnog_lista = kalk.OtpBroj,
                                Broj_Kola = kalk.UkupnoKola,
                                Naznačenje_vrste_robe = roba.Nhm,
                                Masa_u_kg = roba.Smasa,
                                IBK = kola.Ibk
                            };

                var dt = new DataTable();

                dt.Columns.Add("BrojPrispeca");
                dt.Columns.Add("OtpravnaStanica");
                dt.Columns.Add("BrojTovarnogLista");
                dt.Columns.Add("BrojKola");
                dt.Columns.Add("NaznacenjeVrsteRobe");
                dt.Columns.Add("MasaUkg");
                dt.Columns.Add("IBK");

                DataRow row;

                foreach (var item in query)
                {
                    row = dt.NewRow();
                    row["BrojPrispeca"] = item.Broj_Prispeca;
                    row["OtpravnaStanica"] = item.Otpravna_stanica;
                    row["BrojTovarnogLista"] = item.Broj_tovarnog_lista;
                    row["BrojKola"] = item.Broj_Kola;
                    row["NaznacenjeVrsteRobe"] = item.Naznačenje_vrste_robe;
                    row["MasaUkg"] = item.Masa_u_kg;
                    row["IBK"] = item.IBK;
                    dt.Rows.Add(row);
                }

                string renderFormat = "PDF";
                string mimtype = "application/pdf";

                var localReport = new LocalReport();
                localReport.ReportPath = $"{this.webHostEnvironment.WebRootPath}\\Reports\\K254.rdlc";
                localReport.DataSources.Add(new ReportDataSource("K254", dt));
                var parametars = new[]
                {
                    new ReportParameter("Stanica",sifraStanice),
                    new ReportParameter("DatumOd",DatumOd.ToString()),
                    new ReportParameter("DatumDo", DatumDo.ToString())
                };

                localReport.SetParameters(parametars);
                var pdf = localReport.Render(renderFormat);
                return File(pdf, mimtype);
            }


            return View();

        }

    }
}

