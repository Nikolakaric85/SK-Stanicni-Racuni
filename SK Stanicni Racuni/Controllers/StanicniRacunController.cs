using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Reporting.NETCore;
using SK_Stanicni_Racuni.Classes;
using SK_Stanicni_Racuni.CustomModelBinding.StanicniRacunDatumi;
using SK_Stanicni_Racuni.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SK_Stanicni_Racuni.Controllers
{
    public class StanicniRacunController : Controller
    {
        private readonly AppDbContext context;
        private readonly INotyfService notyf;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserLogin userLogin;
        private string blagajnik = string.Empty;
        CultureInfo elGR = CultureInfo.CreateSpecificCulture("el-GR");  //koristim da na izvestaju thousand separator 100.000. i slicno

        public StanicniRacunController(AppDbContext context, INotyfService notyf, IWebHostEnvironment webHostEnvironment, UserLogin userLogin )
        {
            this.context = context;
            this.notyf = notyf;
            this.webHostEnvironment = webHostEnvironment;
            this.userLogin = userLogin;
        }

        public IActionResult StanicniRacun()
        {

         //   var user3 = userLogin.LoggedInUser();

            var UserId = HttpContext.User.Identity.Name; // daje UserId
            var user = context.UserTabs.Where(x => x.UserId == UserId).FirstOrDefault();

            if (user != null)
            {
                if (user.Stanica.StartsWith("000"))
                {
                    ViewBag.blagajnik = user.Naziv.Trim();
                    ViewBag.Admin = true;
                    TempData["blagajnik"] = user.Naziv.Trim();
                    ViewBag.stanicniRacuni = context.SrFakturas.Where(x => x.Realizovano == "N");
                }
                else
                {
                    ViewBag.Admin = false;
                    var pripadajucaStanica = context.ZsStanices.Where(x => x.SifraStanice1 == user.Stanica).Select(x => new { x.Naziv, x.Mesto} ).FirstOrDefault();
                    ViewBag.Stanica = pripadajucaStanica.Naziv;
                    ViewBag.MestoIzdavanjaRacuna = pripadajucaStanica.Mesto;
                    ViewBag.SifraBlagajne = context.ElsSkStaniceRacunis.Where(x => x.Sifra == user.Stanica).Select(x => x.SifraBlagajne).FirstOrDefault();
                    ViewBag.blagajnik = user.Naziv.Trim() + " " + user.Grupa.Trim();
                    TempData["blagajnik"] = user.Naziv.Trim();
                    ViewBag.SifraStanice = user.Stanica;
                    ViewBag.UserID = user.UserId;

                    ViewBag.stanicniRacuni = context.SrFakturas.Where(x => x.Stanica == user.Stanica);
                }

                ViewBag.BlagajnaTip = new SelectList(BlagajnaTip());

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            // ViewBag.Admin = true; // OVO OBRISATI, SETOVANO SAMO DA NE BI MORAO DA SE LOGUJEM SVAKI PUT

            var stanicaKojeNisuGranicne = context.ZsStanices.Where(x => x.Prikaz == "N" && x.SifraStanice1 == user.Stanica);

            if (stanicaKojeNisuGranicne.Any())
            {
                notyf.Information("Nemate prava pristupa.", 3);
                return RedirectToAction("Index", "Home");
            }


            //za PDF prikaz poslednjeg unosa
            if (TempData["PoslednjiUnos"] != null)
            {
                bool poslednjiUnos = (bool)TempData["PoslednjiUnos"];
                if (poslednjiUnos == true)
                {
                    ViewBag.PDFbtn = true;
                }
            }

           // ViewBag.stanicniRacuni = Enumerable.Empty<SrFaktura>();
            return View();
        }


        public List<string> BlagajnaTip()
        {
            List<string> blagajnaTip = new List<string>()
            { "" , "Prispeća", "Otpravljanja" };

            return blagajnaTip;
        }

        public IActionResult PDF(string stanica, string racunBr, int fakturaGod)
        {

            //kada iz tabele poziva PDF onda proverava da li je parametar stanica string ili int
            var stanicaInt = int.TryParse(stanica,  out int result );

            var sifraStanice = stanicaInt == true ? stanica : context.ZsStanices.Where(x => x.Naziv == stanica).Select(x => x.SifraStanice1).FirstOrDefault();

            if (!string.IsNullOrEmpty(stanica) && !string.IsNullOrEmpty(racunBr))
            {

                var query = context.SrFakturas.Where(x => x.Stanica == sifraStanice && x.FakturaBroj == racunBr);

                if (!query.Any())
                {
                    notyf.Error("Nema podataka za izabrane kriterijume.", 3);
                    return RedirectToAction("StanicniRacun");
                }

                string Stanica = string.Empty, SifraStanice = string.Empty, MestoIzdavanjaRacuna = string.Empty, BlagajnaTip = string.Empty, Blagajna = string.Empty, TekuciRacun = string.Empty,
                    Mb = string.Empty, Pib = string.Empty, NazivPrimaocaRacuna = string.Empty, BrojUgovora = string.Empty, FakuraBroj = string.Empty, FakturaGodina = string.Empty,
                    Adresa = string.Empty, Telefon = string.Empty, PrialacTr = string.Empty, PrimalacMb = string.Empty, PrimalacPib = string.Empty, DatumNastankaObaveze = string.Empty,
                    DatumPlacanja = string.Empty, VrstaDobaraiUslugaOpis = string.Empty, DatumPrometaUsluga = string.Empty, JedMera = string.Empty, Kolicina = string.Empty, 
                    JedCenaDin = string.Empty, UkupnoDinara = string.Empty, /*PoreskaOsnovica = string.Empty, */PDV = string.Empty, UkupnaVrednost = string.Empty, Fakturisao = string.Empty, DatumIzdavanja = string.Empty,
                    SifraBlagajne = string.Empty;

                decimal PoreskaOsnovica = 0;
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
                    DatumNastankaObaveze = item.FakturaDatum.HasValue ? item.FakturaDatum.Value.ToString("dd.MM.yyyy") : string.Empty;
                    DatumPlacanja = item.FakturaDatumP.HasValue ? item.FakturaDatumP.Value.ToString("dd.MM.yyyy") : string.Empty;
                    VrstaDobaraiUslugaOpis = item.VrstaUslugaOpis;
                    DatumPrometaUsluga = item.FakturaDatumPromet.HasValue ? item.FakturaDatumPromet.Value.ToString("dd.MM.yyyy") : string.Empty;
                    JedMera = item.Fjedinica;
                    Kolicina = item.Fkolicina;
                    JedCenaDin = item.Fjcena.ToString();
                    UkupnoDinara = item.FakturaOsnovica.ToString();
                    PoreskaOsnovica = (decimal)item.FakturaOsnovica;
                    PDV = item.FakturaPdv.ToString();
                    UkupnaVrednost = item.FakturaTotal.ToString();
                    //Fakturisao 
                    DatumIzdavanja = item.DatumIzdavanja.HasValue ? item.DatumIzdavanja.Value.ToString("dd.MM.yyyy") : string.Empty;
                    blagajnik = context.UserTabs.Where(x => x.UserId == item.Blagajnik).Select(x => x.Naziv.Trim() +" " + x.Grupa.Trim() ).FirstOrDefault();
                }

                SifraBlagajne = context.ElsSkStaniceRacunis.Where(x => x.Sifra == sifraStanice).Select(x => x.SifraBlagajne.ToString()).FirstOrDefault();

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
                    new ReportParameter("Kolicina", Kolicina),
                    new ReportParameter("JedCenaDin", JedCenaDin),
                    new ReportParameter("UkupnoDinara", string.Format(elGR,"{0:0,0}", Double.Parse(UkupnoDinara))),
                    new ReportParameter("PoreskaOsnovica", string.Format(elGR,"{0:0,0}", PoreskaOsnovica)),
                    new ReportParameter("PDV", string.Format(elGR,"{0:0,0}", Double.Parse(PDV))),
                    new ReportParameter("UkupnaVrednost", string.Format(elGR,"{0:0,0}", Double.Parse(UkupnaVrednost))),
                    new ReportParameter("Fakturisao", Fakturisao),
                    new ReportParameter("DatumIzdavanja", DatumIzdavanja),
                    new ReportParameter("SifraBlagajne", SifraBlagajne),
                    new ReportParameter("Blagajnik", blagajnik),

                };

                localReport.SetParameters(parametars);
                var pdf = localReport.Render(renderFormat);
                return File(pdf, mimtype);

            }
            else if (!string.IsNullOrEmpty(stanica) && !string.IsNullOrEmpty(fakturaGod.ToString()) && string.IsNullOrEmpty(racunBr))
            {
                // ovde ide popunjavanje GRID-a faktura broj je racun broj

                var query = context.SrFakturas.Where(x => x.Stanica == sifraStanice && x.FakturaGodina == fakturaGod && x.Realizovano == "N").AsEnumerable();

                if (!query.Any())
                {
                    notyf.Error("Nema podataka za izabrane kriterijume.", 3);
                    return RedirectToAction("StanicniRacun");
                }

                ViewBag.stanicniRacuni = query;
                ViewBag.Admin = true;
                ViewBag.Stanica = stanica;
                ViewBag.FakturaGod = fakturaGod;

                return View("StanicniRacun");

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

            try
            {
                notyf.Success("Uspešno uneti podaci.", 3);
                context.SrFakturas.Add(model);
                context.SaveChanges();

                TempData["PoslednjiUnos"] = true;
                TempData["Stanica"] = model.Stanica;
                TempData["FakturaBroj"] = model.FakturaBroj;
               
            }
            catch (Exception)
            {
                notyf.Error("Greška prilikom upisa podakatak.", 3);
            }

            return RedirectToAction("StanicniRacun");
        }

        public IActionResult LastEntryPDF()
        {
            string stanica = (string)TempData["Stanica"];
            string racunBr = (string)TempData["FakturaBroj"];
            return RedirectToAction("PDF", new { stanica, racunBr });
        }

        public IActionResult Edit(string stanica, string racunBr, int fakturaGod)
        {
            var user = userLogin.LoggedInUser();

            ViewBag.Admin = false;
            var pripadajucaStanica = context.ZsStanices.Where(x => x.SifraStanice1 == user.Stanica).Select(x => new { x.Naziv, x.Mesto }).FirstOrDefault();
            ViewBag.Stanica = pripadajucaStanica.Naziv;
            ViewBag.MestoIzdavanjaRacuna = pripadajucaStanica.Mesto;
            ViewBag.SifraBlagajne = context.ElsSkStaniceRacunis.Where(x => x.Sifra == user.Stanica).Select(x => x.SifraBlagajne).FirstOrDefault();
            ViewBag.blagajnik = user.Naziv.Trim() + " " + user.Grupa.Trim();
            TempData["blagajnik"] = user.Naziv.Trim();
            ViewBag.SifraStanice = user.Stanica;
            ViewBag.UserID = user.UserId;

            var query = context.SrFakturas.Where(x => x.Stanica == stanica && x.FakturaBroj == racunBr && x.FakturaGodina == fakturaGod).FirstOrDefault();

            @ViewBag.Putanja = query.Fpath; // NE PRIKAZUJE IME FAJLA. OVO SA VIEWBAGO-OM NE RADI

            var tipBlagajne = query.BlagajnaTip == "P" ? "Prispeća" : "Otpravljanja";
            ViewBag.BlagajnaTip = new SelectList(BlagajnaTip(), tipBlagajne);

            ViewBag.stanicniRacuni = context.SrFakturas.Where(x => x.Stanica == user.Stanica);
            return View("StanicniRacun", query);
        }


        public IActionResult EditSave(SrFaktura model, string tipBlagajna,
            [ModelBinder(typeof(FakturaDatumModelBinder))] DateTime FakturaDatum,
            [ModelBinder(typeof(DatumIzdavanjaModelBinder))] DateTime DatumIzadavanja,
            [ModelBinder(typeof(FakturaDatumPModelBinder))] DateTime FakturaDatumP,
            [ModelBinder(typeof(FakturaDatumPuModelBinder))] DateTime FakturaDatumPU)
        {

            string nullDate = "1/1/0001 12:00:00 AM";

            model.FakturaDatum = FakturaDatum.ToString() != nullDate ? FakturaDatum : null;
            model.DatumIzdavanja = DatumIzadavanja.ToString() != nullDate ? DatumIzadavanja : null;
            model.FakturaDatumP = FakturaDatumP.ToString() != nullDate ? FakturaDatumP : null;
            model.FakturaDatumPromet = FakturaDatumPU.ToString() != nullDate ? FakturaDatumPU : null;

            model.BlagajnaTip = tipBlagajna == "Prispeća" ? "P" : "O";

            return RedirectToAction("StanicniRacun");
        }


        public IActionResult Realizovano(string stanica, string racunBr, int fakturaGod)
        {
            var query = context.SrFakturas.Where(x => x.Stanica == stanica && x.FakturaBroj == racunBr && x.FakturaGodina == fakturaGod).FirstOrDefault();
            query.DatumR = DateTime.Now;
            query.Realizovano = "D";

            try
            {
                var update = context.SrFakturas.Attach(query);
                update.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
                notyf.Success("Uspešna izmena", 3);
            }
            catch (Exception)
            {
                notyf.Error("Greška prilikom realziacije.", 5);
            }
            
            return RedirectToAction("StanicniRacun");

        }


    }
}
