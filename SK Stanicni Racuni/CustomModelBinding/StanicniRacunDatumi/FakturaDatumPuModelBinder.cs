using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SK_Stanicni_Racuni.CustomModelBinding.StanicniRacunDatumi
{
    public class FakturaDatumPuModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var data = bindingContext.HttpContext.Request.Form;
            var resultDatum = data.TryGetValue("DatumPU", out var VaziOd);

            if (resultDatum)
            {
                try
                {
                    var TestTime = DateTime.ParseExact(VaziOd, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    bindingContext.Result = ModelBindingResult.Success(TestTime);
                }
                catch (Exception)
                {

                }
            }

            return Task.CompletedTask;
        }
    }
}
