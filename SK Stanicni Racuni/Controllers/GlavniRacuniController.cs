using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using SK_Stanicni_Racuni.CustomModelBinding.Datumi;
using SK_Stanicni_Racuni.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace SK_Stanicni_Racuni.Controllers
{
    public class GlavniRacuniController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;
        CultureInfo elGR = CultureInfo.CreateSpecificCulture("el-GR");
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
                if (user.Stanica.StartsWith("00099"))
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


            if (sifraStanice == null)
            {
                sifraStanice = string.Empty;
                stanica = string.Empty;
            }

            if (id == "K167")
            {
                /**********************  SlogKalak   **********************/

                var slogKalk = from sk in context.SlogKalks
                               where sk.PrRbb == Int32.Parse(blagajna)
                               && (sk.PrDatum >= DatumOd && sk.PrDatum <= DatumDo)
                               && sk.PrStanica == sifraStanice
                               select new
                               {
                                   K165a_iznos = sk.K165aIznos,
                                   Saobracaj = sk.Saobracaj,
                                   PrBroj = sk.PrBroj
                               };

                decimal naplacenoK165aUnutrSum = 0;
                decimal naplacenoK165aMedjSum = 0;
                List<int> prBrojUnutList = new List<int>();
                List<int> prBrojMedjList = new List<int>();

                foreach (var item in slogKalk)
                {
                    if (item.Saobracaj == "1")
                    {
                        naplacenoK165aUnutrSum += (decimal)item.K165a_iznos;
                        if (item.PrBroj > 0)
                        {
                            prBrojUnutList.Add((int)item.PrBroj);
                        }


                    }
                    else if (item.Saobracaj != "1")
                    {
                        naplacenoK165aMedjSum += (decimal)item.K165a_iznos;
                        if (item.PrBroj > 0)
                        {
                            prBrojMedjList.Add((int)item.PrBroj);
                        }

                    }
                }

                string[] arrayNaplacenoK165aUnutrSum = new string[2];

                if (naplacenoK165aUnutrSum == 0)
                {
                    arrayNaplacenoK165aUnutrSum[0] = "0";
                    arrayNaplacenoK165aUnutrSum[1] = "0";
                }
                else
                {
                    arrayNaplacenoK165aUnutrSum = naplacenoK165aUnutrSum.ToString($"F{2}").Split('.');
                }

                string[] arrayNaplacenoK165aMedjSum = new string[2];

                if (naplacenoK165aMedjSum == 0)
                {
                    arrayNaplacenoK165aMedjSum[0] = "0";
                    arrayNaplacenoK165aMedjSum[1] = "0";
                }
                else
                {
                    arrayNaplacenoK165aMedjSum = naplacenoK165aMedjSum.ToString($"F{2}").Split('.');
                }


                /************* NALEPNICE **********/

                var razlikaUnutr = 0;
                var prBrojUnutrMAX = 0;
                var prBrojUnutrMIN = 0;

                if (prBrojUnutList.Count != 0)
                {
                    prBrojUnutrMAX = prBrojUnutList.Max();
                    prBrojUnutrMIN = prBrojUnutList.Min();
                    razlikaUnutr = prBrojUnutrMAX - prBrojUnutrMIN + 1;
                }
                else
                {
                    prBrojUnutrMAX = 0;
                    prBrojUnutrMIN = 0;
                    razlikaUnutr = 0;
                }

                var razlikaMedj = 0;
                var prBrojMedjMAX = 0;
                var prBrojMedjMIN = 0;

                if (prBrojMedjList.Count != 0)
                {
                    prBrojMedjMAX = prBrojMedjList.Max();
                    prBrojMedjMIN = prBrojMedjList.Min();
                    razlikaMedj = prBrojMedjMAX - prBrojMedjMIN + 1;
                }
                else
                {
                    prBrojMedjMAX = 0;
                    prBrojMedjMIN = 0;
                    razlikaMedj = 0;
                }

                /****************    K161f   ******************************/

                var K161f = from s in context.SrK161fs
                            where (s.FakturaDatum >= DatumOd && s.FakturaDatum <= DatumDo)
                            && s.Stanica == sifraStanice
                            select new
                            {
                                s.FakturaOsnovica,
                                s.FakturaPdv,
                                s.Saobracaj
                            };

                decimal fakturaUnutSum = 0;

                foreach (var item in K161f)
                {
                    if (item.Saobracaj == "1")
                    {
                        fakturaUnutSum += (decimal)(item.FakturaOsnovica + item.FakturaPdv);
                    }
                }

                string[] arrayFakturaUnut = new string[2];

                if (fakturaUnutSum == 0)
                {
                    arrayFakturaUnut[0] = "0";
                    arrayFakturaUnut[1] = "0";
                }
                else
                {
                    arrayFakturaUnut = fakturaUnutSum.ToString($"F{2}").Split('.');
                }


                //***************** unutrasnji saobracaj  PDV K167 *************/                                    /* NIJE DOBRO KAKO RACUNA OSNOVICU I PDV   */

                var pdv = from sk in context.SlogKalks
                          join skPdv in context.SlogKalkPdvs on sk.RecId equals skPdv.RecId
                          into sks
                          from s in sks.DefaultIfEmpty()
                          where sk.Saobracaj == "1"
                          && sk.PrStanica == sifraStanice
                          && (sk.PrDatum >= DatumOd && sk.PrDatum <= DatumDo)
                          select new { sk.RecId, sk.TlSumaUpDin, s.Pdv2 };

                decimal pdvOsnovicaSum = 0;
                decimal pdvSum = 0;

                foreach (var item in pdv)
                {
                    if (item.TlSumaUpDin != null)
                    {
                        pdvOsnovicaSum += (decimal)(item.TlSumaUpDin);
                    }

                    if (item.Pdv2 != null)
                    {
                        pdvSum += (decimal)(item.Pdv2);
                    }
                }

                string[] arrayPdvOsnovica = new string[2];
                string[] arrayPdv = new string[2];

                if (pdvOsnovicaSum == 0)
                {
                    arrayPdvOsnovica[0] = "0";
                    arrayPdvOsnovica[1] = "0";
                }
                else
                {
                    arrayPdvOsnovica = pdvOsnovicaSum.ToString($"F{2}").Split('.');
                }

                if (pdvSum == 0)
                {
                    arrayPdv[0] = "0";
                    arrayPdv[1] = "0";
                }
                else
                {
                    arrayPdv = pdvSum.ToString($"F{2}").Split('.');
                }

                var suma1 = naplacenoK165aUnutrSum + fakturaUnutSum;
                var suma2 = naplacenoK165aMedjSum;
                var suma = suma1 + suma2;

                string[] arraySuma1 = new string[2];
                string[] arraySuma2 = new string[2];
                string[] arraySuma = new string[2];

                if (suma1 == 0)
                {
                    arraySuma1[0] = "0";
                    arraySuma1[1] = "0";
                }
                else
                {
                    arraySuma1 = suma1.ToString($"F{2}").Split('.');
                }

                if (suma2 == 0)
                {
                    arraySuma2[0] = "0";
                    arraySuma2[1] = "0";
                }
                else
                {
                    arraySuma2 = suma2.ToString($"F{2}").Split('.');
                }

                if (suma == 0)
                {
                    arraySuma[0] = "0";
                    arraySuma[1] = "0";
                }
                else
                {
                    arraySuma = suma.ToString($"F{2}").Split('.');
                }

                var dt = new DataTable();

                string renderFormat = "PDF";
                string mimtype = "application/pdf";

                var localReport = new LocalReport();
                localReport.ReportPath = $"{this.webHostEnvironment.WebRootPath}\\Reports\\K167.rdlc";
                localReport.DataSources.Add(new ReportDataSource("K167", dt));
                var ftm = new NumberFormatInfo();
                ftm.NegativeSign = "-";
                var parametars = new[]
                {
                        new ReportParameter("DatumOd", DatumOd.ToString()),
                        new ReportParameter("DatumDo", DatumDo.ToString()),
                        new ReportParameter("Blagajna", blagajna),
                        new ReportParameter("SifraStanice", stanica),
                        new ReportParameter("Racunopolagac", user),
                        new ReportParameter("naplacenoK165aUnutrSum", string.Format(elGR, "{0:0,0}", Double.Parse(arrayNaplacenoK165aUnutrSum[0], ftm))),
                        new ReportParameter("naplacenoK165aUnutrSum_pare", arrayNaplacenoK165aUnutrSum[1]),
                        new ReportParameter("naplacenoK165aMedjSum", string.Format(elGR, "{0:0,0}", Double.Parse(arrayNaplacenoK165aMedjSum[0], ftm))),
                        new ReportParameter("naplacenoK165aMedjSum_pare", arrayNaplacenoK165aMedjSum[1]),
                        new ReportParameter("fakturaUnutSum", string.Format(elGR, "{0:0,0}", Double.Parse(arrayFakturaUnut[0], ftm))),
                        new ReportParameter("fakturaUnutSum_pare", arrayFakturaUnut[1]),
                        new ReportParameter("prBrojUnutrMAX", string.Format(elGR, "{0:0,0}", Double.Parse(prBrojUnutrMAX.ToString()))),
                        new ReportParameter("prBrojUnutrMIN", string.Format(elGR, "{0:0,0}", Double.Parse(prBrojUnutrMIN.ToString()))),
                        new ReportParameter("razlikaUnutr", string.Format(elGR, "{0:0,0}", Double.Parse(razlikaUnutr.ToString()))),
                        new ReportParameter("prBrojMedjMAX", string.Format(elGR, "{0:0,0}", Double.Parse(prBrojMedjMAX.ToString()))),
                        new ReportParameter("prBrojMedjMIN", string.Format(elGR,"{0:0,0}", Double.Parse(prBrojMedjMIN.ToString()))),
                        new ReportParameter("razlikaMedj",  string.Format(elGR, "{0:0,0}", Double.Parse(razlikaMedj.ToString()))),
                        new ReportParameter("pdvOsnovicaSum", string.Format(elGR,"{0:0,0}", Double.Parse(arrayPdvOsnovica[0]))),
                        new ReportParameter("pdvOsnovicaSum_pare",arrayPdvOsnovica[1]),
                        new ReportParameter("pdvSum", string.Format(elGR,"{0:0,0}", Double.Parse(arrayPdv[0]))),
                        new ReportParameter("pdvSum_pare", arrayPdv[1]),
                        new ReportParameter("Suma1",  string.Format(elGR, "{0:0,0}", Double.Parse(arraySuma1[0], ftm))),
                        new ReportParameter("Suma1_pare", arraySuma1[1]),
                        new ReportParameter("Suma2",  string.Format(elGR, "{0:0,0}", Double.Parse(arraySuma2[0], ftm))),
                        new ReportParameter("Suma2_pare", arraySuma2[1]),
                        new ReportParameter("Suma",  string.Format(elGR, "{0:0,0}", Double.Parse(arraySuma[0], ftm))),
                        new ReportParameter("Suma_pare", arraySuma[1]),
                    };

                localReport.SetParameters(parametars);
                var pdf = localReport.Render(renderFormat);
                return File(pdf, mimtype);
            }
            else if (id == "K157")                                                                                  //*********************** K157 ******************* /
            {

                var SlogKalk = from s in context.SlogKalks
                               where (s.OtpDatum >= DatumOd && s.OtpDatum <= DatumDo)
                               && s.OtpStanica == sifraStanice
                               && s.OtpRbb == Int32.Parse(blagajna)
                               select new
                               {
                                   TlPrevFrDin = s.TlPrevFrDin,
                                   TlNakFrDin = s.TlNakFrDin,
                                   Saobracaj = s.Saobracaj,
                                   OtpBroj = s.OtpBroj
                               };

                decimal naplacenoK119UnutSum = 0;
                decimal naplacenoK119MedjSum = 0;
                List<int> otpBrojUnutList = new List<int>();
                List<int> otpBrojMedjList = new List<int>();

                foreach (var item in SlogKalk)
                {
                    if (item.Saobracaj == "1")
                    {
                        if (item.TlNakFrDin != null && item.TlPrevFrDin != null)
                        {
                            naplacenoK119UnutSum += (decimal)(item.TlNakFrDin + item.TlPrevFrDin);
                        }

                        if (item.OtpBroj > 0)
                        {
                            otpBrojUnutList.Add(item.OtpBroj);
                        }

                    }
                    else if (item.Saobracaj != "1")
                    {
                        if (item.TlNakFrDin != null && item.TlPrevFrDin != null)
                        {
                            naplacenoK119MedjSum += (decimal)(item.TlNakFrDin + item.TlPrevFrDin);
                        }
                        if (item.OtpBroj > 0)
                        {
                            otpBrojMedjList.Add(item.OtpBroj);
                        }

                    }
                }

                string[] arrayNaplacenoK119Unut = new string[2];
                string[] arrayNaplacenoK119Medj = new string[2];

                if (naplacenoK119UnutSum == 0)
                {
                    arrayNaplacenoK119Unut[0] = "0";
                    arrayNaplacenoK119Unut[1] = "0";
                }
                else
                {
                    arrayNaplacenoK119Unut = naplacenoK119UnutSum.ToString($"F{2}").Split('.');
                }

                if (naplacenoK119MedjSum == 0)
                {
                    arrayNaplacenoK119Medj[0] = "0";
                    arrayNaplacenoK119Medj[1] = "0";
                }
                else
                {
                    arrayNaplacenoK119Medj = naplacenoK119MedjSum.ToString($"F{2}").Split('.');
                }

                /************************ NALEPNICE*************/

                var otpBrojUnutMAX = (dynamic)null;
                var otpBrojUnutMIN = (dynamic)null;
                var razlikaOtpBrojUnut = (dynamic)null;

                if (otpBrojUnutList.Count != 0)
                {
                    otpBrojUnutMAX = otpBrojUnutList.Max();
                    otpBrojUnutMIN = otpBrojUnutList.Min();
                    razlikaOtpBrojUnut = otpBrojUnutMAX - otpBrojUnutMIN + 1;
                }
                else
                {
                    otpBrojUnutMAX = "0";
                    otpBrojUnutMIN = "0";
                    razlikaOtpBrojUnut = "0";
                }

                var otpBrojMedjuMAX = (dynamic)null;
                var otpBrojMedjuMIN = (dynamic)null;
                var razlikaOtpBrojMedj = (dynamic)null;

                if (otpBrojMedjList.Count != 0)
                {
                    otpBrojMedjuMAX = otpBrojMedjList.Max();
                    otpBrojMedjuMIN = otpBrojMedjList.Min();
                    razlikaOtpBrojMedj = otpBrojMedjuMAX - otpBrojMedjuMIN + 1;
                }
                else
                {
                    otpBrojMedjuMAX = "0";
                    otpBrojMedjuMIN = "0";
                    razlikaOtpBrojMedj = "0";
                }


                //***************** SR_121a ***********************************************************//

                var SR_121a = from sr121a in context.SrK121as
                              where (sr121a.OtpDatum >= DatumOd && sr121a.OtpDatum <= DatumDo)
                              && sr121a.Stanica == sifraStanice
                              select new
                              {
                                  ObracunFR = sr121a.ObracunFr,
                                  Saobracaj = sr121a.Saobracaj
                              };

                decimal naplacenoK115UnutrSum = 0;
                decimal naplacenoK115MedjSum = 0;

                foreach (var item in SR_121a)
                {
                    if (item.Saobracaj == "1"  && item.ObracunFR != null)
                    {
                        naplacenoK115UnutrSum += (decimal)item.ObracunFR;
                    }
                    else if (item.Saobracaj != "1" && item.ObracunFR != null)
                    {
                        naplacenoK115MedjSum += (decimal)item.ObracunFR;
                    }
                }

                string[] arrayNaplacenoK115UnutrSum = new string[2];
                string[] arrayNaplacenoK115MedjSum = new string[2];

                if (naplacenoK115UnutrSum == 0)
                {
                    arrayNaplacenoK115UnutrSum[0] = "0";
                    arrayNaplacenoK115UnutrSum[1] = "0";
                }
                else
                {
                    arrayNaplacenoK115UnutrSum = naplacenoK115UnutrSum.ToString($"F{2}").Split('.');
                }

                if (naplacenoK115MedjSum == 0)
                {
                    arrayNaplacenoK115MedjSum[0] = "0";
                    arrayNaplacenoK115MedjSum[1] = "0";
                }
                else
                {
                    arrayNaplacenoK115MedjSum = naplacenoK115MedjSum.ToString($"F{2}").Split('.');
                }

                //***************** SR_161f ********************************************************************//

                var sr161f = from sr161 in context.SrK161fs
                             where (sr161.FakturaDatum >= DatumOd && sr161.FakturaDatum <= DatumDo)
                             && sr161.Stanica == sifraStanice
                             && sr161.NaplacenoNb == "D" 
                             select new
                             {
                                 FakturaOsnovica = sr161.FakturaOsnovica,
                                 FakturaPDV = sr161.FakturaPdv,
                                 Saobracaj = sr161.Saobracaj
                             };

                decimal naknade111fUnutrSum = 0;

                foreach (var item in sr161f)
                {
                    if (item.Saobracaj == "1" && item.FakturaOsnovica != null && item.FakturaPDV != null)
                    {
                        naknade111fUnutrSum += (decimal)(item.FakturaOsnovica + item.FakturaPDV);
                    }
                }


                string[] arrayNaknade111fUnutrSum = new string[2];

                if (naknade111fUnutrSum == 0)
                {
                    arrayNaknade111fUnutrSum[0] = "0";
                    arrayNaknade111fUnutrSum[1] = "0";
                }
                else
                {
                    arrayNaknade111fUnutrSum = naknade111fUnutrSum.ToString($"F{2}").Split('.');
                }

                //***************** unutrasnji saobracaj  PDV K157 *************/                                    

                var pdv = from sk in context.SlogKalks
                          join skPdv in context.SlogKalkPdvs on sk.RecId equals skPdv.RecId
                          into sks
                          from s in sks.DefaultIfEmpty()
                          where sk.OtpStanica == sifraStanice &&
                          sk.Saobracaj == "1"
                          && (sk.OtpDatum >= DatumOd && sk.OtpDatum <= DatumDo)
                          select new { sk.RecId, sk.TlSumaFrDin, s.Pdv1 };


                decimal pdvOsnovicaSum = 0;
                decimal pdvSum = 0;

                foreach (var item in pdv)
                {
                    if (item.TlSumaFrDin != null)
                    {
                        pdvOsnovicaSum += (decimal)(item.TlSumaFrDin);
                    }

                    if (item.Pdv1 != null)
                    {
                        pdvSum += (decimal)(item.Pdv1);
                    }
                }

                string[] arrayPdvOsnovica = new string[2];
                string[] arrayPdv = new string[2];

                if (pdvOsnovicaSum == 0)
                {
                    arrayPdvOsnovica[0] = "0";
                    arrayPdvOsnovica[1] = "0";
                }
                else
                {
                    arrayPdvOsnovica = pdvOsnovicaSum.ToString($"F{2}").Split('.');
                }

                if (pdvSum == 0)
                {
                    arrayPdv[0] = "0";
                    arrayPdv[1] = "0";
                }
                else
                {
                    arrayPdv = pdvSum.ToString($"F{2}").Split('.');
                }



                var suma1 = naplacenoK119UnutSum + naplacenoK115UnutrSum + naknade111fUnutrSum;
                var suma2 = naplacenoK119MedjSum + naplacenoK115MedjSum;
                var suma = suma1 + suma2;

                string[] arraySuma1 = new string[2];
                string[] arraySuma2 = new string[2];
                string[] arraySuma = new string[2];

                if (suma1 == 0)
                {
                    arraySuma1[0] = "0";
                    arraySuma1[1] = "0";
                }
                else
                {
                    arraySuma1 = suma1.ToString($"F{2}").Split('.');
                }

                if (suma2 == 0)
                {
                    arraySuma2[0] = "0";
                    arraySuma2[1] = "0";
                }
                else
                {
                    arraySuma2 = suma2.ToString($"F{2}").Split('.');
                }

                if (suma == 0)
                {
                    arraySuma[0] = "0";
                    arraySuma[1] = "0";
                }
                else
                {
                    arraySuma = suma.ToString($"F{2}").Split('.');
                }


                var dt = new DataTable();

                // DataRow row;

                string renderFormat = "PDF";
                string mimtype = "application/pdf";

                var localReport = new LocalReport();
                localReport.ReportPath = $"{this.webHostEnvironment.WebRootPath}\\Reports\\K157.rdlc";
                localReport.DataSources.Add(new ReportDataSource("K157", dt));
                var ftm = new NumberFormatInfo();
                ftm.NegativeSign = "-";
                var parametars = new[]
                {
                        new ReportParameter("DatumOd", DatumOd.ToString()),
                        new ReportParameter("DatumDo", DatumDo.ToString()),
                        new ReportParameter("Blagajna", blagajna),
                        new ReportParameter("SifraStanice", stanica),
                        new ReportParameter("Racunopolagac", user),

                        new ReportParameter("naplacenoK119UnutSum", string.Format(elGR,"{0:0,0}", Double.Parse(arrayNaplacenoK119Unut[0], ftm))),
                        new ReportParameter("naplacenoK119UnutSum_pare", arrayNaplacenoK119Unut[1]),
                        new ReportParameter("naplacenoK119MedjSum", string.Format(elGR,"{0:0,0}", Double.Parse(arrayNaplacenoK119Medj[0], ftm))),
                        new ReportParameter("naplacenoK119MedjSum_pare", arrayNaplacenoK119Medj[1]),
                        new ReportParameter("naplacenoK115UnutrSum",  string.Format(elGR,"{0:0,0}", Double.Parse(arrayNaplacenoK115UnutrSum[0], ftm))),
                        new ReportParameter("naplacenoK115UnutrSum_pare", arrayNaplacenoK115UnutrSum[1]),
                        new ReportParameter("naplacenoK115MedjSum",  string.Format(elGR, "{0:0,0}", Double.Parse(arrayNaplacenoK115MedjSum[0], ftm))),
                        new ReportParameter("naplacenoK115MedjSum_pare", arrayNaplacenoK115MedjSum[1]),
                        new ReportParameter("naknade111fUnutrSum",  string.Format(elGR, "{0:0,0}", Double.Parse(arrayNaknade111fUnutrSum[0], ftm))),
                        new ReportParameter("naknade111fUnutrSum_pare", arrayNaknade111fUnutrSum[1]),
                        new ReportParameter("pdvOsnovicaSum", string.Format(elGR, "{0:0,0}", Double.Parse(arrayPdvOsnovica[0], ftm))),
                        new ReportParameter("pdvOsnovicaSum_pare",arrayPdvOsnovica[1]),
                        new ReportParameter("pdvSum", string.Format(elGR, "{0:0,0}", Double.Parse(arrayPdv[0], ftm))),
                        new ReportParameter("pdvSum_pare",arrayPdv[1]),
                        new ReportParameter("Suma1",  string.Format(elGR, "{0:0,0}", Double.Parse(arraySuma1[0], ftm))),
                        new ReportParameter("Suma1_pare", arraySuma1[1]),
                        new ReportParameter("Suma2",  string.Format(elGR, "{0:0,0}", Double.Parse(arraySuma2[0], ftm))),
                        new ReportParameter("Suma2_pare", arraySuma2[1]),
                        new ReportParameter("Suma",  string.Format(elGR, "{0:0,0}", Double.Parse(arraySuma[0], ftm))),
                        new ReportParameter("Suma_pare", arraySuma[1]),
                        new ReportParameter("otpBrojUnutMAX", string.Format(elGR, "{0:0,0}", Double.Parse(Convert.ToString(otpBrojUnutMAX)))),
                        new ReportParameter("otpBrojUnutMIN", string.Format(elGR, "{0:0,0}", Double.Parse(Convert.ToString(otpBrojUnutMIN)))),
                        new ReportParameter("razlikaOtpBrojUnut",   string.Format(elGR, "{0:0,0}", Double.Parse(Convert.ToString(razlikaOtpBrojUnut)))),
                        new ReportParameter("otpBrojMedjuMAX", string.Format(elGR, "{0:0,0}", Double.Parse(Convert.ToString(otpBrojMedjuMAX)))),
                        new ReportParameter("otpBrojMedjuMIN",  string.Format(elGR, "{0:0,0}", Double.Parse(Convert.ToString(otpBrojMedjuMIN)))),
                        new ReportParameter("razlikaOtpBrojMedj",  string.Format(elGR, "{0:0,0}", Double.Parse(Convert.ToString(razlikaOtpBrojMedj))))


                    };

                localReport.SetParameters(parametars);
                var pdf = localReport.Render(renderFormat);
                return File(pdf, mimtype);
            }

            return new EmptyResult();


        }

    }
}
