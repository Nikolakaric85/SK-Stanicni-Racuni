using AutoMapper;
using SK_Stanicni_Racuni.Models;
using SK_Stanicni_Racuni.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SK_Stanicni_Racuni.Classes
{
    public class ApplicationMapper: Profile
    {
        public ApplicationMapper()
        {
            CreateMap<SrFaktura, SrFakturaViewModel>().ReverseMap();
        }
    }
}



//instaliras nuget paket  AutoMapper.Extensions.Microsoft.DependencyInjection
// startup.cs dodas u ConfigureServices dodas  services.AddAutoMapper(typeof(ApplicationMapper));
//https://www.youtube.com/watch?v=tWXuOBNQh4o&t=430s&ab_channel=kudvenkat
//https://www.youtube.com/watch?v=rlG6fx4OL_8&ab_channel=WebGentle