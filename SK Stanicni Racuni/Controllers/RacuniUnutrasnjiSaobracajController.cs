using AspNetCore.Reporting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
            return View();
        }

        /***************** PRINT PDF DOKUMENT ******************/

        public IActionResult Print(string id, string stanica, string blagajna,
             DateTime DatumOd, DateTime DatumDo)
        {

            if (id == "K140")
            {
                var query = from sk in context.SlogKalks
                            join zs in context.ZsStanices on sk.PrStanica equals zs.SifraStanice

                            where sk.Saobracaj == "1" && (sk.OtpDatum >= DatumOd && sk.OtpDatum <= DatumDo)
                            select new { BrOtpr = sk.OtpBroj, UputnaStanica = zs.Naziv };

                var dt = new DataTable();

                dt.Columns.Add("BrOtpr");
                dt.Columns.Add("UputnaStanica");

                DataRow row;

                var k140 = query.Select(x => new { OtpBroj = x.BrOtpr, UputnaStanica = x.UputnaStanica });

                foreach (var item in k140)
                {
                    row = dt.NewRow();
                    row["BrOtpr"] = item.OtpBroj;
                    row["UputnaStanica"] = item.UputnaStanica;
                    dt.Rows.Add(row);
                }

                string mimtype = "";
                int extension = 1;

                Dictionary<string, string> paramtars = new Dictionary<string, string>();

                paramtars.Add("Stanica", stanica);
                paramtars.Add("Blagajna", blagajna);
                paramtars.Add("DatumOd", DatumOd.ToString());
                paramtars.Add("DatumDo", DatumDo.ToString());

                var path = $"{this.webHostEnvironment.WebRootPath}\\Reports\\K140.rdlc";
                LocalReport localReport = new LocalReport(path);
                localReport.AddDataSource("K140", dt);
                extension = (int)(DateTime.Now.Ticks >> 10);
                result = localReport.Execute(RenderType.Pdf, extension, paramtars, mimtype);
            } else if(id == "K165")
            {

            }

            return File(result.MainStream, "application/pdf");
        }

    }
}
