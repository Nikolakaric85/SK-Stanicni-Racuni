
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using SK_Stanicni_Racuni.CustomModelBinding.Datumi;
using SK_Stanicni_Racuni.Models;
using System;
using System.Data;
using System.Globalization;
using System.Linq;


namespace SK_Stanicni_Racuni.Controllers
{
    public class RekapitulacijaController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly INotyfService notyf;
        CultureInfo elGR = CultureInfo.CreateSpecificCulture("el-GR");
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

        public IActionResult Print_K111f([ModelBinder(typeof(DatumOdModelBinder))] DateTime DatumOd, [ModelBinder(typeof(DatumDoModelBinder))] DateTime DatumDo,
            string blagajna, string stanica)
        {
            var sifraStanice = string.Empty;
            if (!string.IsNullOrEmpty(stanica))
            {
                sifraStanice = context.ZsStanices.Where(x => x.Naziv == stanica).FirstOrDefault().SifraStanice;
            }

            var query = from s in context.SrK161fs
                        where (s.FakturaDatum >= DatumOd && s.FakturaDatum <= DatumDo)
                        && s.Blagajna == Int32.Parse(blagajna)
                        && s.Stanica == sifraStanice
                        select s;

            var dt = new DataTable();

            dt.Columns.Add("FakturaBroj");
            dt.Columns.Add("FakturaDatum");
            dt.Columns.Add("FakturalniIznos");
            dt.Columns.Add("FakturalniIznos_pare");
            dt.Columns.Add("FakturaDatum5");
            dt.Columns.Add("FakturalniIznos6a");
            dt.Columns.Add("FakturalniIznos6a_pare");
            dt.Columns.Add("FakturalniIznos6b");
            dt.Columns.Add("FakturalniIznos6b_pare");
            dt.Columns.Add("FakturaDatum7");
            dt.Columns.Add("FakturalniIznos8");
            dt.Columns.Add("FakturalniIznos8_pare");

            DataRow row;

            decimal fakIznosSum = 0;
            decimal fakIznos6aSum = 0;
            decimal fakIznos6bSum = 0;
            decimal fakIznos8Sum = 0;

            foreach (var item in query)
            {
                row = dt.NewRow();

                row["FakturaBroj"] = item.FakturaBroj;
                if (item.FakturaDatum.HasValue)
                {
                    row["FakturaDatum"] = item.FakturaDatum.Value.ToString("dd.MM.yyyy");
                }

                if (item.NaplacenoNB == 'D' && item.Saobracaj == '1')
                {
                    if (item.FakturaDatum.HasValue)
                    {
                        row["FakturaDatum5"] = item.FakturaDatum.Value.ToString("dd.MM.yyyy");
                    }

                    decimal fakUznos6a = (decimal)(item.FakturaOsnovica + item.FakturaPdv);
                    fakIznos6aSum += fakUznos6a;
                    string[] array = fakUznos6a.ToString().Split('.');
                    row["FakturalniIznos6a"] = string.Format(elGR, "{0:0,0}", Double.Parse(array[0]));
                    row["FakturalniIznos6a_pare"] = array[1];
                }
                else if (item.NaplacenoNB == 'D' && item.Saobracaj == '2')
                {
                    if (item.FakturaDatum.HasValue)
                    {
                        row["FakturaDatum5"] = item.FakturaDatum.Value.ToString("dd.MM.yyyy");
                    }

                    decimal fakUznos6b = (decimal)(item.FakturaOsnovica + item.FakturaPdv);
                    fakIznos6bSum += fakUznos6b;
                    string[] array = fakUznos6b.ToString().Split('.');
                    row["FakturalniIznos6b"] = string.Format(elGR, "{0:0,0}", Double.Parse(array[0]));
                    row["FakturalniIznos6b_pare"] = array[1];
                }
                else
                {
                    decimal fakUznos = (decimal)(item.FakturaOsnovica + item.FakturaPdv);
                    fakIznosSum += fakUznos;
                    string[] arrayFaktura = fakUznos.ToString().Split('.');
                    row["FakturalniIznos"] = string.Format(elGR, "{0:0,0}", Double.Parse(arrayFaktura[0]));
                    row["FakturalniIznos_pare"] = arrayFaktura[1];
                }

                if (item.NaplacenoNB == 'N')
                {
                    if (item.FakturaDatum.HasValue)
                    {
                        row["FakturaDatum7"] = item.FakturaDatum.Value.ToString("dd.MM.yyyy");
                    }

                    decimal fakUznos8 = (decimal)(item.FakturaOsnovica + item.FakturaPdv);
                    fakIznos8Sum += fakUznos8;
                    string[] array = fakUznos8.ToString().Split('.');
                    row["FakturalniIznos8"] = string.Format(elGR, "{0:0,0}", Double.Parse(array[0]));
                    row["FakturalniIznos8_pare"] = array[1];
                }

                dt.Rows.Add(row);
            }

            string[] arrayFakIznosSum = new string[2];
            string[] arrayFakIznos6aSum = new string[2];
            string[] arrayFakIznos6bSum = new string[2];
            string[] arrayFakIznos8Sum = new string[2];

            if (fakIznosSum == 0)
            {
                arrayFakIznosSum[0] = "0";
                arrayFakIznosSum[1] = "0";
            }
            else
            {
                arrayFakIznosSum = fakIznosSum.ToString($"F{2}").Split('.');
            }

            if (fakIznos6aSum == 0)
            {
                arrayFakIznos6aSum[0] = "0";
                arrayFakIznos6aSum[1] = "0";
            }
            else
            {
                arrayFakIznos6aSum = fakIznos6aSum.ToString($"F{2}").Split('.');
            }

            if (fakIznos6bSum == 0)
            {
                arrayFakIznos6bSum[0] = "0";
                arrayFakIznos6bSum[1] = "0";
            }
            else
            {
                arrayFakIznos6bSum = fakIznos6bSum.ToString($"F{2}").Split('.');
            }

            if (fakIznos8Sum == 0)
            {
                arrayFakIznos8Sum[0] = "0";
                arrayFakIznos8Sum[1] = "0";
            }
            else
            {
                arrayFakIznos8Sum = fakIznos8Sum.ToString($"F{2}").Split('.');
            }

            string renderFormat = "PDF";
            //  string mimtype = "application/pdf";

            var localReport = new LocalReport();
            localReport.ReportPath = $"{this.webHostEnvironment.WebRootPath}\\Reports\\K111f.rdlc";
            localReport.DataSources.Add(new ReportDataSource("K111f", dt));
            var parametars = new[]
            {
                    new ReportParameter("Stanica", sifraStanice),
                    new ReportParameter("Blagajna", blagajna),
                    new ReportParameter("DatumOd", DatumOd.ToString()),
                    new ReportParameter("DatumDo", DatumDo.ToString()),
                    new ReportParameter("FakUznosSum", string.Format(elGR, "{0:0,0}", Double.Parse(arrayFakIznosSum[0]))),
                    new ReportParameter("FakUznosSum_pare",arrayFakIznosSum[1]),
                    new ReportParameter("FakIznos6aSum", string.Format(elGR, "{0:0,0}", Double.Parse(arrayFakIznos6aSum[0]))),
                    new ReportParameter("FakIznos6aSum_pare",arrayFakIznos6aSum[1]),
                    new ReportParameter("FakIznos6bSum", string.Format(elGR, "{0:0,0}", Double.Parse(arrayFakIznos6bSum[0]))),
                    new ReportParameter("FakIznos6bSum_pare",arrayFakIznos6bSum[1]),
                    new ReportParameter("FakturalniIznos8", string.Format(elGR, "{0:0,0}", Double.Parse(arrayFakIznos8Sum[0]))),
                    new ReportParameter("FakturalniIznos8_pare", arrayFakIznos8Sum[1])
            };

            localReport.SetParameters(parametars);
            var pdf = localReport.Render(renderFormat);
            return File(pdf, "application/pdf");
        }

