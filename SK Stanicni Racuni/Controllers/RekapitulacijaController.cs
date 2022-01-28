﻿
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using SK_Stanicni_Racuni.CustomModelBinding.Datumi;
using SK_Stanicni_Racuni.Models;
using System;
using System.Data;
using System.Linq;


namespace SK_Stanicni_Racuni.Controllers
{
    public class RekapitulacijaController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly INotyfService notyf;

        public RekapitulacijaController(AppDbContext context, IWebHostEnvironment webHostEnvironment, INotyfService notyf)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
            this.notyf = notyf;
        }

        public IActionResult K111f()
        {
            var UserId = HttpContext.User.Identity.Name; // daje UserId
            var user = context.UserTabs.Where(x => x.UserId == UserId).FirstOrDefault();
            if (user != null)
            {
                ViewBag.UserId = UserId;
                ViewBag.Stanica = context.ZsStanices.Where(x => x.SifraStanice1 == user.Stanica).FirstOrDefault().Naziv;
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        public IActionResult Print_K111f([ModelBinder(typeof(DatumOdModelBinder))] DateTime DatumOd, [ModelBinder(typeof(DatumDoModelBinder))] DateTime DatumDo,
            string blagajna, string stanica)
        {

            var sifraStanice = context.ZsStanices.Where(x => x.Naziv == stanica).FirstOrDefault();

            var query = from s in context.SrK161fs
                        where (s.FakturaDatum >= DatumOd || s.FakturaDatum <= DatumDo)
                        && s.Blagajna == Int32.Parse(blagajna)
                        && s.Stanica == sifraStanice.SifraStanice
                        select s;

            var dt = new DataTable();

            dt.Columns.Add("FakturaBroj");
            dt.Columns.Add("FakturaDatum");
            dt.Columns.Add("FakturalniIznos");
            dt.Columns.Add("FakturalniIznos_pare");
            dt.Columns.Add("FakturaDatum5");
            dt.Columns.Add("FakturalniIznos6a");
            dt.Columns.Add("FakturalniIznos6a_pare");
            dt.Columns.Add("FakturaDatum7");
            dt.Columns.Add("FakturalniIznos8");
            dt.Columns.Add("FakturalniIznos8_pare");

            DataRow row;

            decimal fakUznosSum = 0;
            decimal fakUznos6aSum = 0;
            decimal fakUznos6bSum = 0;
            decimal fakUznos8Sum = 0;

            foreach (var item in query)
            {
                row = dt.NewRow();

                row["FakturaBroj"] = item.FakturaBroj;


                row["FakturaDatum"] = item.FakturaDatum;
                decimal fakUznos = (decimal)(item.FakturaOsnovica + item.FakturaPdv);
                fakUznosSum += fakUznos;
                string[] arrayFaktura = fakUznos.ToString().Split('.');
                row["FakturalniIznos"] = arrayFaktura[0];
                row["FakturalniIznos_pare"] = arrayFaktura[1];


                if (item.NaplacenoNB == 'D' && item.Saobracaj == '1')
                {
                    row["FakturaDatum5"] = item.FakturaDatum;
                    decimal fakUznos6a = (decimal)(item.FakturaOsnovica + item.FakturaPdv);
                    fakUznos6aSum += fakUznos6a;
                    string[] array = fakUznos6a.ToString().Split('.');
                    row["FakturalniIznos6a"] = array[0];
                    row["FakturalniIznos6a_pare"] = array[1];
                }
                if (item.NaplacenoNB == 'D' && item.Saobracaj == '2')
                {
                    row["FakturaDatum5"] = item.FakturaDatum;
                    decimal fakUznos6b = (decimal)(item.FakturaOsnovica + item.FakturaPdv);
                    fakUznos6bSum += fakUznos6b;
                    string[] array = fakUznos6b.ToString().Split('.');
                    row["FakturalniIznos6b"] = array[0];
                    row["FakturalniIznos6b_pare"] = array[1];
                }

                if (item.NaplacenoNB == 'N')
                {
                    row["FakturaDatum7"] = item.FakturaDatum.ToString();

                    decimal fakUznos8 = (decimal)(item.FakturaOsnovica + item.FakturaPdv);
                    fakUznos8Sum += fakUznos8;
                    string[] array = fakUznos8.ToString().Split('.');
                    row["FakturalniIznos8"] = array[0];
                    row["FakturalniIznos8_pare"] = array[1];
                }

                dt.Rows.Add(row);
            }

            string[] arrayFakUznosSum = fakUznosSum.ToString().Split('.');
            string[] arrayFakUznos6aSum = fakUznos6aSum.ToString().Split('.');
            string[] arrayFakUznos6bSum = fakUznos6bSum.ToString().Split('.');
            string[] arrayFakUznos8Sum = fakUznos8Sum.ToString().Split('.');


            string renderFormat = "PDF";
            string mimtype = "application/pdf";

            var localReport = new LocalReport();
            localReport.ReportPath = $"{this.webHostEnvironment.WebRootPath}\\Reports\\K111f.rdlc";
            localReport.DataSources.Add(new ReportDataSource("K111f", dt));
            var parametars = new[]
            {
                    new ReportParameter("Stanica", sifraStanice.SifraStanice),
                    new ReportParameter("DatumOd", DatumOd.ToString()),
                    new ReportParameter("DatumDo", DatumDo.ToString()),
                    new ReportParameter("FakturalniIznos8", arrayFakUznos8Sum[0]),
                    new ReportParameter("FakturalniIznos8_pare", arrayFakUznos8Sum[1])
            };

            localReport.SetParameters(parametars);
            var pdf = localReport.Render(renderFormat);
            return File(pdf, mimtype);

        }
    }
}
