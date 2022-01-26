using AspNetCore.Reporting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SK_Stanicni_Racuni.CustomModelBinding.Datumi;
using SK_Stanicni_Racuni.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SK_Stanicni_Racuni.Controllers
{
    public class RacuniUnutrasnjiSaobracajController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private ReportResult result; // za pdf izvestaj

        public RacuniUnutrasnjiSaobracajController(AppDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        }


        public ActionResult ListaStanica()
        {
            var stanice = context.ZsStanices.Select(x => x.Naziv);
            return Json(stanice);
        }


        public IActionResult RacuniUnutrasnjiSaobracaj(string id)
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

        /***************** PRINT PDF DOKUMENT ******************/

        public IActionResult Print(string id, string stanica, string blagajna,
              [ModelBinder(typeof(DatumOdModelBinder))] DateTime DatumOd, [ModelBinder(typeof(DatumDoModelBinder))] DateTime DatumDo)
        {
            var sifraStanice = context.ZsStanices.Where(x => x.Naziv == stanica).Select(x => x.SifraStanice).FirstOrDefault();           // sve sa 72 na primer 7223499
            if (id == "K140")
            {
                var query = from SlogKalk in context.SlogKalks
                            join ZsStanice in context.ZsStanices
                            on SlogKalk.PrStanica equals ZsStanice.SifraStanice
                            join ZsStanice_1 in context.ZsStanices
                            on SlogKalk.Stanica equals ZsStanice_1.SifraStanice1
                            join Ugovori in context.Ugovoris
                            on SlogKalk.Ugovor equals Ugovori.BrojUgovora
                            into sku
                            from SlogKalkUgovori in sku.DefaultIfEmpty()
                            join SlogKalkPDV in context.SlogKalkPdvs
                            on SlogKalk.RecId equals SlogKalkPDV.RecId
                            into skp
                            from SlogKalk_PDV in skp.DefaultIfEmpty()
                            where SlogKalk.Saobracaj == "1" &&
                            SlogKalk.OtpStanica == sifraStanice &&
                            SlogKalk.OtpRbb == Int32.Parse(blagajna) &&
                            SlogKalk.OtpDatum >= DatumOd && SlogKalk.OtpDatum <= DatumDo

                            select new
                            {
                                OtpBroj = SlogKalk.OtpBroj,
                                OtpStanica = SlogKalk.OtpStanica,
                                OtpRbb = SlogKalk.OtpRbb,
                                Naziv = ZsStanice.Naziv,
                                tlSumaFrDin = SlogKalk.TlSumaFrDin,
                                PDV1 = SlogKalk.TlSumaFrDin > 0 ? SlogKalk_PDV.Pdv1 : 0,
                                VrstaObracuna = SlogKalkUgovori.VrstaObracuna,
                                Saobracaj = SlogKalk.Saobracaj,
                                Datum = SlogKalk.OtpDatum,
                                OtpStNaziv = ZsStanice_1.Naziv,
                                PoreskaOsnovica = SlogKalk.TlSumaFrDin > 0 ? SlogKalk.TlSumaFrDin - SlogKalk_PDV.Pdv1 : 0
                            };

                var dt = new DataTable();

                dt.Columns.Add("OtpBroj");
                dt.Columns.Add("OtpStanica");
                dt.Columns.Add("OtpRbb");
                dt.Columns.Add("Naziv");
                dt.Columns.Add("tlSumaFrDin");
                dt.Columns.Add("tlSumaFrDin_pare");
                dt.Columns.Add("PDV1");
                dt.Columns.Add("PDV1_pare");
                dt.Columns.Add("VrstaObracuna");
                dt.Columns.Add("Saobracaj");
                dt.Columns.Add("Datum");
                dt.Columns.Add("OtpStNaziv");
                dt.Columns.Add("PoreskaOsnovica");
                dt.Columns.Add("PoreskaOsnovica_pare");

                DataRow row;

                double intPartPoreskaO = 0.00;
                double intPartPDV = 0.00;
                double intPartFranko = 0.00;
                int fractionalPartPoreskaO = 0;
                int fractionalPartPDV = 0;
                int fractionalPartFranko = 0;
                Decimal svePoreskaO = 0;
                Decimal svePDV = 0;
                Decimal sveFranko = 0;

                foreach (var item in query)
                {
                    row = dt.NewRow();
                    row["OtpBroj"] = item.OtpBroj;
                    row["OtpStanica"] = item.OtpStanica;
                    row["OtpRbb"] = item.OtpRbb;
                    row["Naziv"] = item.Naziv;

                    if (item.tlSumaFrDin != null)
                    {
                        string[] array = item.tlSumaFrDin.ToString().Split('.');
                        row["tlSumaFrDin"] = array[0];
                        row["tlSumaFrDin_pare"] = array[1];

                        intPartFranko += Double.Parse(array[0]);
                        fractionalPartFranko += Int32.Parse(array[1]); ;
                        sveFranko += (decimal)item.tlSumaFrDin;
                    }
                    else
                    {
                        row["tlSumaFrDin"] = string.Empty;
                        row["tlSumaFrDin_pare"] = string.Empty;
                    }

                    if (item.PDV1 != null)
                    {
                        string[] arrayPDV = item.PDV1.ToString().Split('.');
                        row["PDV1"] = arrayPDV[0];
                        row["PDV1_pare"] = arrayPDV[1];

                        intPartPDV += Double.Parse(arrayPDV[0]);
                        fractionalPartPDV += Int32.Parse(arrayPDV[1]); ;
                        svePDV += (decimal)item.PDV1;
                    }
                    else
                    {
                        row["PDV1"] = string.Empty;
                        row["PDV1_pare"] = string.Empty;
                    }

                    row["VrstaObracuna"] = item.VrstaObracuna;
                    row["Saobracaj"] = item.Saobracaj;
                    row["Datum"] = item.Datum;
                    row["OtpStNaziv"] = item.OtpStNaziv;

                    string[] arrayPoreskaOsnovica = new string[2];

                    if (item.PoreskaOsnovica != null)
                    {
                        arrayPoreskaOsnovica = item.PoreskaOsnovica.ToString().Split('.');
                        row["PoreskaOsnovica"] = arrayPoreskaOsnovica[0];
                        row["PoreskaOsnovica_pare"] = arrayPoreskaOsnovica[1];

                        intPartPoreskaO += Double.Parse(arrayPoreskaOsnovica[0]);
                        fractionalPartPoreskaO += Int32.Parse(arrayPoreskaOsnovica[1]); ;
                        svePoreskaO += (decimal)item.PoreskaOsnovica;
                    }
                    else
                    {
                        row["PoreskaOsnovica"] = string.Empty;
                        row["PoreskaOsnovica_pare"] = string.Empty;
                    }

                    dt.Rows.Add(row);
                }

                long intPartSum = (long)svePoreskaO;
                double fractionalPartSum = (double)(svePoreskaO - intPartSum);
                string[] decimalPart = (fractionalPartSum * 100).ToString().Split('.');

                long intPartSumPDV = (long)svePDV;
                double fractionalPartSumPDV = (double)(svePDV - intPartSumPDV);
                string[] decimalPartPDV = (fractionalPartSumPDV * 100).ToString().Split('.');

                long intPartSumFranko = (long)sveFranko;
                double fractionalPartSumFranko = (double)(sveFranko - intPartSumFranko);
                string[] decimalPartFranko = (fractionalPartSumFranko * 100).ToString().Split('.');

                string mimtype = "";
                int extension = 1;

                Dictionary<string, string> paramtars = new Dictionary<string, string>();

                paramtars.Add("SumIntPoreskaOsnovica", intPartSum.ToString());
                paramtars.Add("SumDecPoreskaOsnovica", decimalPart[0]);

                paramtars.Add("SumIntPDV", intPartSumPDV.ToString());
                paramtars.Add("SumDecPDV", decimalPartPDV[0]);

                paramtars.Add("SumIntFranko", intPartSumFranko.ToString());
                paramtars.Add("SumDecFranko", decimalPartFranko[0]);

                paramtars.Add("Stanica", stanica);
                paramtars.Add("Blagajna", blagajna);
                paramtars.Add("DatumOd", DatumOd.ToString());
                paramtars.Add("DatumDo", DatumDo.ToString());

                var path = $"{this.webHostEnvironment.WebRootPath}\\Reports\\K140.rdlc";
                LocalReport localReport = new LocalReport(path);
                localReport.AddDataSource("K140", dt);
                extension = (int)(DateTime.Now.Ticks >> 10);
                result = localReport.Execute(RenderType.Pdf, extension, paramtars, mimtype);

            }
            else if (id == "K165")
            {
                var query = from SlogKalk in context.SlogKalks
                            join ZsStanice in context.ZsStanices
                            on SlogKalk.PrStanica equals ZsStanice.SifraStanice
                            join ZsStanice_1 in context.ZsStanices
                            on SlogKalk.OtpStanica equals ZsStanice_1.SifraStanice
                            join Ugovori in context.Ugovoris
                            on SlogKalk.Ugovor equals Ugovori.BrojUgovora
                            into sku
                            from SlogKalkUgovori in sku.DefaultIfEmpty()
                            join SlogKalkPDV in context.SlogKalkPdvs
                            on SlogKalk.RecId equals SlogKalkPDV.RecId
                            into skp
                            from SlogKalk_PDV in skp.DefaultIfEmpty()
                            where SlogKalk.Saobracaj == "1" &&
                            SlogKalk.PrStanica == sifraStanice &&
                            SlogKalk.OtpDatum >= DatumOd &&
                            SlogKalk.OtpDatum <= DatumDo

                            select new
                            {
                                PrStanica = SlogKalk.PrStanica,
                                PrStNaziv = ZsStanice_1.Naziv,
                                PrBroj = SlogKalk.PrBroj,
                                PrDatum = SlogKalk.PrDatum,
                                PrRbb = SlogKalk.PrRbb,
                                OtpStanica = SlogKalk.OtpStanica,
                                OtpStNaziv = ZsStanice.Naziv,
                                OtpBroj = SlogKalk.OtpBroj,
                                OtpDatum = SlogKalk.OtpDatum,
                                VrstaObracuna = SlogKalkUgovori.VrstaObracuna,
                                Saobracaj = SlogKalk.Saobracaj,
                                PDV2 = SlogKalk.TlSumaUpDin > 0 ? SlogKalk_PDV.Pdv2 : 0,
                                tlSumaUpDin = SlogKalk.TlSumaUpDin,
                                PoreskaOsnovica = SlogKalk.TlSumaUpDin > 0 ? SlogKalk.TlSumaUpDin - SlogKalk_PDV.Pdv2 : 0
                            };

                var dt = new DataTable();

                dt.Columns.Add("PrBroj");
                dt.Columns.Add("OtpBroj");
                dt.Columns.Add("OtpDatum");
                dt.Columns.Add("OtpStNaziv");
                dt.Columns.Add("tlSumaUpDin");
                dt.Columns.Add("tlSumaUpDin_pare");
                dt.Columns.Add("PoreskaOsnovica");
                dt.Columns.Add("PoreskaOsnovica_pare");
                dt.Columns.Add("PDV2");
                dt.Columns.Add("PDV2_pare");
                dt.Columns.Add("VrstaObracuna");


                DataRow row;

                decimal tlSumaUpDin = 0;
                decimal poreskaOsnovica = 0;
                decimal pdv2 = 0;


                foreach (var item in query)
                {
                    row = dt.NewRow();
                    row["PrBroj"] = item.PrBroj;
                    row["OtpBroj"] = item.OtpBroj;
                    row["OtpDatum"] = item.OtpDatum;
                    row["OtpStNaziv"] = item.OtpStNaziv;

                    if (item.tlSumaUpDin != null)
                    {
                        string[] array = item.tlSumaUpDin.ToString().Split('.');
                        row["tlSumaUpDin"] = array[0];
                        row["tlSumaUpDin_pare"] = array[1];

                        tlSumaUpDin += (decimal)item.tlSumaUpDin;
                    }
                    else
                    {
                        row["tlSumaUpDin"] = string.Empty;
                        row["tlSumaUpDin_pare"] = string.Empty;
                    }

                    if (item.PoreskaOsnovica != null)
                    {
                        string[] arrayPoreskaO = item.PoreskaOsnovica.ToString().Split('.');
                        row["PoreskaOsnovica"] = arrayPoreskaO[0];
                        row["PoreskaOsnovica_pare"] = arrayPoreskaO[1];

                        poreskaOsnovica += (decimal)item.PoreskaOsnovica;
                    }
                    else
                    {
                        row["PoreskaOsnovica"] = string.Empty;
                        row["PoreskaOsnovica_pare"] = string.Empty;
                    }

                    if (item.PDV2 != null)
                    {
                        string[] arrayPDV = item.PDV2.ToString().Split('.');
                        row["PDV2"] = arrayPDV[0];
                        row["PDV2_pare"] = arrayPDV[1];

                        pdv2 += (decimal)item.PDV2;
                    }
                    else
                    {
                        row["PDV2"] = string.Empty;
                        row["PDV2_pare"] = string.Empty;
                    }
                    row["VrstaObracuna"] = item.VrstaObracuna;
                    dt.Rows.Add(row);
                }

                string[] arrayTlSumaUpDin = tlSumaUpDin.ToString().Split('.');
                string[] arrayPoreskaOsnovica = poreskaOsnovica.ToString().Split('.');
                string[] arrayPDV2 = pdv2.ToString().Split('.');

                string mimtype = "";
                int extension = 1;

                Dictionary<string, string> paramtars = new Dictionary<string, string>();

                paramtars.Add("SumIntUP", arrayTlSumaUpDin[0]);
                paramtars.Add("SumDecUP", arrayTlSumaUpDin[1]);

                paramtars.Add("SumIntPoreskaOsnovica", arrayPoreskaOsnovica[0]);
                paramtars.Add("SumDecPoreskaOsnovica", arrayPoreskaOsnovica[1]);

                paramtars.Add("SumIntPDV", arrayPDV2[0]);
                paramtars.Add("SumDecPDV", arrayPDV2[1]);

                paramtars.Add("Stanica", stanica);
                paramtars.Add("Blagajna", blagajna);
                paramtars.Add("DatumOd", DatumOd.ToString());
                paramtars.Add("DatumDo", DatumDo.ToString());

                var path = $"{this.webHostEnvironment.WebRootPath}\\Reports\\K165.rdlc";
                LocalReport localReport = new LocalReport(path);
                localReport.AddDataSource("K165", dt);
                extension = (int)(DateTime.Now.Ticks >> 10);
                result = localReport.Execute(RenderType.Pdf, extension, paramtars, mimtype);

            }

            return File(result.MainStream, "application/pdf");
        }

    }
}