        public IActionResult K115()
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

        public IActionResult Print_K115([ModelBinder(typeof(DatumOdModelBinder))] DateTime DatumOd, [ModelBinder(typeof(DatumDoModelBinder))] DateTime DatumDo,
            string blagajna, string stanica)
        {
            var sifraStanice = string.Empty;

            if (!string.IsNullOrEmpty(stanica))
            {
                sifraStanice = context.ZsStanices.Where(x => x.Naziv == stanica).FirstOrDefault().SifraStanice;
            }

            var query = from s in context.SrK121as
                        where (s.Datum >= DatumOd || s.Datum <= DatumDo)
                        && s.Blagajna == Int32.Parse(blagajna)
                        && s.Stanica == sifraStanice
                        select s;

            var dt = new DataTable();

            dt.Columns.Add("Datum");
            dt.Columns.Add("Broj");
            dt.Columns.Add("Iznos");
            dt.Columns.Add("ObracunFR");
            dt.Columns.Add("ObracunFR13");
            dt.Columns.Add("ObracunFR14");
            dt.Columns.Add("Razlika");

            DataRow row;

            decimal SumaIznos = 0;
            decimal SumaObracunFR = 0;
            decimal SumaObracunFR13 = 0;
            decimal SumaObracunFR14 = 0;
            decimal Razlika = 0;

            foreach (var item in query)
            {
                row = dt.NewRow();

                if (item.Datum.HasValue)
                {
                    row["Datum"] = item.Datum.Value.ToString("dd.MM.yyyy");
                }

                row["Broj"] = item.Broj;

                row["Iznos"] = string.Format(elGR, "{0:0,0}", Double.Parse(item.Iznos.ToString()));
                SumaIznos += (decimal)item.Iznos;

                if (item.Saobracaj == '1')
                {
                    if (item.ObracunFR.HasValue)
                    {
                        row["ObracunFR13"] = string.Format(elGR, "{0:0,0}", Double.Parse(item.ObracunFR.ToString()));
                        SumaObracunFR13 += (decimal)item.ObracunFR;
                    }

                }
                else if (item.Saobracaj == '2')
                {
                    if (item.ObracunFR.HasValue)
                    {
                        row["ObracunFR14"] = string.Format(elGR, "{0:0,0}", Double.Parse(item.ObracunFR.ToString()));
                        SumaObracunFR14 += (decimal)item.ObracunFR;
                    }
                }
                else
                {
                    if (item.ObracunFR.HasValue)
                    {
                        row["ObracunFR"] = string.Format(elGR, "{0:0,0}", Double.Parse(item.ObracunFR.ToString()));
                        SumaObracunFR += (decimal)item.ObracunFR;
                    }
                }

                if (item.Iznos != 0 && item.ObracunFR != 0)
                {
                    if (item.Iznos.HasValue && item.ObracunFR.HasValue)
                    {
                        row["Razlika"] = string.Format(elGR, "{0:0,0}", Double.Parse((item.Iznos - item.ObracunFR).ToString()));
                        Razlika += (decimal)(item.Iznos - item.ObracunFR);
                    }
                }
                dt.Rows.Add(row);
            }

            string renderFormat = "PDF";
            string mimtype = "application/pdf";

            var localReport = new LocalReport();
            localReport.ReportPath = $"{this.webHostEnvironment.WebRootPath}\\Reports\\K115.rdlc";
            localReport.DataSources.Add(new ReportDataSource("K115", dt));
            var parametars = new[]
            {
                    new ReportParameter("Stanica", sifraStanice),
                    new ReportParameter("Blagajna", blagajna),
                    new ReportParameter("DatumOd", DatumOd.ToString()),
                    new ReportParameter("DatumDo", DatumDo.ToString()),
                    new ReportParameter("SumaIznos", string.Format(elGR, "{0:0,0}", Double.Parse(SumaIznos.ToString()))),
                    new ReportParameter("SumaObracunFR", string.Format(elGR, "{0:0,0}", Double.Parse(SumaObracunFR.ToString()))),
                    new ReportParameter("SumaObracunFR13",  string.Format(elGR, "{0:0,0}", Double.Parse(SumaObracunFR13.ToString()))),
                    new ReportParameter("SumaObracunFR14", string.Format(elGR, "{0:0,0}", Double.Parse(SumaObracunFR14.ToString()))),
                    new ReportParameter("Razlika", string.Format(elGR, "{0:0,0}", Double.Parse(Razlika.ToString()))),


            };

            localReport.SetParameters(parametars);
            var pdf = localReport.Render(renderFormat);
            return File(pdf, mimtype);

        }
    }
}
