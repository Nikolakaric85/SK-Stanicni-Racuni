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
    public class RacuniМedjunarodniSaobracajController : Controller
    {

        //  private ReportResult result; // za pdf izvestaj
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public RacuniМedjunarodniSaobracajController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        

        public IActionResult RacuniМedjunarodniSaobracaj(string id)
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


        public IActionResult Print(string id, string stanica, string blagajna, [ModelBinder(typeof(DatumOdModelBinder))] DateTime DatumOd, [ModelBinder(typeof(DatumDoModelBinder))] DateTime DatumDo)
        {

            var UserId = HttpContext.User.Identity.Name;
            var user = context.UserTabs.Where(x => x.UserId == UserId).FirstOrDefault();

            if (string.IsNullOrEmpty(stanica))
            {
                stanica = string.Empty;
            }

            var sifraStanice = context.ZsStanices.Where(x => x.Naziv == stanica).Select(x => x.SifraStanice).FirstOrDefault();           // sve sa 72 na primer 7223499
            var _sifraStanice = context.ZsStanices.Where(x => x.Naziv == stanica).Select(x => x.SifraStanice1).FirstOrDefault();

            if (sifraStanice == null)
            {
                stanica = string.Empty;
            }

            if (id == "K140m")
            {
                var firstJoin = from ZsStanice in context.ZsStanices
                                join SlogKalk in context.SlogKalks
                                on ZsStanice.SifraStanice equals SlogKalk.OtpStanica
                                select new
                                {
                                    OtpBroj = SlogKalk.OtpBroj,
                                    OtpDatum = SlogKalk.OtpDatum,
                                    PrStanica = SlogKalk.PrStanica,
                                    TlSumaFrDin = SlogKalk.TlSumaFrDin,
                                    OtpUprava = SlogKalk.OtpUprava,
                                    OtpStanica = SlogKalk.OtpStanica,
                                    RecID = SlogKalk.RecId,
                                    Stanica = SlogKalk.Stanica,
                                    Saobracaj = SlogKalk.Saobracaj,
                                    OtpRbb = SlogKalk.OtpRbb

                                };

                var query = from fij in firstJoin
                            join SlogKola in context.SlogKolas
                            on new { OtpUprava = fij.OtpUprava, OtpStanica = fij.OtpStanica, OtpBroj = fij.OtpBroj, OtpDatum = fij.OtpDatum, RecID = fij.RecID, Stanica = fij.Stanica } equals
                               new { OtpUprava = SlogKola.OtpUprava, OtpStanica = SlogKola.OtpStanica, OtpBroj = SlogKola.OtpBroj, OtpDatum = SlogKola.OtpDatum, RecID = SlogKola.RecId, Stanica = SlogKola.Stanica }
                            where fij.Saobracaj == "3" &&
                            fij.OtpRbb == Int32.Parse(blagajna) &&
                            fij.OtpDatum >= DatumOd && fij.OtpDatum <= DatumDo &&
                            fij.Stanica == _sifraStanice

                            select new
                            {
                                OtpBroj = fij.OtpBroj,
                                OtpDatum = fij.OtpDatum,
                                PrStanica = fij.PrStanica,
                                TlSumaFrDin = fij.TlSumaFrDin,
                                IBK = SlogKola.Ibk
                            };


                var dt = new DataTable();

                dt.Columns.Add("OtpBroj");
                dt.Columns.Add("OtpDatum");
                dt.Columns.Add("PrStanica");
                dt.Columns.Add("PrUprava");
                dt.Columns.Add("TlSumaFrDin");
                dt.Columns.Add("TlSumaFrDin_pare");
                dt.Columns.Add("IBK");

                DataRow row;
                double intPart = 0.00;
                int fractionalPart = 0;
                Decimal sve = 0;
                foreach (var item in query)
                {

                    row = dt.NewRow();
                    row["OtpBroj"] = item.OtpBroj;
                    row["OtpDatum"] = item.OtpDatum.ToString("dd.MM.yyyy");
                    row["PrStanica"] = item.PrStanica;
                    row["PrUprava"] = item.PrStanica.Substring(0, 2);

                    var TlSuma = item.TlSumaFrDin.ToString();
                    string[] array = TlSuma.Split('.');

                    row["TlSumaFrDin"] = array[0];
                    row["TlSumaFrDin_pare"] = array[1];
                    row["IBK"] = item.IBK;

                    intPart += Double.Parse(array[0]);
                    fractionalPart += Int32.Parse(array[1]); ;
                    sve += (decimal)item.TlSumaFrDin;
                    dt.Rows.Add(row);
                }


                long intPartSum = (long)sve;
                double fractionalPartSum = (double)(sve - intPartSum);
                string[] decimalPart = (fractionalPartSum * 100).ToString().Split('.');

                string renderFormat = "PDF";
                string mimtype = "application/pdf";

                var localReport = new LocalReport();
                localReport.ReportPath = $"{this.webHostEnvironment.WebRootPath}\\Reports\\K140m.rdlc";
                localReport.DataSources.Add(new ReportDataSource("K140m", dt));
                var parametars = new[]
                {
                    new ReportParameter("SumInt", intPartSum.ToString()),
                    new ReportParameter("SumDec", decimalPart[0]),
                    new ReportParameter("DatumOd", DatumOd.ToString()),
                    new ReportParameter("DatumDo", DatumDo.ToString()),
                    new ReportParameter("Blagajna", blagajna),
                    new ReportParameter("SifraStanice", stanica),
                    new ReportParameter("Racunopolagac", user.Naziv.Trim())
                };

                localReport.SetParameters(parametars);
                var pdf = localReport.Render(renderFormat);
                return File(pdf, mimtype);


            }
            else if (id == "K165m")
            {
                var LeftOuterJoin = from ZsPrelazi in context.ZsPrelazis
                                    join SlogKalk in context.SlogKalks
                                    on ZsPrelazi.SifraPrelaza equals SlogKalk.ZsIzPrelaz into gruping
                                    from LeftGroup in gruping.DefaultIfEmpty()
                                    select new
                                    {
                                        LeftGroup.OtpStanica,
                                        OtpBroj = LeftGroup.OtpBroj.ToString() == null ? (int?)null : LeftGroup.OtpBroj,
                                        OtpDatum = LeftGroup.OtpDatum.ToString() == null ? (DateTime?)null : LeftGroup.OtpDatum,
                                        PrUprava = LeftGroup.PrUprava,
                                        PrStanica = LeftGroup.PrStanica,
                                        Ugovor = LeftGroup.Ugovor,
                                        OtpUprava = LeftGroup.OtpUprava,
                                        BrojVoza2 = LeftGroup.BrojVoza2,
                                        SatVoza2 = LeftGroup.SatVoza2,
                                        DatumIzlaza = LeftGroup.DatumIzlaza,
                                        ZsIzPrelaz = LeftGroup.ZsIzPrelaz,
                                        Saobracaj = LeftGroup.Saobracaj,
                                        RecID = LeftGroup.RecId.ToString() == null ? (int?)null : LeftGroup.RecId,
                                        Stanica = LeftGroup.Stanica,
                                        SifraTarife = LeftGroup.SifraTarife,
                                        Naziv = ZsPrelazi.Naziv,
                                        ZsSifraPrelaza = ZsPrelazi.SifraPrelaza,
                                        PrDatum = LeftGroup.PrDatum,
                                        PrBroj = LeftGroup.PrBroj,
                                        TlSumaUpDin = LeftGroup.TlSumaUpDin
                                    };

                var RightOuterJoin = from SlogKalk in context.SlogKalks
                                     join ZsPrelazi in context.ZsPrelazis
                                     on SlogKalk.ZsIzPrelaz equals ZsPrelazi.SifraPrelaza into gruping
                                     from RightGroup in gruping.DefaultIfEmpty()
                                     select new
                                     {
                                         SlogKalk.OtpStanica,
                                         OtpBroj = SlogKalk.OtpBroj.ToString() == null ? (int?)null : SlogKalk.OtpBroj,
                                         OtpDatum = SlogKalk.OtpDatum.ToString() == null ? (DateTime?)null : SlogKalk.OtpDatum,
                                         PrUprava = SlogKalk.PrUprava,
                                         PrStanica = SlogKalk.PrStanica,
                                         Ugovor = SlogKalk.Ugovor,
                                         OtpUprava = SlogKalk.OtpUprava,
                                         BrojVoza2 = SlogKalk.BrojVoza2,
                                         SatVoza2 = SlogKalk.SatVoza2,
                                         DatumIzlaza = SlogKalk.DatumIzlaza,
                                         ZsIzPrelaz = SlogKalk.ZsIzPrelaz,
                                         Saobracaj = SlogKalk.Saobracaj,
                                         RecID = SlogKalk.RecId.ToString() == null ? (int?)null : SlogKalk.RecId,
                                         Stanica = SlogKalk.Stanica,
                                         SifraTarife = SlogKalk.SifraTarife,
                                         Naziv = RightGroup.Naziv,
                                         ZsSifraPrelaza = RightGroup.SifraPrelaza,
                                         PrDatum = SlogKalk.PrDatum,
                                         PrBroj = SlogKalk.PrBroj,
                                         TlSumaUpDin = SlogKalk.TlSumaUpDin

                                     };

                var fullOuterJoin = LeftOuterJoin.Union(RightOuterJoin);

                var firstInnerJoin = from foj in fullOuterJoin
                                     join SKo in context.SlogKolas on new { OtpUprava = foj.OtpUprava, OtpStanica = foj.OtpStanica, OtpBroj = (int?)foj.OtpBroj, OtpDatum = (DateTime?)foj.OtpDatum, Stanica = foj.Stanica, RecID = (int?)foj.RecID } equals
                                                                      new { OtpUprava = SKo.OtpUprava, OtpStanica = SKo.OtpStanica, OtpBroj = (int?)SKo.OtpBroj, OtpDatum = (DateTime?)SKo.OtpDatum, Stanica = SKo.Stanica, RecID = (int?)SKo.RecId }
                                     select new
                                     {
                                         OtpStanica = foj.OtpStanica,
                                         OtpBroj = foj.OtpBroj,
                                         OtpDatum = foj.OtpDatum,
                                         PrUprava = foj.PrUprava,
                                         PrStanica = foj.PrStanica,
                                         Ugovor = foj.Ugovor,
                                         OtpUprava = foj.OtpUprava,
                                         BrojVoza2 = foj.BrojVoza2,
                                         SatVoza2 = foj.SatVoza2,
                                         DatumIzlaza = foj.DatumIzlaza,
                                         ZsIzPrelaz = foj.ZsIzPrelaz,
                                         Saobracaj = foj.Saobracaj,
                                         RecID = foj.RecID,
                                         Stanica = foj.Stanica,
                                         SifraTarife = foj.SifraTarife,
                                         Naziv = foj.Naziv,
                                         ZsSifraPrelaza = foj.ZsSifraPrelaza,
                                         PrDatum = foj.PrDatum,
                                         PrBroj = foj.PrBroj,
                                         TlSumaUpDin = foj.TlSumaUpDin,


                                     };

                var final = from fij in firstInnerJoin
                            join ZsTarifa in context.ZsTarifas
                            on fij.SifraTarife equals ZsTarifa.SifraTarife
                            where fij.Saobracaj == "2" &&
                            fij.PrStanica == sifraStanice &&
                            fij.PrDatum >= DatumOd && fij.PrDatum <= DatumDo
                            group fij by new
                            {
                                fij.OtpStanica,
                                fij.OtpBroj,
                                fij.OtpDatum,
                                fij.PrBroj,
                                fij.TlSumaUpDin
                            }
                            into fijGroup
                            select new
                            {
                                PrBroj = fijGroup.Key.PrBroj,
                                OtpBroj = fijGroup.Key.OtpBroj,
                                OtpStanica = fijGroup.Key.OtpStanica,
                                OtpDatum = fijGroup.Key.OtpDatum,
                                TlSumaUpDin = fijGroup.Key.TlSumaUpDin
                            };


                var dt = new DataTable();

                dt.Columns.Add("PrBroj");
                dt.Columns.Add("OtpStanica");
                dt.Columns.Add("NazivStanice");
                dt.Columns.Add("ŠifraStanice");
                dt.Columns.Add("OtpBroj");
                dt.Columns.Add("OtpDatum");
                dt.Columns.Add("TlSumaUpDin");
                dt.Columns.Add("TlSumaUpDin_pare");

                DataRow row;

                foreach (var item in final)
                {
                    row = dt.NewRow();
                    row["PrBroj"] = item.PrBroj;
                    row["OtpStanica"] = item.OtpStanica.Substring(0, 2);

                    var nazivStanice = context.ZsStanices.Where(x => x.SifraStanice == item.OtpStanica).FirstOrDefault();
                    _ = nazivStanice != null ? row["NazivStanice"] = nazivStanice.Naziv : row["NazivStanice"] = string.Empty;

                    row["ŠifraStanice"] = item.OtpStanica.Substring(2, 5);
                    row["OtpBroj"] = item.OtpBroj;
                    if (item.OtpDatum.HasValue)
                    {
                        row["OtpDatum"] = item.OtpDatum.Value.ToString("dd.MM.yyyy");
                    }


                    string res = item.TlSumaUpDin.ToString();
                    string[] array = res.Split('.');

                    row["TlSumaUpDin"] = array[0];
                    row["TlSumaUpDin_pare"] = array[1];

                    dt.Rows.Add(row);
                }


                string renderFormat = "PDF";
                string mimtype = "application/pdf";

                var localReport = new LocalReport();
                localReport.ReportPath = $"{this.webHostEnvironment.WebRootPath}\\Reports\\K165m.rdlc";
                localReport.DataSources.Add(new ReportDataSource("K165m", dt));
                var parametars = new[]
                {
                    new ReportParameter("Stanica", stanica.ToString()),
                    new ReportParameter("Blagajna", blagajna),
                    new ReportParameter("DatumDo", DatumDo.ToString()),
                    new ReportParameter("DatumOd", DatumOd.ToString()),

                };

                localReport.SetParameters(parametars);
                var pdf = localReport.Render(renderFormat);
                return File(pdf, mimtype);


            }
            else if (id == "K140trz")
            {
                var query = from kalk in context.SlogKalks
                            join kola in context.SlogKolas on new { OtpUprava = kalk.OtpUprava, OtpStanica = kalk.OtpStanica, OtpBroj = kalk.OtpBroj, OtpDatum = kalk.OtpDatum, RecID = kalk.RecId, Stanica = kalk.Stanica } equals
                                                              new { OtpUprava = kola.OtpUprava, OtpStanica = kola.OtpStanica, OtpBroj = kola.OtpBroj, OtpDatum = kola.OtpDatum, RecID = kola.RecId, Stanica = kola.Stanica }
                            where kalk.Saobracaj == "4" &&
                            kalk.Stanica == _sifraStanice &&
                            kalk.ZsUlPrelaz == _sifraStanice &&
                            kalk.DatumUlaza == DatumDo
                            select new
                            {
                                UlEtiketa = kalk.UlEtiketa,
                                OtpUprava = kalk.OtpUprava,
                                OtpStanica = kalk.OtpStanica,
                                OtpBroj = kalk.OtpBroj,
                                OtpDatum = kalk.OtpDatum,
                                PrUprava = kalk.PrUprava,
                                PrStanica = kalk.PrStanica,
                                KolaStavka = kola.KolaStavka,
                                IBK = kola.Ibk,
                                BrojVoza = kalk.BrojVoza,
                                SatVoza = kalk.SatVoza,
                                Ugovor = kalk.Ugovor,
                                ZsIzPrelaz = kalk.ZsIzPrelaz
                            };


                var dt = new DataTable();

                dt.Columns.Add("UlaznaEtiketa");
                dt.Columns.Add("OtpUprava");
                dt.Columns.Add("OtpStanica");
                dt.Columns.Add("OtpBroj");
                dt.Columns.Add("OtpDatum");
                dt.Columns.Add("PrUprava");
                dt.Columns.Add("PrStanica");
                dt.Columns.Add("RbKola");
                dt.Columns.Add("IBK");
                dt.Columns.Add("TrasaVoza");
                dt.Columns.Add("SatVoza");
                dt.Columns.Add("TarifaUgovor");
                dt.Columns.Add("IzlazniPrelaz");

                DataRow row;

                foreach (var item in query)
                {
                    row = dt.NewRow();
                    row["UlaznaEtiketa"] = item.UlEtiketa;
                    row["OtpUprava"] = item.OtpUprava;
                    row["OtpStanica"] = item.OtpStanica;
                    row["OtpBroj"] = item.OtpBroj;
                    row["OtpDatum"] = item.OtpDatum.ToString("dd.MM.yyyy");
                    row["PrUprava"] = item.PrUprava;
                    row["PrStanica"] = item.PrStanica;
                    row["RbKola"] = item.KolaStavka;
                    row["IBK"] = item.IBK;
                    row["TrasaVoza"] = item.BrojVoza;
                    row["SatVoza"] = item.SatVoza.Trim();
                    row["TarifaUgovor"] = item.Ugovor;
                    row["IzlazniPrelaz"] = item.ZsIzPrelaz;
                    dt.Rows.Add(row);
                }

                string renderFormat = "PDF";
                string mimtype = "application/pdf";

                var localReport = new LocalReport();
                localReport.ReportPath = $"{this.webHostEnvironment.WebRootPath}\\Reports\\K140trz.rdlc";
                localReport.DataSources.Add(new ReportDataSource("K140trz", dt));
                var parametars = new[]
                {
                    new ReportParameter("Stanica", stanica.ToString()),
                    new ReportParameter("SifraStanice", _sifraStanice),
                    new ReportParameter("DatumDo", DatumDo.ToString()),

                };

                localReport.SetParameters(parametars);
                var pdf = localReport.Render(renderFormat);
                return File(pdf, mimtype);


            }
            else if (id == "K165trz")
            {
                var query = from kalk in context.SlogKalks
                            join kola in context.SlogKolas on new { OtpUprava = kalk.OtpUprava, OtpStanica = kalk.OtpStanica, OtpBroj = kalk.OtpBroj, OtpDatum = kalk.OtpDatum, RecID = kalk.RecId, Stanica = kalk.Stanica } equals
                                                              new { OtpUprava = kola.OtpUprava, OtpStanica = kola.OtpStanica, OtpBroj = kola.OtpBroj, OtpDatum = kola.OtpDatum, RecID = kola.RecId, Stanica = kola.Stanica }
                            where kalk.Saobracaj == "4" &&
                            kalk.IzEtiketa > 0 &&
                            kalk.ZsIzPrelaz == _sifraStanice &&
                            kalk.DatumIzlaza == DatumDo
                            select new
                            {
                                IzEtiketa = kalk.IzEtiketa,
                                OtpUprava = kalk.OtpUprava,
                                OtpStanica = kalk.OtpStanica,
                                OtpBroj = kalk.OtpBroj,
                                OtpDatum = kalk.OtpDatum,
                                PrUprava = kalk.PrUprava,
                                PrStanica = kalk.PrStanica,
                                KolaStavka = kola.KolaStavka,
                                IBK = kola.Ibk,
                                BrojVoza2 = kalk.BrojVoza2,
                                SatVoza2 = kalk.SatVoza2,
                                Ugovor = kalk.Ugovor,
                                ZsUlPrelaz = kalk.ZsUlPrelaz,
                                UlEtiketa = kalk.UlEtiketa
                            };



                var dt = new DataTable();

                dt.Columns.Add("IzEtiketa");
                dt.Columns.Add("OtpUprava");
                dt.Columns.Add("OtpStanica");
                dt.Columns.Add("OtpBroj");
                dt.Columns.Add("OtpDatum");
                dt.Columns.Add("PrUprava");
                dt.Columns.Add("PrStanica");
                dt.Columns.Add("KolaStavka");
                dt.Columns.Add("IBK");
                dt.Columns.Add("BrojVoza2");
                dt.Columns.Add("SatVoza2");
                dt.Columns.Add("Ugovor");
                dt.Columns.Add("ZsUlPrelaz");
                dt.Columns.Add("UlEtiketa");

                DataRow row;

                foreach (var item in query)
                {
                    row = dt.NewRow();
                    row["IzEtiketa"] = item.IzEtiketa;
                    row["OtpUprava"] = item.OtpUprava;
                    row["OtpStanica"] = item.OtpStanica;
                    row["OtpBroj"] = item.OtpBroj;
                    row["OtpDatum"] = item.OtpDatum.ToString("dd.MM.yyyy");
                    row["PrUprava"] = item.PrUprava;
                    row["PrStanica"] = item.PrStanica;
                    row["KolaStavka"] = item.KolaStavka;
                    row["IBK"] = item.IBK;
                    row["BrojVoza2"] = item.BrojVoza2;
                    row["SatVoza2"] = item.SatVoza2.Trim();
                    row["Ugovor"] = item.Ugovor;
                    row["ZsUlPrelaz"] = item.ZsUlPrelaz;
                    row["UlEtiketa"] = item.UlEtiketa;
                    dt.Rows.Add(row);
                }

                string renderFormat = "PDF";
                string mimtype = "application/pdf";

                var localReport = new LocalReport();
                localReport.ReportPath = $"{this.webHostEnvironment.WebRootPath}\\Reports\\K165trz.rdlc";
                localReport.DataSources.Add(new ReportDataSource("K165trz", dt));
                var parametars = new[]
                {
                    new ReportParameter("Stanica", stanica.ToString()),
                    new ReportParameter("sifraStanice", _sifraStanice),
                    new ReportParameter("DatumDo", DatumDo.ToString()),

                };

                localReport.SetParameters(parametars);
                var pdf = localReport.Render(renderFormat);
                return File(pdf, mimtype);
            }


            return View();

        }
    }
}
