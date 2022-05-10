using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Reporting.NETCore;
using SK_Stanicni_Racuni.Classes;
using SK_Stanicni_Racuni.CustomModelBinding.StanicniRacunDatumi;
using SK_Stanicni_Racuni.Models;
using SK_Stanicni_Racuni.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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
        private readonly IMapper mapper;
        private readonly DirectoryAndFiles directoryAndFiles;
        private readonly RealizovanoPrilog realizovanoPrilog;
        private string blagajnik = string.Empty;
        CultureInfo elGR = CultureInfo.CreateSpecificCulture("el-GR");  //koristim da na izvestaju thousand separator 100.000. i slicno

        public StanicniRacunController(AppDbContext context, INotyfService notyf, IWebHostEnvironment webHostEnvironment, UserLogin userLogin,
            IMapper mapper, DirectoryAndFiles directoryAndFiles, RealizovanoPrilog realizovanoPrilog)
        {
            this.context = context;
            this.notyf = notyf;
            this.webHostEnvironment = webHostEnvironment;
            this.userLogin = userLogin;
            this.mapper = mapper;
            this.directoryAndFiles = directoryAndFiles;
            this.realizovanoPrilog = realizovanoPrilog;
        }

        public IActionResult StanicniRacun(string stanica, int fakturaGod, bool sveFakture, bool samoNfakture)
        {
            var user = userLogin.LoggedInUser();

            if (user != null)
            {
                if (user.Stanica.StartsWith("00099"))
                {
                    ViewBag.blagajnik = user.Naziv.Trim();
                    ViewBag.Admin = true;
                    TempData["blagajnik"] = user.Naziv.Trim();

                    var model = context.SrFakturas.Where(x => x.Realizovano == "N");
                    var viewModelList = mapper.Map<List<SrFakturaViewModel>>(model);

                    foreach (var item in viewModelList)
                    {
                        item.NazivStanice = context.ZsStanices.Where(x => x.SifraStanice1 == item.Stanica).Select(x => x.Naziv).FirstOrDefault();
                    }

                    ViewBag.stanicniRacuni = viewModelList.OrderByDescending(x => x.DatumIzdavanja);
                    ViewBag.sveFakture = sveFakture;
                    ViewBag.samoNfakture = sveFakture == true ? false : true;
                    ViewBag.FakturaGod = fakturaGod != 0 && fakturaGod != DateTime.Now.Year ? fakturaGod : DateTime.Now.Year;
                    ViewBag.Stanica = stanica;
                }
                else if(!user.Stanica.StartsWith("00099") && !user.Stanica.StartsWith("000"))
                {
                    ViewBag.Admin = false;
                    var pripadajucaStanica = context.ZsStanices.Where(x => x.SifraStanice1 == user.Stanica).Select(x => new { x.Naziv, x.Mesto} ).FirstOrDefault();
                    if (pripadajucaStanica != null)
                    {
                        ViewBag.Stanica = pripadajucaStanica.Naziv;
                        TempData["StanicaNaziv"] = pripadajucaStanica.Naziv;
                        ViewBag.MestoIzdavanjaRacuna = pripadajucaStanica.Mesto;
                    }
                    
                    ViewBag.SifraBlagajne = context.ElsSkStaniceRacunis.Where(x => x.Sifra == user.Stanica).Select(x => x.SifraBlagajne).FirstOrDefault();
                    TempData["SifraBlagajne"] = ViewBag.SifraBlagajne;
                    ViewBag.blagajnik = user.Naziv.Trim() + " " + user.Grupa.Trim();
                    TempData["blagajnik"] = user.Naziv.Trim();
                    ViewBag.SifraStanice = user.Stanica;
                    ViewBag.UserID = user.UserId;

                    var model = context.SrFakturas.Where(x => x.Stanica == user.Stanica);
                    var viewModelList = mapper.Map<List<SrFakturaViewModel>>(model);

                    foreach (var item in viewModelList)
                    {
                        item.NazivStanice = context.ZsStanices.Where(x => x.SifraStanice1 == item.Stanica).Select(x => x.Naziv).FirstOrDefault();
                    }

                    ViewBag.stanicniRacuni = viewModelList.OrderByDescending(x => x.DatumIzdavanja);
                } else
                {
                    notyf.Error("Nemate prava pristupa.", 3);
                    return RedirectToAction("Index", "Home");
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
            return View();
        }


        public List<string> BlagajnaTip()
        {
            List<string> blagajnaTip = new List<string>()
            { "" , "Prispeća", "Otpravljanja" };

            return blagajnaTip;
        }

        public IActionResult PDF(string stanica, string racunBr, int fakturaGod, bool sveFakture, bool samoNfakture)
        {

            TempData["stancaPretraga"] = stanica;
            TempData["racunBrPretraga"] = racunBr;
            TempData["fakturaGodPretraga"] = fakturaGod;
            TempData["sveFakturePretraga"] = sveFakture;
            TempData["samoNfakturePretraga"] = samoNfakture;

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
                    JedCenaDin = string.Empty, UkupnoDinara = string.Empty, PoreskaOsnovica = string.Empty, PDV = string.Empty, UkupnaVrednost = string.Empty, Fakturisao = string.Empty, DatumIzdavanja = string.Empty,
                    SifraBlagajne = string.Empty, MestoStacice = string.Empty, NazivStnice = string.Empty, FakturaTekst = string.Empty, MestoPrimaoca = string.Empty;

                //decimal PoreskaOsnovica = 0;
                foreach (var item in query)
                {
                    Stanica = sifraStanice;
                    NazivStnice = context.ZsStanices.Where(x => x.SifraStanice1 == sifraStanice).Select(x => x.Naziv).FirstOrDefault();
                    MestoStacice = context.ZsStanices.Where(x => x.SifraStanice1 == sifraStanice).Select(x => x.Mesto).FirstOrDefault();
                    SifraStanice = item.Stanica;
                    BlagajnaTip = item.BlagajnaTip == "P" ? "Приспећа" : "Отправљања"  ;
                    Blagajna = item.Blagajna.ToString();
                    Mb = "211271116";                           //ovo videti da se prilikom upisa upise u bazu
                    Pib = "109108446";
                    TekuciRacun = item.TekuciRacun;
                    FakuraBroj = item.FakturaBroj;
                    FakturaGodina = item.FakturaGodina.ToString();
                    NazivPrimaocaRacuna = item.Primalac;
                    MestoPrimaoca = item.PrimalacMesto;
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
                    UkupnoDinara = item.Fiznos.ToString();
                    PoreskaOsnovica = item.FakturaOsnovica.ToString();
                    PDV = item.FakturaPdv.ToString();
                    UkupnaVrednost = item.FakturaTotal.ToString();
                    //Fakturisao 
                    DatumIzdavanja = item.DatumIzdavanja.HasValue ? item.DatumIzdavanja.Value.ToString("dd.MM.yyyy") : string.Empty;
                    blagajnik = context.UserTabs.Where(x => x.UserId == item.Blagajnik).Select(x => x.Naziv.Trim() +" " + x.Grupa.Trim() ).FirstOrDefault();
                    FakturaTekst = item.FakturaTekst;
                }

                SifraBlagajne = context.ElsSkStaniceRacunis.Where(x => x.Sifra == sifraStanice).Select(x => x.SifraBlagajne.ToString()).FirstOrDefault();

                string[] _ukupnoDinara = new string[2];
                string[] _pOsnovica = new string[2];
                string[] _pdv = new string[2];
                string[] _ukupnaVrednost = new string[2];
                string[] _jedCenaDinara = new string[2];

                if (UkupnoDinara == "0" || string.IsNullOrEmpty(UkupnoDinara))
                {
                    _ukupnoDinara[0] = "0";
                    _ukupnoDinara[1] = "0";
                }
                else
                {
                    _ukupnoDinara = UkupnoDinara.Split(".");
                }


                if (PoreskaOsnovica == "0" || string.IsNullOrEmpty(PoreskaOsnovica))
                {
                    _pOsnovica[0] = "0";
                    _pOsnovica[1] = "0";
                }
                else
                {
                    _pOsnovica = PoreskaOsnovica.Split(".");
                }

                if (PDV == "0" || string.IsNullOrEmpty(PDV))
                {
                    _pdv[0] = "0";
                    _pdv[1] = "0";
                }
                else
                {
                    _pdv = PDV.Split(".");
                }

                if (UkupnaVrednost == "0" || string.IsNullOrEmpty(UkupnaVrednost))
                {
                    _ukupnaVrednost[0] = "0";
                    _ukupnaVrednost[1] = "0";
                }
                else
                {
                    _ukupnaVrednost = UkupnaVrednost.Split(".");
                }

                if (JedCenaDin == "0" || string.IsNullOrEmpty(JedCenaDin))
                {
                    _jedCenaDinara[0] = "0";
                    _jedCenaDinara[1] = "0";
                }
                else
                {
                    _jedCenaDinara = JedCenaDin.Split(".");
                }

                var dt = new DataTable();
                string renderFormat = "PDF";
                string mimtype = "application/pdf";
                var ftm = new NumberFormatInfo();
                ftm.NegativeSign = "-";
                var localReport = new LocalReport();
                localReport.ReportPath = $"{this.webHostEnvironment.WebRootPath}\\Reports\\StanicniRacun.rdlc";
                localReport.DataSources.Add(new ReportDataSource("StanicniRacun",dt));
                var parametars = new[]
                {
                    new ReportParameter("Stanica",sifraStanice),
                    new ReportParameter("MestoStanice",MestoStacice),
                    new ReportParameter("NazivStanice",NazivStnice),
                    new ReportParameter("BlagajnaTip", BlagajnaTip),
                    new ReportParameter("Blagajna", Blagajna),
                    new ReportParameter("Mb", Mb),
                    new ReportParameter("Pib", Pib),
                    new ReportParameter("TekuciRacun", TekuciRacun),
                    new ReportParameter("FakuraBroj", FakuraBroj),
                    new ReportParameter("FakturaGodina", FakturaGodina),
                    new ReportParameter("NazivPrimaocaRacuna", NazivPrimaocaRacuna),
                    new ReportParameter("MestoPrimaoca", MestoPrimaoca),
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

                    new ReportParameter("JedCenaDin", string.Format(elGR,"{0:0,0}", Double.Parse(_jedCenaDinara[0], ftm))),
                    new ReportParameter("JedCenaDinDecimal", _jedCenaDinara[1]),

                    new ReportParameter("UkupnoDinara", string.Format(elGR,"{0:0,0}", Double.Parse(_ukupnoDinara[0], ftm))),
                    new ReportParameter("UkupnoDinaraDecimal", _ukupnoDinara[1]),

                    new ReportParameter("PoreskaOsnovica", string.Format(elGR,"{0:0,0}", Double.Parse(_pOsnovica[0], ftm))),
                    new ReportParameter("PoreskaOsnovicaDecimal", _pOsnovica[1]),

                    new ReportParameter("PDV", string.Format(elGR,"{0:0,0}", Double.Parse(_pdv[0],ftm))),
                    new ReportParameter("PDV_Decimal", _pdv[1]),

                    new ReportParameter("UkupnaVrednost", string.Format(elGR,"{0:0,0}", Double.Parse(_ukupnaVrednost[0], ftm))),
                    new ReportParameter("UkupnaVrednostDecimal", _ukupnaVrednost[1]),

                    new ReportParameter("Fakturisao", Fakturisao),
                    new ReportParameter("DatumIzdavanja", DatumIzdavanja),
                    new ReportParameter("SifraBlagajne", SifraBlagajne),
                    new ReportParameter("Blagajnik", blagajnik),
                    new ReportParameter("FakturaTekst", FakturaTekst)
                    

                };

                localReport.SetParameters(parametars);
                var pdf = localReport.Render(renderFormat);
                return File(pdf, mimtype);

            }                                                                                                                                   //***************** ODAVDE IDE PRETRAGA NA ADMIN FORMI
            else if (!string.IsNullOrEmpty(stanica) && fakturaGod !=0  && string.IsNullOrEmpty(racunBr))
            {
                // ovde ide popunjavanje GRID-a faktura broj je racun broj

                var klas = new PretragaStanicniRacun(stanica, racunBr, fakturaGod,  sveFakture, samoNfakture);

                var query = Enumerable.Empty<SrFaktura>();

                if (sveFakture)
                {
                    ViewBag.sveFakture = true;  //sluzi za setovanje na formi chekbox-va, sta je izabrano da to i ostane
                    ViewBag.samoNfakture = false;
                    query = context.SrFakturas.Where(x => x.Stanica == sifraStanice && x.FakturaGodina == fakturaGod).AsEnumerable();
                } else
                {
                    ViewBag.sveFakture = false;
                    ViewBag.samoNfakture = true;
                    query = context.SrFakturas.Where(x => x.Stanica == sifraStanice && x.FakturaGodina == fakturaGod && x.Realizovano == "N").AsEnumerable();
                }

                if (!query.Any())
                {
                    TempData.Remove("stancaPretraga"); // uklanja ove podatke jer ako se pre toga kliknulo na dugme PRETRAŽI, onda kada se klikne na dugme PRILOG popuni polja za stanicu i godinu
                    TempData.Remove("fakturaGodPretraga");
                    notyf.Error("Nema podataka za izabrane kriterijume.", 3);   
                    return RedirectToAction("StanicniRacun", new {  stanica,  fakturaGod,  sveFakture,  samoNfakture });
                }

                var viewModelList = mapper.Map<List<SrFakturaViewModel>>(query);

                foreach (var item in viewModelList)
                {
                    item.NazivStanice = context.ZsStanices.Where(x => x.SifraStanice1 == item.Stanica).Select(x => x.Naziv).FirstOrDefault();
                }

                ViewBag.stanicniRacuni = viewModelList.OrderByDescending(x => x.DatumIzdavanja);
                ViewBag.Admin = true;
                ViewBag.Stanica = stanica;
                ViewBag.FakturaGod = fakturaGod;

                return View("StanicniRacun");
            }
            else if (string.IsNullOrEmpty(stanica) && fakturaGod == 0 && string.IsNullOrEmpty(racunBr))
            {
                // kada vrsi pretragu samo po checkbox-vima

                var query = Enumerable.Empty<SrFaktura>();

                if (sveFakture)
                {
                    ViewBag.sveFakture = true;  //sluzi za setovanje na formi chekbox-va, sta je izabrano da to i ostane
                    ViewBag.samoNfakture = false;
                    query = context.SrFakturas.AsEnumerable();
                }
                else
                {
                    ViewBag.sveFakture = false;
                    ViewBag.samoNfakture = true;
                    query = context.SrFakturas.Where(x => x.Realizovano == "N").AsEnumerable();
                }

                if (!query.Any())
                {
                    TempData.Remove("stancaPretraga"); // uklanja ove podatke jer ako se pre toga kliknulo na dugme PRETRAŽI, onda kada se klikne na dugme PRILOG popuni polja za stanicu i godinu
                    TempData.Remove("fakturaGodPretraga");
                    notyf.Error("Nema podataka za izabrane kriterijume.", 3);
                    return RedirectToAction("StanicniRacun", new { stanica, fakturaGod, sveFakture, samoNfakture });
                }

                var viewModelList = mapper.Map<List<SrFakturaViewModel>>(query);

                foreach (var item in viewModelList)
                {
                    item.NazivStanice = context.ZsStanices.Where(x => x.SifraStanice1 == item.Stanica).Select(x => x.Naziv).FirstOrDefault();
                }

                ViewBag.stanicniRacuni = viewModelList.OrderByDescending(x => x.DatumIzdavanja);
                ViewBag.Admin = true;
                ViewBag.FakturaGod = string.Empty;

                return View("StanicniRacun");
            }
            else if (string.IsNullOrEmpty(stanica) && fakturaGod != 0 && string.IsNullOrEmpty(racunBr))
            {
                // kada vrsi pretragu samo po godini i checkboxovima

                var query = Enumerable.Empty<SrFaktura>();

                if (sveFakture)
                {
                    ViewBag.sveFakture = true;  //sluzi za setovanje na formi chekbox-va, sta je izabrano da to i ostane
                    ViewBag.samoNfakture = false;
                    query = context.SrFakturas.Where(x=>x.FakturaGodina == fakturaGod).AsEnumerable();
                }
                else
                {
                    ViewBag.sveFakture = false;
                    ViewBag.samoNfakture = true;
                    query = context.SrFakturas.Where(x =>  x.FakturaGodina == fakturaGod && x.Realizovano == "N").AsEnumerable();
                }

                if (!query.Any())
                {
                    TempData.Remove("stancaPretraga"); // uklanja ove podatke jer ako se pre toga kliknulo na dugme PRETRAŽI, onda kada se klikne na dugme PRILOG popuni polja za stanicu i godinu
                    TempData.Remove("fakturaGodPretraga");
                    notyf.Error("Nema podataka za izabrane kriterijume.", 3);
                    return RedirectToAction("StanicniRacun", new { stanica, fakturaGod, sveFakture, samoNfakture });
                }

                var viewModelList = mapper.Map<List<SrFakturaViewModel>>(query);

                foreach (var item in viewModelList)
                {
                    item.NazivStanice = context.ZsStanices.Where(x => x.SifraStanice1 == item.Stanica).Select(x => x.Naziv).FirstOrDefault();
                }

                ViewBag.stanicniRacuni = viewModelList.OrderByDescending(x => x.DatumIzdavanja);
                ViewBag.Admin = true;
                ViewBag.FakturaGod = fakturaGod;

                return View("StanicniRacun");


            }
            else if (!string.IsNullOrEmpty(stanica) && fakturaGod == 0 && string.IsNullOrEmpty(racunBr))
            {
                // kada vrsi pretragu samo po nazivu stanice i checkboxovima

                var query = Enumerable.Empty<SrFaktura>();

                if (sveFakture)
                {
                    ViewBag.sveFakture = true;  //sluzi za setovanje na formi chekbox-va, sta je izabrano da to i ostane
                    ViewBag.samoNfakture = false;
                    query = context.SrFakturas.Where(x => x.Stanica == sifraStanice).AsEnumerable();
                }
                else
                {
                    ViewBag.sveFakture = false;
                    ViewBag.samoNfakture = true;
                    query = context.SrFakturas.Where(x => x.Stanica == sifraStanice && x.Realizovano == "N").AsEnumerable();
                }

                if (!query.Any())
                {
                    TempData.Remove("stancaPretraga"); // uklanja ove podatke jer ako se pre toga kliknulo na dugme PRETRAŽI, onda kada se klikne na dugme PRILOG popuni polja za stanicu i godinu
                    TempData.Remove("fakturaGodPretraga");
                    notyf.Error("Nema podataka za izabrane kriterijume.", 3);
                    return RedirectToAction("StanicniRacun", new { stanica, fakturaGod, sveFakture, samoNfakture });
                }

                var viewModelList = mapper.Map<List<SrFakturaViewModel>>(query);

                foreach (var item in viewModelList)
                {
                    item.NazivStanice = context.ZsStanices.Where(x => x.SifraStanice1 == item.Stanica).Select(x => x.Naziv).FirstOrDefault();
                }

                ViewBag.stanicniRacuni = viewModelList.OrderByDescending(x => x.DatumIzdavanja);
                ViewBag.Admin = true;
                ViewBag.FakturaGod = string.Empty;
                ViewBag.Stanica = stanica;

                return View("StanicniRacun");
            }
            else
            {
                notyf.Error("Nema podataka za izabrane kriterijume.", 3);
            }

            return RedirectToAction("StanicniRacun");
        }




        public IActionResult Save(SrFakturaViewModel viewModel, string tipBlagajna,
            [ModelBinder(typeof(FakturaDatumModelBinder))] DateTime FakturaDatum,
            [ModelBinder(typeof(DatumIzdavanjaModelBinder))] DateTime DatumIzadavanja,
            [ModelBinder(typeof(FakturaDatumPModelBinder))] DateTime FakturaDatumP,
            [ModelBinder(typeof(FakturaDatumPuModelBinder))] DateTime FakturaDatumPU)
        {

            var query = context.SrFakturas.Where(x => x.Stanica == viewModel.Stanica && x.FakturaBroj == viewModel.FakturaBroj && x.FakturaGodina == viewModel.FakturaGodina);

            if (query.Any())
            {
                ViewBag.Stanica = TempData.Peek("StanicaNaziv");
                ViewBag.SifraStanice = viewModel.Stanica;
                ViewBag.MestoIzdavanjaRacuna = context.ZsStanices.Where(x => x.SifraStanice1 == viewModel.Stanica).Select(x => x.Mesto).FirstOrDefault();

                string bTip = viewModel.BlagajnaTip == "P" ? "Prispeća" : "Otpravljanja";

                ViewBag.BlagajnaTip = new SelectList(BlagajnaTip(), bTip );
                ViewBag.SifraBlagajne = TempData.Peek("SifraBlagajne");
                ViewBag.blagajnik = TempData.Peek("blagajnik");

                notyf.Error("Postoji već račun pod tim brojem.",5);
                ViewBag.stanicniRacuni = Enumerable.Empty<SrFakturaViewModel>();
                return View("StanicniRacun", viewModel);
            }

            string nullDate = "1/1/0001 12:00:00 AM";

            viewModel.FakturaDatum = FakturaDatum.ToString() != nullDate ? FakturaDatum : null;
            viewModel.DatumIzdavanja = DatumIzadavanja.ToString() != nullDate ? DatumIzadavanja : null ;
            viewModel.FakturaDatumP = FakturaDatumP.ToString() != nullDate ? FakturaDatumP : null ;
            viewModel.FakturaDatumPromet = FakturaDatumPU.ToString() != nullDate ? FakturaDatumPU: null ;

            viewModel.Realizovano = viewModel.Realizovano == null ? "N" : "D";

            viewModel.BlagajnaTip = tipBlagajna == "P" ? "P" : "O";

            var NazivStanice = context.ZsStanices.Where(x => x.SifraStanice1 == viewModel.Stanica).Select(x => x.Naziv).FirstOrDefault();
            var model = new SrFaktura();
            mapper.Map(viewModel, model);

            if (!directoryAndFiles.IsDirectoryExist(NazivStanice))
            {
                directoryAndFiles.CreateDirectory(NazivStanice);   
            }

            if (viewModel.UploadFile != null)
            {
                if (directoryAndFiles.IsFileExist(viewModel.UploadFile.FileName, NazivStanice))
                {
                    var path = $"{this.webHostEnvironment.WebRootPath}\\files\\" + NazivStanice + "\\" + viewModel.UploadFile.FileName;
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        viewModel.UploadFile.CopyTo(fileStream);
                    }
                    model.Fpath = "http://10.3.4.80/StR/files" + "/" + NazivStanice + "/" + viewModel.UploadFile.FileName; ;
                }
                else
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.UploadFile.FileName;
                    var path = $"{this.webHostEnvironment.WebRootPath}\\files\\" + NazivStanice + "\\" + uniqueFileName;
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        viewModel.UploadFile.CopyTo(fileStream);
                    }
                    model.Fpath = "http://10.3.4.80/StR/files" + "/" + NazivStanice + "/" + uniqueFileName;
                }
            }

          

            try
            {
                notyf.Success("Uspešno uneti podaci.", 3);
                context.SrFakturas.Add(model);
                context.SaveChanges();

                TempData["PoslednjiUnos"] = true;
                TempData["Stanica"] = viewModel.Stanica;
                TempData["FakturaBroj"] = viewModel.FakturaBroj;
               
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

            var viewModel = new SrFakturaViewModel();
            mapper.Map(query, viewModel);

            if (viewModel.Fpath != null)
            {
                string[] imeFajleArray = viewModel.Fpath.Split('/');
                ViewBag.ImeFajla = imeFajleArray[imeFajleArray.Length - 1];
            }

            var tipBlagajne = query.BlagajnaTip == "P" ? "Prispeća" : "Otpravljanja";
            ViewBag.BlagajnaTip = new SelectList(BlagajnaTip(), tipBlagajne);

            var modelTEMP = context.SrFakturas.Where(x => x.Stanica == user.Stanica).OrderByDescending(x => x.DatumIzdavanja);

            var viewModelList = mapper.Map<List<SrFakturaViewModel>>(modelTEMP);        // ovo mozda ce se pojednostavniti kada se resi kako da se prikaze putanja i ostalo

            foreach (var item in viewModelList)
            {
                item.NazivStanice = context.ZsStanices.Where(x => x.SifraStanice1 == item.Stanica).Select(x => x.Naziv).FirstOrDefault();
            }

            ViewBag.stanicniRacuni = viewModelList.OrderByDescending(x => x.DatumIzdavanja);

            return View("StanicniRacun", viewModel);
        }


        public IActionResult EditSave(SrFakturaViewModel viewModel, string tipBlagajna,
            [ModelBinder(typeof(FakturaDatumModelBinder))] DateTime FakturaDatum,
            [ModelBinder(typeof(DatumIzdavanjaModelBinder))] DateTime DatumIzadavanja,
            [ModelBinder(typeof(FakturaDatumPModelBinder))] DateTime FakturaDatumP,
            [ModelBinder(typeof(FakturaDatumPuModelBinder))] DateTime FakturaDatumPU)
        {
            string nullDate = "1/1/0001 12:00:00 AM";

            viewModel.FakturaDatum = FakturaDatum.ToString() != nullDate ? FakturaDatum : null;
            viewModel.DatumIzdavanja = DatumIzadavanja.ToString() != nullDate ? DatumIzadavanja : null;
            viewModel.FakturaDatumP = FakturaDatumP.ToString() != nullDate ? FakturaDatumP : null;
            viewModel.FakturaDatumPromet = FakturaDatumPU.ToString() != nullDate ? FakturaDatumPU : null;

            viewModel.BlagajnaTip = tipBlagajna == "Prispeća" ? "P" : "O";

            var NazivStanice = context.ZsStanices.Where(x => x.SifraStanice1 == viewModel.Stanica).Select(x => x.Naziv).FirstOrDefault();

            var model = new SrFaktura();
            mapper.Map(viewModel, model);

            if (!directoryAndFiles.IsDirectoryExist(NazivStanice))
            {
                directoryAndFiles.CreateDirectory(NazivStanice);
            }

            if (viewModel.UploadFile != null)
            {

                if (directoryAndFiles.IsFileExist(viewModel.UploadFile.FileName, NazivStanice))
                {
                    model.Fpath = viewModel.UploadFile.FileName;
                    var path = $"{this.webHostEnvironment.WebRootPath}\\files\\" + NazivStanice + "\\" + viewModel.UploadFile.FileName;
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        viewModel.UploadFile.CopyTo(fileStream);
                    }
                    model.Fpath = "http://10.3.4.80/StR/files" + "/" + NazivStanice + "/" + viewModel.UploadFile.FileName; 
                }
                else
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.UploadFile.FileName;
                    var path = $"{this.webHostEnvironment.WebRootPath}\\files\\" + NazivStanice + "\\" + uniqueFileName;
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        viewModel.UploadFile.CopyTo(fileStream);
                    }
                    model.Fpath = "http://10.3.4.80/StR/files" + "/" + NazivStanice + "/" + uniqueFileName; 
                }
            }

            try
            {
                var update = context.SrFakturas.Attach(model);
                update.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
                notyf.Success("Uspešna izmena", 3);
            }
            catch (Exception)
            {
                notyf.Error("Greška prilikom izmene podataka.", 5);
            }

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

            string stancaPretraga = (string)TempData.Peek("stancaPretraga");
            int fakturaGodPretraga = TempData.ContainsKey("fakturaGodPretraga") ? (int)TempData.Peek("fakturaGodPretraga") : 0;
            bool sveFakturePretraga = TempData.ContainsKey("sveFakturePretraga") ? (bool)TempData.Peek("sveFakturePretraga") : false;
            bool samoNfakturePretraga = TempData.ContainsKey("samoNfakturePretraga") ? (bool)TempData.Peek("samoNfakturePretraga") : true; //setovano je true jer kada udje na formu i ne vrsi nikakvu pretragu onda TempDate nisu setovane, a po defoltu je setovano da prikaze samo N fakture

            ViewBag.Stanica = stancaPretraga;
            ViewBag.FakturaGod = fakturaGodPretraga == 0 ? null : string.Empty;     //Da vrati na view-u unete paramtre pretrage
            ViewBag.sveFakture = sveFakturePretraga;
            ViewBag.samoNfakture = samoNfakturePretraga;
            ViewBag.Admin = true;

            ViewBag.stanicniRacuni = realizovanoPrilog.SearchResults(stancaPretraga, fakturaGodPretraga, sveFakturePretraga, samoNfakturePretraga);
            return View("StanicniRacun");
        }

        //public IActionResult Prilog(string stanica, string racunBr, int fakturaGod, bool sveFakture, bool samoNfakture)
        //{
        //    var query = context.SrFakturas.Where(x => x.Stanica == stanica && x.FakturaBroj == racunBr && x.FakturaGodina == fakturaGod).Select(x => x.Fpath).FirstOrDefault();

        //    if (query == null)
        //    {
        //        notyf.Information("Nema priloga", 3);
        //    } else
        //    {
        //        try
        //        {
                   
        //            Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
        //            GetLearningReport();

        //        }
        //        catch (Exception)
        //        {
        //            notyf.Error("Neispravna putanja ili fajl.", 5);
        //        }
        //    }

        //    string stancaPretraga = (string)TempData.Peek("stancaPretraga");
        //    int fakturaGodPretraga = TempData.ContainsKey("fakturaGodPretraga") ? (int)TempData.Peek("fakturaGodPretraga") : 0;
        //    bool sveFakturePretraga = TempData.ContainsKey("sveFakturePretraga")? (bool)TempData.Peek("sveFakturePretraga") : false;
        //    bool samoNfakturePretraga = TempData.ContainsKey("samoNfakturePretraga") ? (bool)TempData.Peek("samoNfakturePretraga") : true; //setovano je true jer kada udje na formu i ne vrsi nikakvu pretragu onda TempDate nisu setovane, a po defoltu je setovano da prikaze samo N fakture

        //    ViewBag.Stanica = stancaPretraga;
        //    ViewBag.FakturaGod = fakturaGodPretraga == 0 ? null : fakturaGodPretraga.ToString();     //Da vrati na view-u unete paramtre pretrage
        //    ViewBag.sveFakture = sveFakturePretraga;
        //    ViewBag.samoNfakture = samoNfakturePretraga;
        //    ViewBag.Admin = true;

        //    ViewBag.stanicniRacuni = realizovanoPrilog.SearchResults(stancaPretraga, fakturaGodPretraga, sveFakturePretraga, samoNfakturePretraga);
        //    return View("StanicniRacun");

        //}

        
 


    }
}
