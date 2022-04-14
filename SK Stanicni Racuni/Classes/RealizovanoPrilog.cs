using AutoMapper;
using SK_Stanicni_Racuni.Models;
using SK_Stanicni_Racuni.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SK_Stanicni_Racuni.Classes
{
    
    //Da vrati na viewu sve uslove i rezultate pretrage kada klikne na dugme Realizovano ili Prilog
    public class RealizovanoPrilog
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;

        public RealizovanoPrilog(AppDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public List<SrFakturaViewModel> SearchResults(string stancaPretraga, int fakturaGodPretraga, bool sveFakturePretraga, bool samoNfakturePretraga)
        {
            string stanicaPretragaSifra = string.Empty;

            if (stancaPretraga != null)
            {
                stanicaPretragaSifra = context.ZsStanices.Where(x => x.Naziv == stancaPretraga).Select(x => x.SifraStanice1).FirstOrDefault();
            }

            var queryPretraga = Enumerable.Empty<SrFaktura>();

            if (sveFakturePretraga)
            {
                if (string.IsNullOrEmpty(stanicaPretragaSifra) && fakturaGodPretraga == 0)
                {
                    queryPretraga = context.SrFakturas.OrderByDescending(x => x.DatumIzdavanja);
                }
                else if (!string.IsNullOrEmpty(stanicaPretragaSifra) && fakturaGodPretraga == 0)
                {
                    queryPretraga = context.SrFakturas.Where(x => x.Stanica == stanicaPretragaSifra).OrderByDescending(x => x.DatumIzdavanja);
                }
                else if (!string.IsNullOrEmpty(stanicaPretragaSifra) && fakturaGodPretraga != 0)
                {
                    queryPretraga = context.SrFakturas.Where(x => x.Stanica == stanicaPretragaSifra && x.FakturaGodina == fakturaGodPretraga).OrderByDescending(x => x.DatumIzdavanja);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(stanicaPretragaSifra) && fakturaGodPretraga == 0)
                {
                    queryPretraga = context.SrFakturas.Where(x => x.Realizovano == "N").OrderByDescending(x => x.DatumIzdavanja);
                }
                else if (!string.IsNullOrEmpty(stanicaPretragaSifra) && fakturaGodPretraga == 0)
                {
                    queryPretraga = context.SrFakturas.Where(x => x.Stanica == stanicaPretragaSifra && x.Realizovano == "N").OrderByDescending(x => x.DatumIzdavanja);
                }
                else if (!string.IsNullOrEmpty(stanicaPretragaSifra) && fakturaGodPretraga != 0)
                {
                    queryPretraga = context.SrFakturas.Where(x => x.Stanica == stanicaPretragaSifra && x.FakturaGodina == fakturaGodPretraga && x.Realizovano == "N").OrderByDescending(x=>x.DatumIzdavanja);
                }

            }

            var viewModelList = mapper.Map<List<SrFakturaViewModel>>(queryPretraga);

            foreach (var item in viewModelList)
            {
                item.NazivStanice = context.ZsStanices.Where(x => x.SifraStanice1 == item.Stanica).Select(x => x.Naziv).FirstOrDefault();
            }

            return viewModelList;
        }
    }
}
