using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using SK_Stanicni_Racuni.CustomModelBinding.StanicniRacunDatumi;
using SK_Stanicni_Racuni.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SK_Stanicni_Racuni.Controllers
{
    public class StanicniRacunController : Controller
    {
        private readonly AppDbContext context;
        private readonly INotyfService notyf;
        private readonly IWebHostEnvironment webHostEnvironment;
        private string blagajnik = string.Empty;

        public StanicniRacunController(AppDbContext context, INotyfService notyf, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.notyf = notyf;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult StanicniRacun()
        {

            var UserId = HttpContext.User.Identity.Name; // daje UserId
            var user = context.UserTabs.Where(x => x.UserId == UserId).FirstOrDefault();
            

            if (user != null)
            {
                

                if (user.Stanica.StartsWith("000"))
                {
                    ViewBag.blagajnik = user.Naziv.Trim();
                    ViewBag.Admin = true;
                }
                else
                {
                    ViewBag.Admin = false;
                    ViewBag.Stanica = context.ZsStanices.Where(x => x.SifraStanice1 == user.Stanica).FirstOrDefault().Naziv;
                    ViewBag.blagajnik = user.Naziv.Trim() + " " + user.Grupa.Trim();
                    ViewBag.SifraStanice = user.Stanica;
                    ViewBag.UserID = user.UserId;
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            //   ViewBag.Admin = false; // OVO OBRISATI, SETOVANO SAMO DA NE BI MORAO DA SE LOGUJEM SVAKI PUT

            ViewBag.stanicniRacuni = Enumerable.Empty<SrFaktura>();
            return View();
        }

        public IActionResult Prikazi(string stanica, string racunBr)
        {
            var sifraStanice = context.ZsStanices.Where(x => x.Naziv == stanica).Select(x => x.SifraStanice1).FirstOrDefault();

            ViewBag.blagajnik = blagajnik;

            if (!string.IsNullOrEmpty(stanica) && !string.IsNullOrEmpty(racunBr))
            {

                var query = context.SrFakturas.Where(x => x.Stanica == sifraStanice && x.FakturaBroj == racunBr);

                string Stanica = string.Empty, SifraStanice = string.Empty, MestoIzdavanjaRacuna = string.Empty, BlagajnaTip = string.Empty, Blagajna = string.Empty, TekuciRacun = string.Empty,
                    Mb = string.Empty, Pib = string.Empty, NazivPrimaocaRacuna = string.Empty, BrojUgovora = string.Empty, FakuraBroj = string.Empty, FakturaGodina = string.Empty,
                    Adresa = string.Empty, Telefon = string.Empty, PrialacTr = string.Empty, PrimalacMb = string.Empty, PrimalacPib = string.Empty, DatumNastankaObaveze = string.Empty,
                    DatumPlacanja = string.Empty, VrstaDobaraiUslugaOpis = string.Empty, DatumPrometaUsluga = string.Empty, JedMera = string.Empty, Kolicina = string.Empty, 
                    JedCenaDin = string.Empty, UkupnoDinara = string.Empty, PoreskaOsnovica = string.Empty, PDV = string.Empty, UkupnaVrednost = string.Empty, Fakturisao = string.Empty, DatumIzdavanja = string.Empty;

                foreach (var item in query)
                {
                    Stanica = sifraStanice;
                    SifraStanice = item.Stanica;
                    MestoIzdavanjaRacuna = stanica;     // ovo da se proveri da li ce tako ostati
                    BlagajnaTip = item.BlagajnaTip == "P" ? "Приспећа" : "Отправљања"  ;
                    Blagajna = item.Blagajna.ToString();
                    Mb = "211271116";                           //ovo videti da se prilikom upisa upise u bazu
                    Pib = "109108446";
                    FakuraBroj = item.FakturaBroj;
                    FakturaGodina = item.FakturaGodina.ToString();
                    NazivPrimaocaRacuna = item.Primalac;
                    Adresa = item.PrimalacAdresa;
                    Telefon = item.PrimalacTelefon;
                    PrialacTr = item.PrimalacTr;
                    PrimalacMb = item.PrimalacMb;
                    PrimalacPib = item.PrimalacPib;
                    DatumNastankaObaveze = item.FakturaDatum.HasValue ? item.FakturaDatum.Value.ToString("dd.MM.yyyy") : null;
                    DatumPlacanja = item.FakturaDatumP.HasValue ? item.FakturaDatumP.Value.ToString("dd.MM.yyyy") : null;
                    VrstaDobaraiUslugaOpis = item.VrstaUslugaOpis;
                    DatumPrometaUsluga = item.FakturaDatumPromet.HasValue ? item.FakturaDatumPromet.Value.ToString("dd.MM.yyyy") : null;
                    JedMera = item.Fjedinica;
                    Kolicina = item.Fkolicina;
                    JedCenaDin = item.Fjcena.ToString();
                    UkupnoDinara = item.FakturaOsnovica.ToString();
                    PoreskaOsnovica = item.FakturaOsnovica.ToString();
                    PDV = item.FakturaPdv.ToString();
                    UkupnaVrednost = item.FakturaTotal.ToString();
                    //Fakturisao 
                    DatumIzdavanja = item.DatumIzdavanja.HasValue ? item.DatumIzdavanja.Value.ToString("dd.MM.yyyy") : null;


                }

                var dt = new DataTable();
                string renderFormat = "PDF";
                string mimtype = "application/pdf";

                var localReport = new LocalReport();
                localReport.ReportPath = $"{this.webHostEnvironment.WebRootPath}\\Reports\\StanicniRacun.rdlc";
                localReport.DataSources.Add(new ReportDataSource("StanicniRacun",dt));
                var parametars = new[]
                {
                    new ReportParameter("Stanica",Stanica),
                    new ReportParameter("MestoIzdavanjaRacuna",MestoIzdavanjaRacuna),
                    new ReportParameter("BlagajnaTip", BlagajnaTip),
                    new ReportParameter("Blagajna", Blagajna),
                    new ReportParameter("Mb", Mb),
                    new ReportParameter("Pib", Pib),
                    new ReportParameter("FakuraBroj", FakuraBroj),
                    new ReportParameter("FakturaGodina", FakturaGodina),
                    new ReportParameter("NazivPrimaocaRacuna", NazivPrimaocaRacuna),
                    new ReportParameter("Adresa", Adresa),
                    new ReportParameter("Telefon", Telefon),
                    new ReportParameter("PrialacTr", PrialacTr),
                    new ReportParameter("PrimalacMb", PrimalacMb),
                    new ReportParameter("PrimalacPib", PrimalacPib),
                    new ReportParameter("DatumNastankaObaveze", DatumNastankaObaveze),
                    new ReportParameter("DatumPlacanja", DatumPlacanja),
                    new ReportParameter("VrstaDobaraiUslugaOpis", VrstaDobaraiUslugaOpis),
                    new ReportParameter("DatumPrometaUsluga", DatumPrometaUsluga),
                    new ReportParameter("JedMera", JedMera),
                    new ReportParameter("JedCenaDin", JedCenaDin),
                    new ReportParameter("UkupnoDinara", UkupnoDinara),
                    new ReportParameter("PoreskaOsnovica", PoreskaOsnovica),
                    new ReportParameter("PDV", PDV),
                    new ReportParameter("UkupnaVrednost", UkupnaVrednost),
                    new ReportParameter("Fakturisao", Fakturisao),
                    new ReportParameter("DatumIzdavanja", DatumIzdavanja),
              
                };

                localReport.SetParameters(parametars);
                var pdf = localReport.Render(renderFormat);
                return File(pdf, mimtype);




            }
            else if (!string.IsNullOrEmpty(stanica) && string.IsNullOrEmpty(racunBr))
            {

                var query = context.SrFakturas.Where(x => x.Stanica == sifraStanice).Select(x => new { x.Stanica, x.FakturaBroj}).AsEnumerable();
              //  ViewBag.stanicniRacuni = query;
                return View("StanicniRacun");

                // ovde ide popunjavanje GRID-a faktura broj je racun broj
            }

            return RedirectToAction("StanicniRacun");
        }


        public IActionResult Save(SrFaktura model, string tipBlagajna,
            [ModelBinder(typeof(FakturaDatumModelBinder))] DateTime FakturaDatum,
            [ModelBinder(typeof(DatumIzdavanjaModelBinder))] DateTime DatumIzadavanja,
            [ModelBinder(typeof(FakturaDatumPModelBinder))] DateTime FakturaDatumP,
            [ModelBinder(typeof(FakturaDatumPuModelBinder))] DateTime FakturaDatumPU)
        {

            var query = context.SrFakturas.Where(x => x.Stanica == model.Stanica && x.FakturaBroj == model.FakturaBroj && x.FakturaGodina == model.FakturaGodina);

            if (query.Any())
            {
                notyf.Error("Postoji već račun pod tim brojem.",5);
                ViewBag.stanicniRacuni = Enumerable.Empty<SrFaktura>();
                return View("StanicniRacun", model);
            }

            string nullDate = "1/1/0001 12:00:00 AM";

            model.FakturaDatum = FakturaDatum.ToString() != nullDate ? FakturaDatum : null;
            model.DatumIzdavanja = DatumIzadavanja.ToString() != nullDate ? DatumIzadavanja : null ;
            model.FakturaDatumP = FakturaDatumP.ToString() != nullDate ? FakturaDatumP : null ;
            model.FakturaDatumPromet = FakturaDatumPU.ToString() != nullDate ? FakturaDatumPU: null ;

            model.BlagajnaTip = tipBlagajna == "P" ? "P" : "O";




            context.SrFakturas.Add(model);
         //   context.SaveChanges();

            return RedirectToAction("StanicniRacun");
        }


    }
}
