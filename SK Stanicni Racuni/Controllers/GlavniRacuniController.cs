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
            //else
            //{
            //    return RedirectToAction("Login", "Account");
            //}

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
                var dt = new DataTable();

                // DataRow row;

                string renderFormat = "PDF";
                string mimtype = "application/pdf";

                var localReport = new LocalReport();
                localReport.ReportPath = $"{this.webHostEnvironment.WebRootPath}\\Reports\\K157.rdlc";
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

            return new EmptyResult();
            

        }

    }
}
