﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SK_Stanicni_Racuni.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SK_Stanicni_Racuni.Controllers
{
    public class ListaStanicaController : Controller
    {
        private readonly AppDbContext context;

        public ListaStanicaController(AppDbContext context)
        {
            this.context = context;
        }

 

        public IActionResult ListaMedjunarodnihStanica()
        {
            var stanice = (from s in context.UicStanices
                           select s.SifraStanice).ToList().Take(10);           

            return Json(stanice);
        }




        public ActionResult ListaUnutrasnjihStanica(string data)
        {
            var stanice = (from s in context.ZsStanices
                           where s.Naziv.Contains(data)
                           select s.Naziv).ToList().Take(10);

            return Json(stanice);
        }
    }
}
