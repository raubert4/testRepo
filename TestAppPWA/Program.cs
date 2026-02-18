using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using System.Globalization;
using TestAppPWA;
using TestAppPWA.Repository;
using TestAppPWA.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddRadzenComponents();

builder.Services.AddLocalization(options => options.ResourcesPath = "");

// This line registers IHttpClientFactory and enables dynamic client creation
builder.Services.AddHttpClient();
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


IndexedDbStorageDataRepository.CreateDatabase(builder.Services);
builder.Services.AddScoped<IDataRepository, IndexedDbStorageDataRepository>();

builder.Services.AddScoped<IApiService, ArApiService>();

var host = builder.Build();

var dr = host.Services.GetRequiredService<IDataRepository>();

string cultureKey = "en";
var us = await dr.GetUserSettingsAsync();
if (us?.OptionLang == TestAppPWA.Utils.LangUI.FR)
{
    cultureKey = "fr";
}
else if (us?.OptionLang == TestAppPWA.Utils.LangUI.DE)
{
    cultureKey = "de";
}

var culture = new CultureInfo(cultureKey);
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

await host.RunAsync();

