using AspNetCore.Reporting;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SK_Stanicni_Racuni.CustomModelBinding.Datumi;
using SK_Stanicni_Racuni.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SK_Stanicni_Racuni.Controllers
{
    public class RekapitulacijaController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly INotyfService notyf;

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
                ViewBag.UserId = UserId;
                ViewBag.Stanica = context.ZsStanices.Where(x => x.SifraStanice1 == user.Stanica).FirstOrDefault().Naziv;
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

            var sifraStanice = context.ZsStanices.Where(x => x.Naziv == stanica).FirstOrDefault();

            var query = from s in context.SrK161fs
                        where (s.FakturaDatum >= DatumOd || s.FakturaDatum <= DatumDo)
                        && s.Blagajna == Int32.Parse(blagajna)
                        && s.Stanica == sifraStanice.SifraStanice
                        select s;

            var dt = new DataTable();

            dt.Columns.Add("BrojTovarnogLista");
            dt.Columns.Add("PredatoDana");
            dt.Columns.Add("UputnaStanica");
            dt.Columns.Add("BrojKola");
            dt.Columns.Add("NaznacenjeVrsteRobe");
            dt.Columns.Add("NavedenoUtovarnomListu");
            dt.Columns.Add("IBK");

            DataRow row;

            foreach (var item in query)
            {
                row = dt.NewRow();
                row["FakturaBroj"] = item.Broj_tovarnog_lista;
                row["FakturaDatum"] = item.Predato_dana.ToString("dd.MM.yyyy");
                row["FakturalniIznos_pare"] = item.Uputna_stanica;
                row["FakturaDatum5"] = item.Broj_Kola;
                row["FakturalniIznos6a"] = item.Naznačenje_vrste_robe;
                row["FakturalniIznos6a_pare"] = item.Navedena_u_tovarnom_listu;
                row["FakturaDatum7"] = item.IBK;
                row["FakturalniIznos8"] = item.IBK;
                row["FakturalniIznos8_pare"] = item.IBK;
                dt.Rows.Add(row);
            }

            string mimtype = "";
            int extension = 1;

            Dictionary<string, string> paramtars = new Dictionary<string, string>();

            paramtars.Add("Stanica", sifraStanice);
            paramtars.Add("DatumOd", DatumOd.ToString());
            paramtars.Add("DatumDo", DatumDo.ToString());

            var path = $"{this.webHostEnvironment.WebRootPath}\\Reports\\K117.rdlc";
            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("K117", dt);
            localReport.AddDataSource("Stanica", sifraStanice);
            localReport.AddDataSource("DatumOd", DatumOd.ToString());
            localReport.AddDataSource("DatumDo", DatumDo.ToString());
            Random random = new Random();
            extension = random.Next();
            ReportResult result = localReport.Execute(RenderType.Pdf, 1, paramtars, mimtype);
            return File(result.MainStream, "application/pdf");

            
        }
    }
}
