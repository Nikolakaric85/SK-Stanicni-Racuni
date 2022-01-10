using AspNetCore.Reporting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SK_Stanicni_Racuni.CustomModelBinding.RacuniUnutrasnjiSaobracaj;
using SK_Stanicni_Racuni.Models;
using SK_Stanicni_Racuni.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SK_Stanicni_Racuni.Controllers
{
    public class RacuniUnutrasnjiSaobracajController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public RacuniUnutrasnjiSaobracajController(AppDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        }


        public ActionResult ListaStanica()
        {
            var stanice = context.ZsStanices.Select(x=>x.Naziv);
            return Json(stanice);
        }


        public IActionResult RacuniUnutrasnjiSaobracaj(string id)
        {
            ViewBag.Id = id;
            return View();
        }

        /***************** PRINT PDF DOKUMENT ******************/

        public IActionResult Print(string stanica, string blagajna, [ModelBinder(typeof(DatumOdModelBinder))] DateTime DatumOd,
            [ModelBinder(typeof(DatumDoModelBinder))] DateTime DatumDo)
        {

            var query = from sk in context.SlogKalks
                        join zs in context.ZsStanices on sk.PrStanica equals zs.SifraStanice
                        
                        where sk.Saobracaj == "1" && (sk.OtpDatum >= DatumOd && sk.OtpDatum <= DatumDo)
                        select new { BrOtpr = sk.OtpBroj, UputnaStanica = zs.Naziv };


            //var viewModel = new SlogKalkZsStaniceViewModel();

            //foreach (var item in query)
            //{
            //    viewModel.SlogKalk.OtpBroj = item.BrOtpr;
            //    viewModel.ZsStanice.Naziv = item.UputnaStanica;
            //}


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
            
          //  ReportResult result;


            Dictionary<string, string> paramtars = new Dictionary<string, string>();

            paramtars.Add("Stanica", stanica);
            paramtars.Add("Blagajna", blagajna);
            paramtars.Add("DatumOd", DatumOd.ToString());
            paramtars.Add("DatumDo", DatumDo.ToString());
 
            var path = $"{this.webHostEnvironment.WebRootPath}\\Reports\\K140.rdlc";
            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("K140", dt);
            
            var result = localReport.Execute(RenderType.Pdf, extension, paramtars, mimtype);

            return File(result.MainStream, "application/pdf");
        }

    }
}
