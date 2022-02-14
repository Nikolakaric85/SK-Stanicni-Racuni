using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using SK_Stanicni_Racuni.CustomModelBinding.Datumi;
using SK_Stanicni_Racuni.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SK_Stanicni_Racuni.Controllers
{
    public class GlavniRacuniController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public GlavniRacuniController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult GlavniRacuni(string id)
        {
            ViewBag.Id = id;

            var UserId = HttpContext.User.Identity.Name;
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

        public IActionResult Print(string id, string stanica, string blagajna, string user, [ModelBinder(typeof(DatumOdModelBinder))] DateTime DatumOd, [ModelBinder(typeof(DatumDoModelBinder))] DateTime DatumDo)
        {
            var sifraStanice = context.ZsStanices.Where(x => x.Naziv == stanica).Select(x => x.SifraStanice).FirstOrDefault();           // sve sa 72 na primer 7223499
            

            if (id == "K167")
            {

                var dt = new DataTable();

               // DataRow row;

                string renderFormat = "PDF";
                string mimtype = "application/pdf";

                var localReport = new LocalReport();
                localReport.ReportPath = $"{this.webHostEnvironment.WebRootPath}\\Reports\\K167.rdlc";
                localReport.DataSources.Add(new ReportDataSource("K167", dt));
                var parametars = new[]
                {
                        
                        new ReportParameter("DatumOd", DatumOd.ToString()),
                        new ReportParameter("DatumDo", DatumDo.ToString()),
                        new ReportParameter("Blagajna", blagajna),
                        new ReportParameter("SifraStanice", sifraStanice),
                        new ReportParameter("Racunopolagac", user),
                    };

                localReport.SetParameters(parametars);
                var pdf = localReport.Render(renderFormat);
                return File(pdf, mimtype);
            }
            else if (id == "K157")
            {

                var SlogKalk = from s in context.SlogKalks
                               where (s.OtpDatum >= DatumOd && s.OtpDatum <= DatumDo)
                               && s.OtpStanica == sifraStanice 
                               select new { TlPrevFrDin = s.TlPrevFrDin,
                                            TlNakFrDin = s.TlNakFrDin,
                                            Saobracaj = s.Saobracaj};

                decimal naplacenoK119UnutSum = 0;
                decimal naplacenoK119MedjSum = 0;

                foreach (var item in SlogKalk)
                {
                    if (item.Saobracaj == "1")
                    {
                        if (item.TlNakFrDin != null && item.TlPrevFrDin != null)
                        {
                            naplacenoK119UnutSum += (decimal)(item.TlNakFrDin + item.TlPrevFrDin);
                        }
                    }
                    else if (item.Saobracaj != "1")
                    {
                        if (item.TlNakFrDin != null && item.TlPrevFrDin != null)
                        {
                            naplacenoK119MedjSum += (decimal)(item.TlNakFrDin + item.TlPrevFrDin);
                        }
                    }
                }

                string[] arrayNaplacenoK119Unut = new string[2];
                string[] arrayNaplacenoK119Medj = new string[2];

                if (naplacenoK119UnutSum == 0)
                {
                    arrayNaplacenoK119Unut[0] = string.Empty;
                    arrayNaplacenoK119Unut[1] = string.Empty;
                } else
                {
                    arrayNaplacenoK119Unut = naplacenoK119UnutSum.ToString($"F{2}").Split('.');
                }

                if (naplacenoK119MedjSum == 0)
                {
                    arrayNaplacenoK119Medj[0] = string.Empty;
                    arrayNaplacenoK119Medj[1] = string.Empty;
                }
                else
                {
                    arrayNaplacenoK119Medj = naplacenoK119MedjSum.ToString($"F{2}").Split('.');
                }

                //***************** SR_121a

                var SR_121a = from sr121a in context.SrK121as
                              where (sr121a.OtpDatum >= DatumOd && sr121a.OtpDatum <= DatumDo)
                              && sr121a.Stanica == sifraStanice
                              select new { ObracunFR = sr121a.ObracunFR,
                                           Saobracaj = sr121a.Saobracaj};

                decimal naplacenoK115UnutrSum = 0;
                decimal naplacenoK115MedjSum = 0;

                foreach (var item in SR_121a)
                {
                    if (item.Saobracaj == '1' && item.ObracunFR != null)
                    {
                        naplacenoK115UnutrSum += (decimal)item.ObracunFR;
                    }
                    else if (item.Saobracaj != '1' && item.ObracunFR != null)
                    {
                        naplacenoK115MedjSum += (decimal)item.ObracunFR;
                    }
                }

                string[] arrayNaplacenoK115UnutrSum = new string[2];
                string[] arrayNaplacenoK115MedjSum = new string[2];

                if (naplacenoK115UnutrSum == 0)
                {
                    arrayNaplacenoK115UnutrSum[0] = string.Empty;
                    arrayNaplacenoK115UnutrSum[1] = string.Empty;
                } else
                {
                    arrayNaplacenoK115UnutrSum = naplacenoK115UnutrSum.ToString($"F{2}").Split('.');
                }

                if (naplacenoK115MedjSum == 0)
                {
                    arrayNaplacenoK115MedjSum[0] = string.Empty;
                    arrayNaplacenoK115MedjSum[1] = string.Empty;
                }
                else
                {
                    arrayNaplacenoK115MedjSum = naplacenoK115MedjSum.ToString($"F{2}").Split('.');
                }

                //***************** SR_161f

                var sr161f = from sr161 in context.SrK161fs
                             where (sr161.FakturaDatum >= DatumOd && sr161.FakturaDatum <= DatumDo)
                             && sr161.Stanica == sifraStanice
                             
                             select new { FakturaOsnovica = sr161.FakturaOsnovica,
                                          FakturaPDV = sr161.FakturaPdv,
                                          Saobracaj = sr161.Saobracaj
                                        };

                decimal naknade111fUnutrSum = 0;
                
                foreach (var item in sr161f)
                {
                    if (item.Saobracaj == '1' && item.FakturaOsnovica != null && item.FakturaPDV != null)
                    {
                        naknade111fUnutrSum += (decimal)(item.FakturaOsnovica + item.FakturaPDV);
                    }
                }


                string[] arrayNaknade111fUnutrSum = new string[2];

                if (naknade111fUnutrSum == 0)
                {
                    arrayNaknade111fUnutrSum[0] = string.Empty;
                    arrayNaknade111fUnutrSum[1] = string.Empty;
                } else
                {
                    arrayNaknade111fUnutrSum = naknade111fUnutrSum.ToString($"F{2}").Split('.');
                }

                //***************** unutrasnji saobracaj  PDV *************/

                var pdv = from sk in context.SlogKalks
                          join skPdv in context.SlogKalkPdvs on sk.RecId equals skPdv.RecId
                          into sks
                          from s in sks.DefaultIfEmpty()
                          where sk.Saobracaj == "1"
                          select new { sk.RecId, sk.TlPrevFrDin, sk.TlNakFrDin, s.Pdv1, s.Pdv2 };







                var dt = new DataTable();

                // DataRow row;

                string renderFormat = "PDF";
                string mimtype = "application/pdf";

                var localReport = new LocalReport();
                localReport.ReportPath = $"{this.webHostEnvironment.WebRootPath}\\Reports\\K157.rdlc";
                localReport.DataSources.Add(new ReportDataSource("K157", dt));
                var parametars = new[]
                {
                        new ReportParameter("DatumOd", DatumOd.ToString()),
                        new ReportParameter("DatumDo", DatumDo.ToString()),
                        new ReportParameter("Blagajna", blagajna),
                        new ReportParameter("SifraStanice", sifraStanice),
                        new ReportParameter("Racunopolagac", user),

                        new ReportParameter("naplacenoK119UnutSum", arrayNaplacenoK119Unut[0]),
                        new ReportParameter("naplacenoK119UnutSum_pare", arrayNaplacenoK119Unut[1]),
                        new ReportParameter("naplacenoK119MedjSum", arrayNaplacenoK119Medj[0]),
                        new ReportParameter("naplacenoK119MedjSum_pare", arrayNaplacenoK119Medj[1]),
                        new ReportParameter("naplacenoK115UnutrSum", arrayNaplacenoK115UnutrSum[0]),
                        new ReportParameter("naplacenoK115UnutrSum_pare", arrayNaplacenoK115UnutrSum[1]),
                        new ReportParameter("naplacenoK115MedjSum", arrayNaplacenoK115MedjSum[0]),
                        new ReportParameter("naplacenoK115MedjSum_pare", arrayNaplacenoK115MedjSum[1]),
                        new ReportParameter("naknade111fUnutrSum", arrayNaknade111fUnutrSum[0]),
                        new ReportParameter("naknade111fUnutrSum_pare", arrayNaknade111fUnutrSum[1])                        

                    };

                localReport.SetParameters(parametars);
                var pdf = localReport.Render(renderFormat);
                return File(pdf, mimtype);
            }

            return new EmptyResult();
            

        }

    }
}
