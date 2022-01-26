using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SK_Stanicni_Racuni.CustomModelBinding.Datumi
{
    public class DatumOdModelBinder:  IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueProviderResult = bindingContext.ValueProvider.GetValue("datumOd");


            var nesto = bindingContext.HttpContext.Request.Body;

            var data = bindingContext.HttpContext.Request.Form;
            var resultDatum = data.TryGetValue("datumOd", out var VaziOd);

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
