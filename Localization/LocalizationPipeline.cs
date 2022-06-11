using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MiniFacebook.Localization
{
    public class LocalizationPipeline
    {
        public void Configure(IApplicationBuilder app)
        {

            var supportedCultures = new List<CultureInfo>();    
            var ar = new CultureInfo("ar");
            ar.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
            ar.DateTimeFormat.DateSeparator = "/";
            ar.DateTimeFormat.Calendar = new GregorianCalendar();
            supportedCultures.Add(ar);

            var en = new CultureInfo("en");
            en.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            en.DateTimeFormat.DateSeparator = "/";
            en.DateTimeFormat.Calendar = new GregorianCalendar();
            supportedCultures.Add(en);


            var options = new RequestLocalizationOptions()
            {

                DefaultRequestCulture = new RequestCulture(culture: "en", uiCulture: "en"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };

            options.RequestCultureProviders = new[] { new RouteDataRequestCultureProvider() { Options = options, RouteDataStringKey = "lang", UIRouteDataStringKey = "lang" } };

            app.UseRequestLocalization(options);
        }
    }
}
