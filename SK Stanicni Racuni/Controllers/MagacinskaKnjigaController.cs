using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SK_Stanicni_Racuni.CustomModelBinding.RacuniUnutrasnjiSaobracaj;
using SK_Stanicni_Racuni.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SK_Stanicni_Racuni.Controllers
{
    public class MagacinskaKnjigaController : Controller
    {
        private readonly AppDbContext context;

        public MagacinskaKnjigaController(AppDbContext context)
        {
            this.context = context;
        }

        public IActionResult MagacinskaKnjiga()
        {
            return View();
        }


        public IActionResult Print(string stanica, [ModelBinder(typeof(DatumOdModelBinder))] DateTime DatumOd,
         [ModelBinder(typeof(DatumDoModelBinder))] DateTime DatumDo)
        {

            var qu = context.SlogKalks.FromSqlRaw("select kalk.OtpBroj as [Broj tovarnog lista], "+
                                                     "kalk.OtpDatum as [Predato dana], " +
                                                     "kalk.PrStanica as [Uputna stanica], " +
                                                     "kalk.UkupnoKola as [Broj kola], " +
                                                     "roba.NHM as [Naznačenje vrste rove], " +
                                                     "roba.SMasa as [Navedena u tovarnom listu], " +
                                                     "kola.IBK as Ibk " +
                                                     "from SlogKalk kalk join SlogKola kola on kalk.RecID = kola.RecID " +
                                                     "join SlogRoba roba on kola.RecID = roba.RecID and kola.KolaStavka = roba.KolaStavka " +
                                                     "where kalk.Saobracaj in (1, 3) " +
                                                     "and kalk.OtpStanica = '7221001' " +
                                                     "and kalk.OtpDatum between '2022-01-01 00:00:00' and '2022-01-05 00:00:00' ").AsNoTracking();

            var query = from kalk in context.SlogKalks
                        join kola in context.SlogKolas on kalk.RecId equals kola.RecId
                        join roba in context.SlogRobas on new { RecId = kola.RecId, KolaStavka = kola.KolaStavka } equals
                                                          new { RecId = roba.RecId, KolaStavka = roba.KolaStavka }
                        where (kalk.OtpDatum >= DatumOd || kalk.OtpDatum <= DatumDo)
                        && kalk.OtpStanica == stanica
                        && "where kalk.Saobracaj in (1, 3) " +

                        select kalk;


            return new EmptyResult();
        }



        }
}
