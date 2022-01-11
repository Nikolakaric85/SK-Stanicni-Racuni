﻿using AspNetCore.Reporting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SK_Stanicni_Racuni.CustomModelBinding.RacuniUnutrasnjiSaobracaj;
using SK_Stanicni_Racuni.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SK_Stanicni_Racuni.Controllers
{
    public class RacuniМedjunarodniSaobracajController : Controller
    {

        private ReportResult result; // za pdf izvestaj
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
            return View();
        }


        public IActionResult Print(string id, string stanica, string blagajna, [ModelBinder(typeof(DatumOdModelBinder))] DateTime DatumOd,
          [ModelBinder(typeof(DatumDoModelBinder))] DateTime DatumDo)
        {
            var sifraStanice = context.ZsStanices.Where(x => x.Naziv == stanica).Select(x => x.SifraStanice).FirstOrDefault();
            var _sifraStanice = context.ZsStanices.Where(x => x.Naziv == stanica).Select(x => x.SifraStanice1).FirstOrDefault();
            string mimtype = "";
            int extension = 1;

            if (id == "K140m")
            {
                Dictionary<string, string> paramtars = new Dictionary<string, string>();

                var path = $"{this.webHostEnvironment.WebRootPath}\\Reports\\K140m.rdlc";
                LocalReport localReport = new LocalReport(path);
                //localReport.AddDataSource("K117", dt);

                result = localReport.Execute(RenderType.Pdf, extension, paramtars, mimtype);
            }
            else if (id == "K165m")
            {

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

                if (query.Any())
                {



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
                        //row["UlaznaEtiketa"] = item.UlEtiketa;
                        //row["OtpUprava"] = item.OtpUprava;
                        //row["OtpStanica"] = item.OtpStanica;
                        //row["OtpBroj"] = item.OtpBroj;
                        //row["OtpDatum"] = item.OtpDatum.ToString();
                        //row["PrUprava"] = item.PrUprava;
                        //row["PrStanica"] = item.PrStanica;
                        //row["RbKola"] = item.KolaStavka;
                        //row["IBK"] = item.IBK;
                        //row["TrasaVoza"] = item.BrojVoza;
                        //row["SatVoza"] = item.SatVoza;
                        //row["TarifaUgovor"] = item.Ugovor;
                        row["IzlazniPrelaz"] = item.ZsIzPrelaz;
                        dt.Rows.Add(row);
                    }

                    Dictionary<string, string> paramtars = new Dictionary<string, string>();

                    paramtars.Add("Stanica", stanica);
                    paramtars.Add("SifraStanice", _sifraStanice);
                    paramtars.Add("DatumDo", DatumDo.ToString());

                    var path = $"{this.webHostEnvironment.WebRootPath}\\Reports\\K140trz.rdlc";
                    LocalReport localReport = new LocalReport(path);
                    localReport.AddDataSource("K140trz", dt);

                    result = localReport.Execute(RenderType.Pdf, extension, paramtars, mimtype);
                }




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

                if (query.Any())
                {
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
                        row["OtpDatum"] = item.OtpDatum.ToString();
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

                    Dictionary<string, string> paramtars = new Dictionary<string, string>();

                    paramtars.Add("Stanica", stanica);
                    paramtars.Add("sifraStanice", _sifraStanice);
                    paramtars.Add("DatumDo", DatumDo.ToString());

                    var path = $"{this.webHostEnvironment.WebRootPath}\\Reports\\K165trz.rdlc";
                    LocalReport localReport = new LocalReport(path);
                    localReport.AddDataSource("K165trz", dt);

                    result = localReport.Execute(RenderType.Pdf, extension, paramtars, mimtype);

                }

            }


            return File(result.MainStream, "application/pdf");

        }
    }
}
