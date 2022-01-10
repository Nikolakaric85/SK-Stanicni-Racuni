using AspNetCore.Reporting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SK_Stanicni_Racuni.CustomModelBinding.RacuniUnutrasnjiSaobracaj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SK_Stanicni_Racuni.Controllers
{
    public class RacuniМedjunarodniSaobracajController : Controller
    {

        private ReportResult result; // za pdf izvestaj
        private readonly IWebHostEnvironment webHostEnvironment;

        public RacuniМedjunarodniSaobracajController(IWebHostEnvironment webHostEnvironment)
        {
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


            return new EmptyResult();

        }
    }
}
