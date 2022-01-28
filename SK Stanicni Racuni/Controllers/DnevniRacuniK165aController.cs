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
    public class DnevniRacuniK165aController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public DnevniRacuniK165aController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult DnevniRacuniK165a()
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



        public IActionResult Print(string id, string stanica, string blagajna, [ModelBinder(typeof(DatumDoModelBinder))] DateTime Datum)
        {
            var sifraStanice = context.ZsStanices.Where(x => x.Naziv == stanica).Select(x => x.SifraStanice).FirstOrDefault(); //primer 7213670
            var nazivStanice = context.ZsStanices.Where(x => x.Naziv == stanica).Select(x => x.Naziv).FirstOrDefault(); //primer 7213670

            var query = from ZsStanice in context.ZsStanices
                        join SlogKalk in context.SlogKalks
                        on ZsStanice.SifraStanice equals SlogKalk.PrStanica
                        where ((SlogKalk.Saobracaj == "1") || (SlogKalk.Saobracaj == "2"))
                        && SlogKalk.K165a == 'D'
                        && SlogKalk.PrStanica == sifraStanice
                        && SlogKalk.K165a_datum == Datum
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

            string renderFormat = "PDF";
            string mimtype = "application/pdf";

            var localReport = new LocalReport();
            localReport.ReportPath = $"{this.webHostEnvironment.WebRootPath}\\Reports\\K165a.rdlc";
            localReport.DataSources.Add(new ReportDataSource("K165a", dt));
            var parametars = new[]
            {
                    new ReportParameter("SifraStanice", sifraStanice),
                    new ReportParameter("NazivStanice", nazivStanice),
                    new ReportParameter("Blagajna", blagajna),
                    new ReportParameter("DatumDo", Datum.ToString())
            };

            localReport.SetParameters(parametars);
            var pdf = localReport.Render(renderFormat);
            return File(pdf, mimtype);

        }

    }
}
