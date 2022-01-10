using AspNetCore.Reporting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SK_Stanicni_Racuni.CustomModelBinding.RacuniUnutrasnjiSaobracaj;
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
        private ReportResult result; // za pdf izvestaj

        public MagacinskaKnjigaController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult MagacinskaKnjiga(string id)
        {
            ViewBag.Id = id;
            return View();
        }

        /***************** PRINT PDF DOKUMENT ******************/

        public IActionResult Print(string id, string stanica, [ModelBinder(typeof(DatumOdModelBinder))] DateTime DatumOd,
         [ModelBinder(typeof(DatumDoModelBinder))] DateTime DatumDo)
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
                    row["PredatoDana"] = item.Predato_dana;
                    row["UputnaStanica"] = item.Uputna_stanica;
                    row["BrojKola"] = item.Broj_Kola;
                    row["NaznacenjeVrsteRobe"] = item.Naznačenje_vrste_robe;
                    row["NavedenoUtovarnomListu"] = item.Navedena_u_tovarnom_listu;
                    row["IBK"] = item.IBK;
                    dt.Rows.Add(row);
                }

                string mimtype = "";
                int extension = 1;

                Dictionary<string, string> paramtars = new Dictionary<string, string>();

                paramtars.Add("Stanica", sifraStanice);
                paramtars.Add("DatumOd", DatumOd.ToString());
                paramtars.Add("DatumDo", DatumDo.ToString());

                var path = $"{this.webHostEnvironment.WebRootPath}\\Reports\\K117.rdlc";
                LocalReport localReport = new LocalReport(path);
                localReport.AddDataSource("K117", dt);

                result = localReport.Execute(RenderType.Pdf, extension, paramtars, mimtype);

              //  return File(result.MainStream, "application/pdf");
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

                string mimtype = "";
                int extension = 1;

                Dictionary<string, string> paramtars = new Dictionary<string, string>();

                paramtars.Add("Stanica", sifraStanice);
                paramtars.Add("DatumOd", DatumOd.ToString());
                paramtars.Add("DatumDo", DatumDo.ToString());

                var path = $"{this.webHostEnvironment.WebRootPath}\\Reports\\K254.rdlc";
                LocalReport localReport = new LocalReport(path);
                localReport.AddDataSource("K254", dt);

                result = localReport.Execute(RenderType.Pdf, extension, paramtars, mimtype);
            }

            return File(result.MainStream, "application/pdf");

        }



    }
}
